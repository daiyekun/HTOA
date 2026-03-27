using FreeSql.DataAnnotations;

namespace CWHT.OA.Domain.Entities.Schedule;

/// <summary>
/// 日历表
/// </summary>
[Table(Name = "sched_calendar")]
public class Calendar
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public long Id { get; set; }

    /// <summary>
    /// 日历名称
    /// </summary>
    [Column(StringLength = 100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 颜色
    /// </summary>
    [Column(StringLength = 20)]
    public string? Color { get; set; }

    /// <summary>
    /// 所属用户ID
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// 是否默认
    /// </summary>
    public bool IsDefault { get; set; }

    /// <summary>
    /// 是否共享
    /// </summary>
    public bool IsShared { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    [Navigate(nameof(Event.CalendarId))]
    public List<Event>? Events { get; set; }
}
