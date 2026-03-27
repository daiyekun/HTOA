using CWHT.OA.Application.DTOs;
using CWHT.OA.Domain.Entities.Workflow;
using FreeSql;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CWHT.OA.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WorkflowController : ControllerBase
{
    private readonly IFreeSql _fsql;

    public WorkflowController(IFreeSql fsql)
    {
        _fsql = fsql;
    }

    #region 流程定义

    [HttpGet("definitions")]
    public async Task<ApiResponse<PagedResult<WorkflowDefinition>>> GetDefinitions([FromQuery] PagedInput input)
    {
        var query = _fsql.Select<WorkflowDefinition>();

        if (!string.IsNullOrEmpty(input.Keyword))
        {
            query = query.Where(w => w.Name.Contains(input.Keyword) || w.Code.Contains(input.Keyword));
        }

        var total = await query.CountAsync();
        var list = await query
            .OrderByDescending(w => w.CreateTime)
            .Page(input.Page, input.PageSize)
            .ToListAsync();

        return ApiResponse<PagedResult<WorkflowDefinition>>.SuccessResult(new PagedResult<WorkflowDefinition>
        {
            Items = list,
            Total = total,
            Page = input.Page,
            PageSize = input.PageSize
        });
    }

    [HttpGet("definitions/{id}")]
    public async Task<ApiResponse<WorkflowDefinition>> GetDefinitionById(long id)
    {
        var definition = await _fsql.Select<WorkflowDefinition>()
            .Where(w => w.Id == id)
            .Include(w => w.Nodes)
            .FirstAsync();

        if (definition == null)
        {
            return ApiResponse<WorkflowDefinition>.FailResult("流程定义不存在");
        }

        // 加载流转关系
        if (definition.Nodes?.Count > 0)
        {
            var nodeIds = definition.Nodes.Select(n => n.Id).ToList();
            var transitions = await _fsql.Select<WorkflowTransition>()
                .Where(t => nodeIds.Contains(t.FromNodeId) || nodeIds.Contains(t.ToNodeId))
                .ToListAsync();

            foreach (var node in definition.Nodes)
            {
                node.FromTransitions = transitions.Where(t => t.FromNodeId == node.Id).ToList();
                node.ToTransitions = transitions.Where(t => t.ToNodeId == node.Id).ToList();
            }
        }

        return ApiResponse<WorkflowDefinition>.SuccessResult(definition);
    }

    [HttpPost("definitions")]
    public async Task<ApiResponse<long>> CreateDefinition([FromBody] WorkflowDefinition input)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        input.CreateBy = long.Parse(userId ?? "0");
        input.CreateTime = DateTime.Now;

        var id = await _fsql.Insert(input).ExecuteIdentityAsync();

        // 保存节点
        if (input.Nodes?.Count > 0)
        {
            foreach (var node in input.Nodes)
            {
                node.DefinitionId = id;
            }
            await _fsql.Insert(input.Nodes).ExecuteAffrowsAsync();
        }

        return ApiResponse<long>.SuccessResult(id, "创建成功");
    }

    [HttpPut("definitions/{id}")]
    public async Task<ApiResponse> UpdateDefinition(long id, [FromBody] WorkflowDefinition input)
    {
        await _fsql.Update<WorkflowDefinition>(id)
            .Set(w => w.Name, input.Name)
            .Set(w => w.FormConfig, input.FormConfig)
            .Set(w => w.FlowConfig, input.FlowConfig)
            .Set(w => w.Status, input.Status)
            .Set(w => w.Remark, input.Remark)
            .Set(w => w.UpdateTime, DateTime.Now)
            .ExecuteAffrowsAsync();

        return ApiResponse.Success("更新成功");
    }

    [HttpDelete("definitions/{id}")]
    public async Task<ApiResponse> DeleteDefinition(long id)
    {
        var hasInstance = await _fsql.Select<WorkflowInstance>()
            .Where(i => i.DefinitionId == id && i.Status == 0)
            .AnyAsync();

        if (hasInstance)
        {
            return ApiResponse.Fail("该流程存在进行中的实例，不能删除");
        }

        await _fsql.Delete<WorkflowDefinition>(id).ExecuteAffrowsAsync();
        await _fsql.Delete<WorkflowNode>().Where(n => n.DefinitionId == id).ExecuteAffrowsAsync();

        return ApiResponse.Success("删除成功");
    }

    #endregion

    #region 流程实例

    [HttpGet("instances")]
    public async Task<ApiResponse<PagedResult<WorkflowInstance>>> GetInstances([FromQuery] PagedInput input)
    {
        var query = _fsql.Select<WorkflowInstance>();

        if (!string.IsNullOrEmpty(input.Keyword))
        {
            query = query.Where(i => i.Title.Contains(input.Keyword));
        }

        var total = await query.CountAsync();
        var list = await query
            .Include(i => i.Definition)
            .OrderByDescending(i => i.StartTime)
            .Page(input.Page, input.PageSize)
            .ToListAsync();

        return ApiResponse<PagedResult<WorkflowInstance>>.SuccessResult(new PagedResult<WorkflowInstance>
        {
            Items = list,
            Total = total,
            Page = input.Page,
            PageSize = input.PageSize
        });
    }

    [HttpGet("instances/{id}")]
    public async Task<ApiResponse<WorkflowInstance>> GetInstanceById(long id)
    {
        var instance = await _fsql.Select<WorkflowInstance>()
            .Where(i => i.Id == id)
            .Include(i => i.Definition)
            .Include(i => i.Tasks)
            .FirstAsync();

        if (instance == null)
        {
            return ApiResponse<WorkflowInstance>.FailResult("流程实例不存在");
        }

        return ApiResponse<WorkflowInstance>.SuccessResult(instance);
    }

    #endregion

    #region 待办任务

    [HttpGet("tasks/todo")]
    public async Task<ApiResponse<PagedResult<WorkflowTask>>> GetTodoTasks([FromQuery] PagedInput input)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return ApiResponse<PagedResult<WorkflowTask>>.FailResult("未登录");
        }

        var query = _fsql.Select<WorkflowTask>()
            .Where(t => t.AssigneeId == long.Parse(userId) && t.Status == 0);

        var total = await query.CountAsync();
        var list = await query
            .Include(t => t.Instance)
            .OrderByDescending(t => t.CreateTime)
            .Page(input.Page, input.PageSize)
            .ToListAsync();

        return ApiResponse<PagedResult<WorkflowTask>>.SuccessResult(new PagedResult<WorkflowTask>
        {
            Items = list,
            Total = total,
            Page = input.Page,
            PageSize = input.PageSize
        });
    }

    [HttpGet("tasks/done")]
    public async Task<ApiResponse<PagedResult<WorkflowTask>>> GetDoneTasks([FromQuery] PagedInput input)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return ApiResponse<PagedResult<WorkflowTask>>.FailResult("未登录");
        }

        var query = _fsql.Select<WorkflowTask>()
            .Where(t => t.AssigneeId == long.Parse(userId) && t.Status > 0);

        var total = await query.CountAsync();
        var list = await query
            .Include(t => t.Instance)
            .OrderByDescending(t => t.HandleTime)
            .Page(input.Page, input.PageSize)
            .ToListAsync();

        return ApiResponse<PagedResult<WorkflowTask>>.SuccessResult(new PagedResult<WorkflowTask>
        {
            Items = list,
            Total = total,
            Page = input.Page,
            PageSize = input.PageSize
        });
    }

    [HttpPost("tasks/{id}/approve")]
    public async Task<ApiResponse> ApproveTask(long id, [FromBody] ApproveTaskInput input)
    {
        var task = await _fsql.Select<WorkflowTask>().Where(t => t.Id == id).FirstAsync();
        if (task == null)
        {
            return ApiResponse.Fail("任务不存在");
        }

        if (task.Status != 0)
        {
            return ApiResponse.Fail("任务已处理");
        }

        await _fsql.Update<WorkflowTask>(id)
            .Set(t => t.Status, input.Approved ? 1 : 2)
            .Set(t => t.Opinion, input.Opinion)
            .Set(t => t.HandleTime, DateTime.Now)
            .ExecuteAffrowsAsync();

        // TODO: 流程流转逻辑

        return ApiResponse.Success(input.Approved ? "审批通过" : "审批驳回");
    }

    [HttpPost("tasks/{id}/reject")]
    public async Task<ApiResponse> RejectTask(long id, [FromBody] ApproveTaskInput input)
    {
        var task = await _fsql.Select<WorkflowTask>().Where(t => t.Id == id).FirstAsync();
        if (task == null)
        {
            return ApiResponse.Fail("任务不存在");
        }

        await _fsql.Update<WorkflowTask>(id)
            .Set(t => t.Status, 4)
            .Set(t => t.Opinion, input.Opinion)
            .Set(t => t.HandleTime, DateTime.Now)
            .ExecuteAffrowsAsync();

        // TODO: 退回逻辑

        return ApiResponse.Success("已退回");
    }

    #endregion
}

public class ApproveTaskInput
{
    public bool Approved { get; set; }
    public string? Opinion { get; set; }
}
