using FreeSql.DataAnnotations;

namespace CWHT.OA.Domain.Entities.Contract;

/// <summary>
/// 合同信息
/// </summary>
[Table(Name = "contract_info")]
public class ContractInfo
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public long Id { get; set; }

    [Column(StringLength = 100)]
    public string ContractNo { get; set; } = string.Empty;

    [Column(StringLength = 200)]
    public string Title { get; set; } = string.Empty;

    public long TemplateId { get; set; }

    [Column(StringLength = 100)]
    public string PartyA { get; set; } = string.Empty;

    [Column(StringLength = 100)]
    public string PartyB { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public DateTime SignDate { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    [Column(StringLength = -1)]
    public string? Content { get; set; }

    [Column(StringLength = 500)]
    public string? Attachment { get; set; }

    public int Status { get; set; }

    public long CreateBy { get; set; }

    public DateTime CreateTime { get; set; }
}
