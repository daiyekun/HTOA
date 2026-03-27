using FreeSql.DataAnnotations;

namespace CWHT.OA.Domain.Entities.Meeting;

/// <summary>
/// 会议预约
/// </summary>
[Table(Name = "meeting_booking")]
public class MeetingBooking
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public long Id { get; set; }

    [Column(StringLength = 200)]
    public string Title { get; set; } = string.Empty;

    [Column(StringLength = -1)]
    public string? Content { get; set; }

    public long RoomId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    [Column(StringLength = 2000)]
    public string Participants { get; set; } = string.Empty;

    public long CreateUserId { get; set; }

    [Column(StringLength = 50)]
    public string CreateUserName { get; set; } = string.Empty;

    public int Status { get; set; }

    public DateTime CreateTime { get; set; }

    [Navigate(nameof(RoomId))]
    public MeetingRoom? Room { get; set; }
}
