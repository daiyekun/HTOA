using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CWHT.OA.Application.DTOs;
using CWHT.OA.Application.DTOs.Auth;
using CWHT.OA.Domain.Entities.System;
using FreeSql;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CWHT.OA.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IFreeSql _fsql;
    private readonly IConfiguration _configuration;

    public AuthController(IFreeSql fsql, IConfiguration configuration)
    {
        _fsql = fsql;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<ApiResponse<LoginOutput>> Login([FromBody] LoginInput input)
    {
        var user = await _fsql.Select<User>()
            .Where(u => u.Username == input.Username && u.IsDeleted == 0)
            .Include(u => u.Department)
            .FirstAsync();

        if (user == null)
        {
            return ApiResponse<LoginOutput>.FailResult("用户名或密码错误");
        }

        if (user.Status != 1)
        {
            return ApiResponse<LoginOutput>.FailResult("账号已被禁用");
        }

        var hashedPassword = HashPassword(input.Password, user.Salt);
        if (hashedPassword != user.Password)
        {
            return ApiResponse<LoginOutput>.FailResult("用户名或密码错误");
        }

        // 获取用户角色
        var roles = await _fsql.Select<UserRole>()
            .Where(ur => ur.UserId == user.Id)
            .Include(ur => ur.Role)
            .ToListAsync();

        var roleCodes = roles.Select(r => r.Role?.Code ?? "").ToList();

        // 生成JWT Token
        var token = GenerateToken(user, roleCodes);

        // 更新最后登录时间
        await _fsql.Update<User>(user.Id)
            .Set(u => u.LastLoginTime, DateTime.Now)
            .ExecuteAffrowsAsync();

        var output = new LoginOutput
        {
            Token = token,
            RefreshToken = Guid.NewGuid().ToString("N"),
            ExpireTime = DateTime.Now.AddMinutes(1440),
            UserInfo = new UserInfo
            {
                Id = user.Id,
                Username = user.Username,
                RealName = user.RealName,
                Avatar = user.Avatar,
                Email = user.Email,
                Phone = user.Phone,
                DepartmentId = user.DepartmentId,
                DepartmentName = user.Department?.Name,
                Roles = roleCodes
            }
        };

        return ApiResponse<LoginOutput>.SuccessResult(output, "登录成功");
    }

    [HttpPost("logout")]
    public ApiResponse Logout()
    {
        return ApiResponse.Success("退出成功");
    }

    [HttpGet("getUserInfo")]
    public async Task<ApiResponse<UserInfo>> GetUserInfo()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return ApiResponse<UserInfo>.FailResult("未登录");
        }

        var user = await _fsql.Select<User>()
            .Where(u => u.Id == long.Parse(userId) && u.IsDeleted == 0)
            .Include(u => u.Department)
            .FirstAsync();

        if (user == null)
        {
            return ApiResponse<UserInfo>.FailResult("用户不存在");
        }

        var roles = await _fsql.Select<UserRole>()
            .Where(ur => ur.UserId == user.Id)
            .Include(ur => ur.Role)
            .ToListAsync();

        var roleCodes = roles.Select(r => r.Role?.Code ?? "").ToList();

        var userInfo = new UserInfo
        {
            Id = user.Id,
            Username = user.Username,
            RealName = user.RealName,
            Avatar = user.Avatar,
            Email = user.Email,
            Phone = user.Phone,
            DepartmentId = user.DepartmentId,
            DepartmentName = user.Department?.Name,
            Roles = roleCodes
        };

        return ApiResponse<UserInfo>.SuccessResult(userInfo);
    }

    private string GenerateToken(User user, List<string> roles)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"]!;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new("RealName", user.RealName),
            new("IsAdmin", user.IsAdmin.ToString())
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(int.Parse(jwtSettings["ExpirationMinutes"] ?? "1440")),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string HashPassword(string password, string salt)
    {
        using var sha256 = SHA256.Create();
        var combinedBytes = Encoding.UTF8.GetBytes(password + salt);
        var hashBytes = sha256.ComputeHash(combinedBytes);
        return Convert.ToBase64String(hashBytes);
    }
}
