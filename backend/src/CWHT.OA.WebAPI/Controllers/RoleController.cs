using CWHT.OA.Application.DTOs;
using CWHT.OA.Domain.Entities.System;
using FreeSql;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CWHT.OA.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RoleController : ControllerBase
{
    private readonly IFreeSql _fsql;

    public RoleController(IFreeSql fsql)
    {
        _fsql = fsql;
    }

    [HttpGet("list")]
    public async Task<ApiResponse<PagedResult<Role>>> GetList([FromQuery] PagedInput input)
    {
        var query = _fsql.Select<Role>();

        if (!string.IsNullOrEmpty(input.Keyword))
        {
            query = query.Where(r => r.Name.Contains(input.Keyword) || r.Code.Contains(input.Keyword));
        }

        var total = await query.CountAsync();
        var list = await query
            .OrderByDescending(r => r.CreateTime)
            .Page(input.Page, input.PageSize)
            .ToListAsync();

        var result = new PagedResult<Role>
        {
            Items = list,
            Total = total,
            Page = input.Page,
            PageSize = input.PageSize
        };

        return ApiResponse<PagedResult<Role>>.SuccessResult(result);
    }

    [HttpGet("all")]
    public async Task<ApiResponse<List<Role>>> GetAll()
    {
        var list = await _fsql.Select<Role>()
            .Where(r => r.Status == 1)
            .OrderBy(r => r.Sort)
            .ToListAsync();

        return ApiResponse<List<Role>>.SuccessResult(list);
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<Role>> GetById(long id)
    {
        var role = await _fsql.Select<Role>().Where(r => r.Id == id).FirstAsync();
        if (role == null)
        {
            return ApiResponse<Role>.FailResult("角色不存在");
        }
        return ApiResponse<Role>.SuccessResult(role);
    }

    [HttpGet("{id}/menus")]
    public async Task<ApiResponse<List<long>>> GetRoleMenus(long id)
    {
        var menuIds = await _fsql.Select<RoleMenu>()
            .Where(rm => rm.RoleId == id)
            .ToListAsync(rm => rm.MenuId);

        return ApiResponse<List<long>>.SuccessResult(menuIds);
    }

    [HttpPost]
    public async Task<ApiResponse<long>> Create([FromBody] CreateRoleInput input)
    {
        var exists = await _fsql.Select<Role>()
            .Where(r => r.Code == input.Role.Code)
            .AnyAsync();

        if (exists)
        {
            return ApiResponse<long>.FailResult("角色编码已存在");
        }

        input.Role.CreateTime = DateTime.Now;
        var id = await _fsql.Insert(input.Role).ExecuteIdentityAsync();

        // 保存菜单关联
        if (input.MenuIds?.Count > 0)
        {
            var roleMenus = input.MenuIds.Select(menuId => new RoleMenu
            {
                RoleId = id,
                MenuId = menuId
            }).ToList();

            await _fsql.Insert(roleMenus).ExecuteAffrowsAsync();
        }

        return ApiResponse<long>.SuccessResult(id, "创建成功");
    }

    [HttpPut("{id}")]
    public async Task<ApiResponse> Update(long id, [FromBody] UpdateRoleInput input)
    {
        await _fsql.Update<Role>(id)
            .Set(r => r.Name, input.Role.Name)
            .Set(r => r.Sort, input.Role.Sort)
            .Set(r => r.DataScope, input.Role.DataScope)
            .Set(r => r.Status, input.Role.Status)
            .Set(r => r.Remark, input.Role.Remark)
            .Set(r => r.UpdateTime, DateTime.Now)
            .ExecuteAffrowsAsync();

        // 更新菜单关联
        await _fsql.Delete<RoleMenu>().Where(rm => rm.RoleId == id).ExecuteAffrowsAsync();

        if (input.MenuIds?.Count > 0)
        {
            var roleMenus = input.MenuIds.Select(menuId => new RoleMenu
            {
                RoleId = id,
                MenuId = menuId
            }).ToList();

            await _fsql.Insert(roleMenus).ExecuteAffrowsAsync();
        }

        return ApiResponse.Success("更新成功");
    }

    [HttpDelete("{id}")]
    public async Task<ApiResponse> Delete(long id)
    {
        var hasUser = await _fsql.Select<UserRole>().Where(ur => ur.RoleId == id).AnyAsync();
        if (hasUser)
        {
            return ApiResponse.Fail("角色已分配用户，不能删除");
        }

        await _fsql.Delete<Role>(id).ExecuteAffrowsAsync();
        await _fsql.Delete<RoleMenu>().Where(rm => rm.RoleId == id).ExecuteAffrowsAsync();

        return ApiResponse.Success("删除成功");
    }
}

public class CreateRoleInput
{
    public Role Role { get; set; } = new();
    public List<long>? MenuIds { get; set; }
}

public class UpdateRoleInput
{
    public Role Role { get; set; } = new();
    public List<long>? MenuIds { get; set; }
}
