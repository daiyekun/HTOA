using FreeSql.DataAnnotations;

namespace CWHT.OA.Domain.Entities.Knowledge;

/// <summary>
/// 知识文档
/// </summary>
[Table(Name = "kb_document")]
public class KnowledgeDocument
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public long Id { get; set; }

    [Column(StringLength = 200)]
    public string Title { get; set; } = string.Empty;

    [Column(StringLength = -1)]
    public string Content { get; set; } = string.Empty;

    public long CategoryId { get; set; }

    [Column(StringLength = 500)]
    public string? Attachment { get; set; }

    public int ViewCount { get; set; }

    public int DownloadCount { get; set; }

    public bool IsPublic { get; set; }

    public int Status { get; set; }

    public long CreateBy { get; set; }

    [Column(StringLength = 50)]
    public string CreateByName { get; set; } = string.Empty;

    public DateTime CreateTime { get; set; }

    public DateTime? UpdateTime { get; set; }

    [Navigate(nameof(CategoryId))]
    public KnowledgeCategory? Category { get; set; }
}
