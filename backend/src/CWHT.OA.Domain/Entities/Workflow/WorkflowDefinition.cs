using FreeSql.DataAnnotations;

namespace CWHT.OA.Domain.Entities.Workflow;

/// <summary>
/// 工作流定义表
/// </summary>
[Table(Name = "wf_definition")]
public class WorkflowDefinition
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public long Id { get; set; }

    /// <summary>
    /// 流程名称
    /// </summary>
    [Column(StringLength = 100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 流程编码
    /// </summary>
    [Column(StringLength = 100)]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// 版本号
    /// </summary>
    public int Version { get; set; }

    /// <summary>
    /// 表单配置JSON
    /// </summary>
    [Column(StringLength = -1)]
    public string? FormConfig { get; set; }

    /// <summary>
    /// 流程配置JSON
    /// </summary>
    [Column(StringLength = -1)]
    public string? FlowConfig { get; set; }

    /// <summary>
    /// 状态 0-禁用 1-启用 2-草稿
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [Column(StringLength = 500)]
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime? UpdateTime { get; set; }

    /// <summary>
    /// 创建人ID
    /// </summary>
    public long CreateBy { get; set; }

    [Navigate(nameof(WorkflowNode.DefinitionId))]
    public List<WorkflowNode>? Nodes { get; set; }

    [Navigate(nameof(WorkflowInstance.DefinitionId))]
    public List<WorkflowInstance>? Instances { get; set; }
}
