using FreeSql.DataAnnotations;

namespace CWHT.OA.Domain.Entities.Attendance;

/// <summary>
/// 考勤记录表
/// </summary>
[Table(Name = "att_record")]
public class AttendanceRecord
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public long Id { get; set; }

    /// <summary>
    /// 用户ID
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// 用户姓名
    /// </summary>
    [Column(StringLength = 50)]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 部门ID
    /// </summary>
    public long DepartmentId { get; set; }

    /// <summary>
    /// 部门名称
    /// </summary>
    [Column(StringLength = 50)]
    public string DepartmentName { get; set; } = string.Empty;

    /// <summary>
    /// 考勤日期
    /// </summary>
    public DateTime RecordDate { get; set; }

    /// <summary>
    /// 上班打卡时间
    /// </summary>
    public DateTime? CheckInTime { get; set; }

    /// <summary>
    /// 下班打卡时间
    /// </summary>
    public DateTime? CheckOutTime { get; set; }

    /// <summary>
    /// 上班打卡状态 0-正常 1-迟到 2-缺卡
    /// </summary>
    public int CheckInStatus { get; set; }

    /// <summary>
    /// 下班打卡状态 0-正常 1-早退 2-缺卡
    /// </summary>
    public int CheckOutStatus { get; set; }

    /// <summary>
    /// 工作时长(小时)
    /// </summary>
    public decimal WorkHours { get; set; }

    /// <summary>
    /// 加班时长(小时)
    /// </summary>
    public decimal OvertimeHours { get; set; }

    /// <summary>
    /// 打卡地址
    /// </summary>
    [Column(StringLength = 500)]
    public string? Location { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [Column(StringLength = 500)]
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
}
