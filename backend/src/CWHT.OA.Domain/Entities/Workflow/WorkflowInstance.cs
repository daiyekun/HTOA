using FreeSql.DataAnnotations;

namespace CWHT.OA.Domain.Entities.Workflow;

/// <summary>
/// 工作流实例表
/// </summary>
[Table(Name = "wf_instance")]
public class WorkflowInstance
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public long Id { get; set; }

    /// <summary>
    /// 流程定义ID
    /// </summary>
    public long DefinitionId { get; set; }

    /// <summary>
    /// 业务类型
    /// </summary>
    [Column(StringLength = 50)]
    public string BusinessType { get; set; } = string.Empty;

    /// <summary>
    /// 业务ID
    /// </summary>
    public long BusinessId { get; set; }

    /// <summary>
    /// 流程标题
    /// </summary>
    [Column(StringLength = 200)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 发起人ID
    /// </summary>
    public long StartUserId { get; set; }

    /// <summary>
    /// 发起人姓名
    /// </summary>
    [Column(StringLength = 50)]
    public string StartUserName { get; set; } = string.Empty;

    /// <summary>
    /// 当前节点ID
    /// </summary>
    public long? CurrentNodeId { get; set; }

    /// <summary>
    /// 当前节点名称
    /// </summary>
    [Column(StringLength = 100)]
    public string? CurrentNodeName { get; set; }

    /// <summary>
    /// 状态 0-进行中 1-已完成 2-已驳回 3-已撤销 4-已终止
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// 表单数据JSON
    /// </summary>
    [Column(StringLength = -1)]
    public string? FormData { get; set; }

    /// <summary>
    /// 发起时间
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? EndTime { get; set; }

    [Navigate(nameof(DefinitionId))]
    public WorkflowDefinition? Definition { get; set; }

    [Navigate(nameof(WorkflowTask.InstanceId))]
    public List<WorkflowTask>? Tasks { get; set; }
}
