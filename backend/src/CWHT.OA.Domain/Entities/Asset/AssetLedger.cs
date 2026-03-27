using FreeSql.DataAnnotations;

namespace CWHT.OA.Domain.Entities.Asset;

/// <summary>
/// 资产台账
/// </summary>
[Table(Name = "asset_ledger")]
public class AssetLedger
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public long Id { get; set; }

    [Column(StringLength = 100)]
    public string Name { get; set; } = string.Empty;

    [Column(StringLength = 100)]
    public string Code { get; set; } = string.Empty;

    public long CategoryId { get; set; }

    [Column(StringLength = 50)]
    public string? Spec { get; set; }

    [Column(StringLength = 50)]
    public string? Brand { get; set; }

    public decimal PurchasePrice { get; set; }

    public DateTime? PurchaseDate { get; set; }

    public int Status { get; set; }

    public long? KeeperId { get; set; }

    [Column(StringLength = 50)]
    public string? KeeperName { get; set; }

    public long? DepartmentId { get; set; }

    [Column(StringLength = 500)]
    public string? Remark { get; set; }

    public DateTime CreateTime { get; set; }
}
