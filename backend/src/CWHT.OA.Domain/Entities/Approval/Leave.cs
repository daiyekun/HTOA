using FreeSql.DataAnnotations;

namespace CWHT.OA.Domain.Entities.Approval;

/// <summary>
/// 请假申请表
/// </summary>
[Table(Name = "appr_leave")]
public class Leave
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public long Id { get; set; }

    /// <summary>
    /// 请假类型 1-事假 2-病假 3-年假 4-调休 5-婚假 6-产假 7-陪产假 8-丧假 9-其他
    /// </summary>
    public int Type { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// 请假天数
    /// </summary>
    public decimal Days { get; set; }

    /// <summary>
    /// 请假原因
    /// </summary>
    [Column(StringLength = 500)]
    public string Reason { get; set; } = string.Empty;

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
