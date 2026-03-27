using FreeSql.DataAnnotations;

namespace CWHT.OA.Domain.Entities.Workflow;

/// <summary>
/// 工作流转接表
/// </summary>
[Table(Name = "wf_transition")]
public class WorkflowTransition
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public long Id { get; set; }

    /// <summary>
    /// 流程定义ID
    /// </summary>
    public long DefinitionId { get; set; }

    /// <summary>
    /// 来源节点ID
    /// </summary>
    public long FromNodeId { get; set; }

    /// <summary>
    /// 目标节点ID
    /// </summary>
    public long ToNodeId { get; set; }

    /// <summary>
    /// 条件表达式
    /// </summary>
    [Column(StringLength = 2000)]
    public string? ConditionExpression { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    [Navigate(nameof(FromNodeId))]
    public WorkflowNode? FromNode { get; set; }

    [Navigate(nameof(ToNodeId))]
    public WorkflowNode? ToNode { get; set; }
}
