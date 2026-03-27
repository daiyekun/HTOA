using FreeSql.DataAnnotations;

namespace CWHT.OA.Domain.Entities.Vehicle;

/// <summary>
/// 用车申请
/// </summary>
[Table(Name = "vehicle_apply")]
public class VehicleApply
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public long Id { get; set; }

    [Column(StringLength = 200)]
    public string Title { get; set; } = string.Empty;

    public long? CarId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    [Column(StringLength = 200)]
    public string Destination { get; set; } = string.Empty;

    [Column(StringLength = 500)]
    public string Reason { get; set; } = string.Empty;

    public int PassengerCount { get; set; }

    public long ApplyUserId { get; set; }

    [Column(StringLength = 50)]
    public string ApplyUserName { get; set; } = string.Empty;

    public long? DepartmentId { get; set; }

    public int Status { get; set; }

    public DateTime CreateTime { get; set; }
}
