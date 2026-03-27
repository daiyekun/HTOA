using FreeSql.DataAnnotations;

namespace CWHT.OA.Domain.Entities.Approval;

/// <summary>
/// 出差申请表
/// </summary>
[Table(Name = "appr_business_trip")]
public class BusinessTrip
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public long Id { get; set; }

    /// <summary>
    /// 出差事由
    /// </summary>
    [Column(StringLength = 500)]
    public string Reason { get; set; } = string.Empty;

    /// <summary>
    /// 出发城市
    /// </summary>
    [Column(StringLength = 100)]
    public string FromCity { get; set; } = string.Empty;

    /// <summary>
    /// 目的城市
    /// </summary>
    [Column(StringLength = 100)]
    public string ToCity { get; set; } = string.Empty;

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// 出差天数
    /// </summary>
    public int Days { get; set; }

    /// <summary>
    /// 交通工具 1-飞机 2-火车 3-汽车 4-自驾
    /// </summary>
    public int TransportType { get; set; }

    /// <summary>
    /// 预计费用
    /// </summary>
    public decimal EstimatedCost { get; set; }

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
