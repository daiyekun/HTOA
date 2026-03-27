using FreeSql.DataAnnotations;

namespace CWHT.OA.Domain.Entities.Workflow;

/// <summary>
/// 工作流节点表
/// </summary>
[Table(Name = "wf_node")]
public class WorkflowNode
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public long Id { get; set; }

    /// <summary>
    /// 流程定义ID
    /// </summary>
    public long DefinitionId { get; set; }

    /// <summary>
    /// 节点名称
    /// </summary>
    [Column(StringLength = 100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 节点编码
    /// </summary>
    [Column(StringLength = 100)]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// 节点类型 1-开始 2-结束 3-审批 4-条件分支 5-并行 6-抄送
    /// </summary>
    public int Type { get; set; }

    /// <summary>
    /// 审批人类型 1-指定人 2-角色 3-部门负责人 4-发起人自选 5-发起人自己
    /// </summary>
    public int AssigneeType { get; set; }

    /// <summary>
    /// 审批人配置JSON
    /// </summary>
    [Column(StringLength = 2000)]
    public string? AssigneeConfig { get; set; }

    /// <summary>
    /// 审批方式 1-或签 2-会签 3-依次审批
    /// </summary>
    public int ApproveType { get; set; }

    /// <summary>
    /// 超时时间(小时)
    /// </summary>
    public int TimeoutHours { get; set; }

    /// <summary>
    /// 超时处理 1-自动通过 2-自动拒绝 3-通知
    /// </summary>
    public int TimeoutAction { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 位置X
    /// </summary>
    public int PositionX { get; set; }

    /// <summary>
    /// 位置Y
    /// </summary>
    public int PositionY { get; set; }

    [Navigate(nameof(DefinitionId))]
    public WorkflowDefinition? Definition { get; set; }

    [Navigate(nameof(WorkflowTransition.FromNodeId))]
    public List<WorkflowTransition>? FromTransitions { get; set; }

    [Navigate(nameof(WorkflowTransition.ToNodeId))]
    public List<WorkflowTransition>? ToTransitions { get; set; }
}
