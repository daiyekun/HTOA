using FreeSql.DataAnnotations;

namespace CWHT.OA.Domain.Entities.Approval;

/// <summary>
/// 费用申请表
/// </summary>
[Table(Name = "appr_expense")]
public class Expense
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public long Id { get; set; }

    /// <summary>
    /// 费用类型 1-办公用品 2-差旅费 3-招待费 4-培训费 5-其他
    /// </summary>
    public int Type { get; set; }

    /// <summary>
    /// 费用金额
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// 费用说明
    /// </summary>
    [Column(StringLength = 500)]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// 附件
    /// </summary>
    [Column(StringLength = 2000)]
    public string? Attachments { get; set; }

    /// <summary>
    /// 申请人ID
    /// </summary>
    public long ApplyUserId { get; set; }

    /// <summary>
    /// 申请人姓名
    /// </summary>
    [Column(StringLength = 50)]
    public string ApplyUserName { get; set; } = string.Empty;

    /// <summary>
    /// 申请部门ID
    /// </summary>
    public long ApplyDeptId { get; set; }

    /// <summary>
    /// 状态 0-草稿 1-审批中 2-已通过 3-已驳回 4-已撤销
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// 流程实例ID
    /// </summary>
    public long? InstanceId { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime? UpdateTime { get; set; }
}
