using FreeSql.DataAnnotations;

namespace CWHT.OA.Domain.Entities.Meeting;

/// <summary>
/// 会议室
/// </summary>
[Table(Name = "meeting_room")]
public class MeetingRoom
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public long Id { get; set; }

    [Column(StringLength = 100)]
    public string Name { get; set; } = string.Empty;

    [Column(StringLength = 200)]
    public string? Location { get; set; }

    public int Capacity { get; set; }

    [Column(StringLength = 500)]
    public string? Equipment { get; set; }

    [Column(StringLength = 500)]
    public string? Remark { get; set; }

    public int Status { get; set; }

    public DateTime CreateTime { get; set; }
}
