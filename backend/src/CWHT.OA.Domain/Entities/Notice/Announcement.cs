using FreeSql.DataAnnotations;

namespace CWHT.OA.Domain.Entities.Notice;

/// <summary>
/// 公告表
/// </summary>
[Table(Name = "notice_announcement")]
public class Announcement
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public long Id { get; set; }

    /// <summary>
    /// 公告标题
    /// </summary>
    [Column(StringLength = 200)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 公告内容
    /// </summary>
    [Column(StringLength = -1)]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 公告类型 1-通知 2-公告 3-活动
    /// </summary>
    public int Type { get; set; }

    /// <summary>
    /// 优先级 0-普通 1-重要 2-紧急
    /// </summary>
    public int Priority { get; set; }

    /// <summary>
    /// 是否置顶
    /// </summary>
    public bool IsTop { get; set; }

    /// <summary>
    /// 状态 0-草稿 1-已发布 2-已撤回
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// 发布时间
    /// </summary>
    public DateTime? PublishTime { get; set; }

    /// <summary>
    /// 过期时间
    /// </summary>
    public DateTime? ExpireTime { get; set; }

    /// <summary>
    /// 发布人ID
    /// </summary>
    public long CreateBy { get; set; }

    /// <summary>
    /// 发布人姓名
    /// </summary>
    [Column(StringLength = 50)]
    public string CreateByName { get; set; } = string.Empty;

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime? UpdateTime { get; set; }
}
