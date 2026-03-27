using CWHT.OA.Application.DTOs;
using CWHT.OA.Domain.Entities.System;
using FreeSql;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CWHT.OA.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DepartmentController : ControllerBase
{
    private readonly IFreeSql _fsql;

    public DepartmentController(IFreeSql fsql)
    {
        _fsql = fsql;
    }

    [HttpGet("tree")]
    public async Task<ApiResponse<List<DepartmentTreeItem>>> GetTree([FromQuery] string? keyword)
    {
        var query = _fsql.Select<Department>().Where(d => d.Status == 1);

        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(d => d.Name.Contains(keyword));
        }

        var list = await query.OrderBy(d => d.Sort).ToListAsync();

        var tree = BuildTree(list, null);
        return ApiResponse<List<DepartmentTreeItem>>.SuccessResult(tree);
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<Department>> GetById(long id)
    {
        var dept = await _fsql.Select<Department>().Where(d => d.Id == id).FirstAsync();
        if (dept == null)
        {
            return ApiResponse<Department>.FailResult("部门不存在");
        }
        return ApiResponse<Department>.SuccessResult(dept);
    }

    [HttpPost]
    public async Task<ApiResponse<long>> Create([FromBody] Department input)
    {
        input.CreateTime = DateTime.Now;
        var id = await _fsql.Insert(input).ExecuteIdentityAsync();
        return ApiResponse<long>.SuccessResult(id, "创建成功");
    }

    [HttpPut("{id}")]
    public async Task<ApiResponse> Update(long id, [FromBody] Department input)
    {
        await _fsql.Update<Department>(id)
            .Set(d => d.Name, input.Name)
            .Set(d => d.ParentId, input.ParentId)
            .Set(d => d.Sort, input.Sort)
            .Set(d => d.Leader, input.Leader)
            .Set(d => d.Phone, input.Phone)
            .Set(d => d.Email, input.Email)
            .Set(d => d.Status, input.Status)
            .Set(d => d.UpdateTime, DateTime.Now)
            .ExecuteAffrowsAsync();

        return ApiResponse.Success("更新成功");
    }

    [HttpDelete("{id}")]
    public async Task<ApiResponse> Delete(long id)
    {
        var hasChild = await _fsql.Select<Department>().Where(d => d.ParentId == id).AnyAsync();
        if (hasChild)
        {
            return ApiResponse.Fail("存在子部门，不能删除");
        }

        var hasUser = await _fsql.Select<User>().Where(u => u.DepartmentId == id && u.IsDeleted == 0).AnyAsync();
        if (hasUser)
        {
            return ApiResponse.Fail("部门下存在用户，不能删除");
        }

        await _fsql.Delete<Department>(id).ExecuteAffrowsAsync();
        return ApiResponse.Success("删除成功");
    }

    private List<DepartmentTreeItem> BuildTree(List<Department> list, long? parentId)
    {
        return list
            .Where(d => d.ParentId == parentId)
            .Select(d => new DepartmentTreeItem
            {
                Id = d.Id,
                Name = d.Name,
                ParentId = d.ParentId,
                Sort = d.Sort,
                Leader = d.Leader,
                Phone = d.Phone,
                Email = d.Email,
                Status = d.Status,
                Children = BuildTree(list, d.Id)
            })
            .ToList();
    }
}

public class DepartmentTreeItem
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public long? ParentId { get; set; }
    public int Sort { get; set; }
    public string? Leader { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public int Status { get; set; }
    public List<DepartmentTreeItem> Children { get; set; } = [];
}
