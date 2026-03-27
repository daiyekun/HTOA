using FreeSql.DataAnnotations;

namespace CWHT.OA.Domain.Entities.Knowledge;

/// <summary>
/// 知识分类
/// </summary>
[Table(Name = "kb_category")]
public class KnowledgeCategory
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public long Id { get; set; }

    [Column(StringLength = 100)]
    public string Name { get; set; } = string.Empty;

    public long? ParentId { get; set; }

    public int Sort { get; set; }

    public int Status { get; set; }

    public DateTime CreateTime { get; set; }

    [Navigate(nameof(ParentId))]
    public KnowledgeCategory? Parent { get; set; }
}
