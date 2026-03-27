using FreeSql.DataAnnotations;

namespace CWHT.OA.Domain.Entities.Asset;

/// <summary>
/// 资产分类
/// </summary>
[Table(Name = "asset_category")]
public class AssetCategory
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public long Id { get; set; }

    [Column(StringLength = 100)]
    public string Name { get; set; } = string.Empty;

    [Column(StringLength = 50)]
    public string Code { get; set; } = string.Empty;

    public long? ParentId { get; set; }

    public int Sort { get; set; }

    public int DepreciationYears { get; set; }

    public DateTime CreateTime { get; set; }
}
