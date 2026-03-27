using FreeSql.DataAnnotations;

namespace CWHT.OA.Domain.Entities.Vehicle;

/// <summary>
/// 车辆信息
/// </summary>
[Table(Name = "vehicle_car")]
public class VehicleCar
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public long Id { get; set; }

    [Column(StringLength = 50)]
    public string PlateNumber { get; set; } = string.Empty;

    [Column(StringLength = 50)]
    public string Brand { get; set; } = string.Empty;

    [Column(StringLength = 50)]
    public string Model { get; set; } = string.Empty;

    [Column(StringLength = 20)]
    public string? Color { get; set; }

    public int SeatCount { get; set; }

    [Column(StringLength = 20)]
    public string? FuelType { get; set; }

    public decimal? Mileage { get; set; }

    public DateTime? PurchaseDate { get; set; }

    [Column(StringLength = 50)]
    public string? InsuranceExpireDate { get; set; }

    public int Status { get; set; }

    [Column(StringLength = 500)]
    public string? Remark { get; set; }

    public DateTime CreateTime { get; set; }
}
