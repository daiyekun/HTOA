using CWHT.OA.Application.DTOs;
using CWHT.OA.Domain.Entities.System;
using FreeSql;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CWHT.OA.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IFreeSql _fsql;

    public UserController(IFreeSql fsql)
    {
        _fsql = fsql;
    }

    [HttpGet("list")]
    public async Task<ApiResponse<PagedResult<UserListItem>>> GetList([FromQuery] PagedInput input)
    {
        var query = _fsql.Select<User>()
            .Where(u => u.IsDeleted == 0);

        if (!string.IsNullOrEmpty(input.Keyword))
        {
            query = query.Where(u => u.Username.Contains(input.Keyword) || u.RealName.Contains(input.Keyword));
        }

        var total = await query.CountAsync();
        var list = await query
            .Include(u => u.Department)
            .Include(u => u.Position)
            .OrderByDescending(u => u.CreateTime)
            .Page(input.Page, input.PageSize)
            .ToListAsync(u => new UserListItem
            {
                Id = u.Id,
                Username = u.Username,
                RealName = u.RealName,
                Phone = u.Phone,
                Email = u.Email,
                Avatar = u.Avatar,
                Gender = u.Gender,
                Status = u.Status,
                DepartmentId = u.DepartmentId,
                DepartmentName = u.Department!.Name,
                PositionId = u.PositionId,
                PositionName = u.Position!.Name,
                CreateTime = u.CreateTime
            });

        var result = new PagedResult<UserListItem>
        {
            Items = list,
            Total = total,
            Page = input.Page,
            PageSize = input.PageSize
        };

        return ApiResponse<PagedResult<UserListItem>>.SuccessResult(result);
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<User>> GetById(long id)
    {
        var user = await _fsql.Select<User>()
            .Where(u => u.Id == id && u.IsDeleted == 0)
            .Include(u => u.Department)
            .Include(u => u.Position)
            .FirstAsync();

        if (user == null)
        {
            return ApiResponse<User>.FailResult("用户不存在");
        }

        return ApiResponse<User>.SuccessResult(user);
    }

    [HttpPost]
    public async Task<ApiResponse<long>> Create([FromBody] CreateUserInput input)
    {
        var exists = await _fsql.Select<User>()
            .Where(u => u.Username == input.Username && u.IsDeleted == 0)
            .AnyAsync();

        if (exists)
        {
            return ApiResponse<long>.FailResult("用户名已存在");
        }

        var salt = Guid.NewGuid().ToString("N")[..8];
        var user = new User
        {
            Username = input.Username,
            Password = HashPassword(input.Password, salt),
            Salt = salt,
            RealName = input.RealName,
            Phone = input.Phone,
            Email = input.Email,
            Gender = input.Gender,
            Status = 1,
            DepartmentId = input.DepartmentId,
            PositionId = input.PositionId,
            CreateTime = DateTime.Now
        };

        var id = await _fsql.Insert(user).ExecuteIdentityAsync();
        return ApiResponse<long>.SuccessResult(id, "创建成功");
    }

    [HttpPut("{id}")]
    public async Task<ApiResponse> Update(long id, [FromBody] UpdateUserInput input)
    {
        var user = await _fsql.Select<User>().Where(u => u.Id == id && u.IsDeleted == 0).FirstAsync();
        if (user == null)
        {
            return ApiResponse.Fail("用户不存在");
        }

        await _fsql.Update<User>(id)
            .Set(u => u.RealName, input.RealName)
            .Set(u => u.Phone, input.Phone)
            .Set(u => u.Email, input.Email)
            .Set(u => u.Gender, input.Gender)
            .Set(u => u.DepartmentId, input.DepartmentId)
            .Set(u => u.PositionId, input.PositionId)
            .Set(u => u.UpdateTime, DateTime.Now)
            .ExecuteAffrowsAsync();

        return ApiResponse.Success("更新成功");
    }

    [HttpDelete("{id}")]
    public async Task<ApiResponse> Delete(long id)
    {
        await _fsql.Update<User>(id)
            .Set(u => u.IsDeleted, 1)
            .Set(u => u.UpdateTime, DateTime.Now)
            .ExecuteAffrowsAsync();

        return ApiResponse.Success("删除成功");
    }

    [HttpPut("{id}/status")]
    public async Task<ApiResponse> UpdateStatus(long id, [FromBody] int status)
    {
        await _fsql.Update<User>(id)
            .Set(u => u.Status, status)
            .Set(u => u.UpdateTime, DateTime.Now)
            .ExecuteAffrowsAsync();

        return ApiResponse.Success("状态更新成功");
    }

    [HttpPut("{id}/password")]
    public async Task<ApiResponse> ResetPassword(long id, [FromBody] ResetPasswordInput input)
    {
        var salt = Guid.NewGuid().ToString("N")[..8];
        await _fsql.Update<User>(id)
            .Set(u => u.Password, HashPassword(input.Password, salt))
            .Set(u => u.Salt, salt)
            .Set(u => u.UpdateTime, DateTime.Now)
            .ExecuteAffrowsAsync();

        return ApiResponse.Success("密码重置成功");
    }

    private static string HashPassword(string password, string salt)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var combinedBytes = System.Text.Encoding.UTF8.GetBytes(password + salt);
        var hashBytes = sha256.ComputeHash(combinedBytes);
        return Convert.ToBase64String(hashBytes);
    }
}

public class UserListItem
{
    public long Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string RealName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Avatar { get; set; }
    public int Gender { get; set; }
    public int Status { get; set; }
    public long? DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
    public long? PositionId { get; set; }
    public string? PositionName { get; set; }
    public DateTime CreateTime { get; set; }
}

public class CreateUserInput
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = "123456";
    public string RealName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public int Gender { get; set; }
    public long? DepartmentId { get; set; }
    public long? PositionId { get; set; }
}

public class UpdateUserInput
{
    public string RealName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public int Gender { get; set; }
    public long? DepartmentId { get; set; }
    public long? PositionId { get; set; }
}

public class ResetPasswordInput
{
    public string Password { get; set; } = "123456";
}
