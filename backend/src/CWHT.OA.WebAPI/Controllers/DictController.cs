using CWHT.OA.Application.DTOs;
using CWHT.OA.Domain.Entities.System;
using FreeSql;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CWHT.OA.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DictController : ControllerBase
{
    private readonly IFreeSql _fsql;

    public DictController(IFreeSql fsql)
    {
        _fsql = fsql;
    }

    [HttpGet("types")]
    public async Task<ApiResponse<PagedResult<DictType>>> GetTypes([FromQuery] PagedInput input)
    {
        var query = _fsql.Select<DictType>();

        if (!string.IsNullOrEmpty(input.Keyword))
        {
            query = query.Where(d => d.Name.Contains(input.Keyword) || d.Code.Contains(input.Keyword));
        }

        var total = await query.CountAsync();
        var list = await query
            .OrderByDescending(d => d.CreateTime)
            .Page(input.Page, input.PageSize)
            .ToListAsync();

        return ApiResponse<PagedResult<DictType>>.SuccessResult(new PagedResult<DictType>
        {
            Items = list,
            Total = total,
            Page = input.Page,
            PageSize = input.PageSize
        });
    }

    [HttpGet("types/{id}")]
    public async Task<ApiResponse<DictType>> GetTypeById(long id)
    {
        var dictType = await _fsql.Select<DictType>().Where(d => d.Id == id).FirstAsync();
        if (dictType == null)
        {
            return ApiResponse<DictType>.FailResult("字典类型不存在");
        }
        return ApiResponse<DictType>.SuccessResult(dictType);
    }

    [HttpPost("types")]
    public async Task<ApiResponse<long>> CreateType([FromBody] DictType input)
    {
        input.CreateTime = DateTime.Now;
        var id = await _fsql.Insert(input).ExecuteIdentityAsync();
        return ApiResponse<long>.SuccessResult(id, "创建成功");
    }

    [HttpPut("types/{id}")]
    public async Task<ApiResponse> UpdateType(long id, [FromBody] DictType input)
    {
        await _fsql.Update<DictType>(id)
            .Set(d => d.Name, input.Name)
            .Set(d => d.Code, input.Code)
            .Set(d => d.Status, input.Status)
            .Set(d => d.Remark, input.Remark)
            .Set(d => d.UpdateTime, DateTime.Now)
            .ExecuteAffrowsAsync();

        return ApiResponse.Success("更新成功");
    }

    [HttpDelete("types/{id}")]
    public async Task<ApiResponse> DeleteType(long id)
    {
        await _fsql.Delete<DictType>(id).ExecuteAffrowsAsync();
        await _fsql.Delete<DictData>().Where(d => d.DictTypeId == id).ExecuteAffrowsAsync();
        return ApiResponse.Success("删除成功");
    }

    [HttpGet("data/{typeCode}")]
    public async Task<ApiResponse<List<DictData>>> GetDictData(string typeCode)
    {
        var dictType = await _fsql.Select<DictType>().Where(d => d.Code == typeCode).FirstAsync();
        if (dictType == null)
        {
            return ApiResponse<List<DictData>>.FailResult("字典类型不存在");
        }

        var list = await _fsql.Select<DictData>()
            .Where(d => d.DictTypeId == dictType.Id && d.Status == 1)
            .OrderBy(d => d.Sort)
            .ToListAsync();

        return ApiResponse<List<DictData>>.SuccessResult(list);
    }

    [HttpPost("data")]
    public async Task<ApiResponse<long>> CreateData([FromBody] DictData input)
    {
        input.CreateTime = DateTime.Now;
        var id = await _fsql.Insert(input).ExecuteIdentityAsync();
        return ApiResponse<long>.SuccessResult(id, "创建成功");
    }

    [HttpPut("data/{id}")]
    public async Task<ApiResponse> UpdateData(long id, [FromBody] DictData input)
    {
        await _fsql.Update<DictData>(id)
            .Set(d => d.Label, input.Label)
            .Set(d => d.Value, input.Value)
            .Set(d => d.Sort, input.Sort)
            .Set(d => d.Status, input.Status)
            .Set(d => d.Remark, input.Remark)
            .ExecuteAffrowsAsync();

        return ApiResponse.Success("更新成功");
    }

    [HttpDelete("data/{id}")]
    public async Task<ApiResponse> DeleteData(long id)
    {
        await _fsql.Delete<DictData>(id).ExecuteAffrowsAsync();
        return ApiResponse.Success("删除成功");
    }
}
