using CWHT.OA.Application.DTOs;
using CWHT.OA.Domain.Entities.System;
using FreeSql;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CWHT.OA.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MenuController : ControllerBase
{
    private readonly IFreeSql _fsql;

    public MenuController(IFreeSql fsql)
    {
        _fsql = fsql;
    }

    [HttpGet("tree")]
    public async Task<ApiResponse<List<MenuTreeItem>>> GetTree()
    {
        var list = await _fsql.Select<Menu>()
            .Where(m => m.Status == 1)
            .OrderBy(m => m.Sort)
            .ToListAsync();

        var tree = BuildTree(list, null);
        return ApiResponse<List<MenuTreeItem>>.SuccessResult(tree);
    }

    [HttpGet("user-menus")]
    public async Task<ApiResponse<List<MenuTreeItem>>> GetUserMenus()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return ApiResponse<List<MenuTreeItem>>.FailResult("未登录");
        }

        var isAdmin = User.FindFirst("IsAdmin")?.Value == "True";

        List<Menu> menus;
        if (isAdmin)
        {
            menus = await _fsql.Select<Menu>()
                .Where(m => m.Status == 1)
                .OrderBy(m => m.Sort)
                .ToListAsync();
        }
        else
        {
            var roleIds = await _fsql.Select<UserRole>()
                .Where(ur => ur.UserId == long.Parse(userId))
                .ToListAsync(ur => ur.RoleId);

            var menuIds = await _fsql.Select<RoleMenu>()
                .Where(rm => roleIds.Contains(rm.RoleId))
                .Distinct()
                .ToListAsync(rm => rm.MenuId);

            menus = await _fsql.Select<Menu>()
                .Where(m => m.Status == 1 && menuIds.Contains(m.Id))
                .OrderBy(m => m.Sort)
                .ToListAsync();
        }

        var tree = BuildTree(menus, null);
        return ApiResponse<List<MenuTreeItem>>.SuccessResult(tree);
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<Menu>> GetById(long id)
    {
        var menu = await _fsql.Select<Menu>().Where(m => m.Id == id).FirstAsync();
        if (menu == null)
        {
            return ApiResponse<Menu>.FailResult("菜单不存在");
        }
        return ApiResponse<Menu>.SuccessResult(menu);
    }

    [HttpPost]
    public async Task<ApiResponse<long>> Create([FromBody] Menu input)
    {
        input.CreateTime = DateTime.Now;
        var id = await _fsql.Insert(input).ExecuteIdentityAsync();
        return ApiResponse<long>.SuccessResult(id, "创建成功");
    }

    [HttpPut("{id}")]
    public async Task<ApiResponse> Update(long id, [FromBody] Menu input)
    {
        await _fsql.Update<Menu>(id)
            .Set(m => m.Name, input.Name)
            .Set(m => m.ParentId, input.ParentId)
            .Set(m => m.Sort, input.Sort)
            .Set(m => m.Path, input.Path)
            .Set(m => m.Component, input.Component)
            .Set(m => m.Type, input.Type)
            .Set(m => m.Permission, input.Permission)
            .Set(m => m.Icon, input.Icon)
            .Set(m => m.IsVisible, input.IsVisible)
            .Set(m => m.Status, input.Status)
            .Set(m => m.UpdateTime, DateTime.Now)
            .ExecuteAffrowsAsync();

        return ApiResponse.Success("更新成功");
    }

    [HttpDelete("{id}")]
    public async Task<ApiResponse> Delete(long id)
    {
        var hasChild = await _fsql.Select<Menu>().Where(m => m.ParentId == id).AnyAsync();
        if (hasChild)
        {
            return ApiResponse.Fail("存在子菜单，不能删除");
        }

        await _fsql.Delete<Menu>(id).ExecuteAffrowsAsync();
        return ApiResponse.Success("删除成功");
    }

    private List<MenuTreeItem> BuildTree(List<Menu> list, long? parentId)
    {
        return list
            .Where(m => m.ParentId == parentId)
            .Select(m => new MenuTreeItem
            {
                Id = m.Id,
                Name = m.Name,
                ParentId = m.ParentId,
                Sort = m.Sort,
                Path = m.Path,
                Component = m.Component,
                Type = m.Type,
                Permission = m.Permission,
                Icon = m.Icon,
                IsVisible = m.IsVisible,
                Status = m.Status,
                Children = BuildTree(list, m.Id)
            })
            .ToList();
    }
}

public class MenuTreeItem
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public long? ParentId { get; set; }
    public int Sort { get; set; }
    public string? Path { get; set; }
    public string? Component { get; set; }
    public int Type { get; set; }
    public string? Permission { get; set; }
    public string? Icon { get; set; }
    public int IsVisible { get; set; }
    public int Status { get; set; }
    public List<MenuTreeItem> Children { get; set; } = [];
}
