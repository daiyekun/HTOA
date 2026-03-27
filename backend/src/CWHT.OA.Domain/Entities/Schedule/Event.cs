using FreeSql.DataAnnotations;

namespace CWHT.OA.Domain.Entities.Schedule;

/// <summary>
/// 日程事件表
/// </summary>
[Table(Name = "sched_event")]
public class Event
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public long Id { get; set; }

    /// <summary>
    /// 日历ID
    /// </summary>
    public long CalendarId { get; set; }

    /// <summary>
    /// 事件标题
    /// </summary>
    [Column(StringLength = 200)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 事件内容
    /// </summary>
    [Column(StringLength = -1)]
    public string? Content { get; set; }

    /// <summary>
    /// 事件类型 1-会议 2-任务 3-提醒 4-其他
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
    /// 是否全天
    /// </summary>
    public bool IsAllDay { get; set; }

    /// <summary>
    /// 地点
    /// </summary>
    [Column(StringLength = 200)]
    public string? Location { get; set; }

    /// <summary>
    /// 提醒时间(分钟)
    /// </summary>
    public int ReminderMinutes { get; set; }

    /// <summary>
    /// 重复类型 0-不重复 1-每天 2-每周 3-每月 4-每年
    /// </summary>
    public int RepeatType { get; set; }

    /// <summary>
    /// 参与人ID列表
    /// </summary>
    [Column(StringLength = 2000)]
    public string? Participants { get; set; }

    /// <summary>
    /// 颜色
    /// </summary>
    [Column(StringLength = 20)]
    public string? Color { get; set; }

    /// <summary>
    /// 创建人ID
    /// </summary>
    public long CreateBy { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime? UpdateTime { get; set; }

    [Navigate(nameof(CalendarId))]
    public Calendar? Calendar { get; set; }
}
