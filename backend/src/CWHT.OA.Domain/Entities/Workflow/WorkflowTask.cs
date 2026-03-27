using FreeSql.DataAnnotations;

namespace CWHT.OA.Domain.Entities.Workflow;

/// <summary>
/// 工作流任务表
/// </summary>
[Table(Name = "wf_task")]
public class WorkflowTask
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public long Id { get; set; }

    /// <summary>
    /// 流程实例ID
    /// </summary>
    public long InstanceId { get; set; }

    /// <summary>
    /// 节点ID
    /// </summary>
    public long NodeId { get; set; }

    /// <summary>
    /// 节点名称
    /// </summary>
    [Column(StringLength = 100)]
    public string NodeName { get; set; } = string.Empty;

    /// <summary>
    /// 审批人ID
    /// </summary>
    public long AssigneeId { get; set; }

    /// <summary>
    /// 审批人姓名
    /// </summary>
    [Column(StringLength = 50)]
    public string AssigneeName { get; set; } = string.Empty;

    /// <summary>
    /// 状态 0-待处理 1-已同意 2-已拒绝 3-已转办 4-已退回 5-已撤回
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// 审批意见
    /// </summary>
    [Column(StringLength = 500)]
    public string? Opinion { get; set; }

    /// <summary>
    /// 处理时间
    /// </summary>
    public DateTime? HandleTime { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 超时时间
    /// </summary>
    public DateTime? TimeoutTime { get; set; }

    [Navigate(nameof(InstanceId))]
    public WorkflowInstance? Instance { get; set; }
}
