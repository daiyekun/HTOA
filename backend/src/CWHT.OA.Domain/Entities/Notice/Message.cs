using FreeSql.DataAnnotations;

namespace CWHT.OA.Domain.Entities.Notice;

/// <summary>
/// 消息表
/// </summary>
[Table(Name = "notice_message")]
public class Message
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public long Id { get; set; }

    /// <summary>
    /// 消息类型 1-系统消息 2-审批消息 3-通知消息
    /// </summary>
    public int Type { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    [Column(StringLength = 200)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 内容
    /// </summary>
    [Column(StringLength = -1)]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 发送人ID
    /// </summary>
    public long? SenderId { get; set; }

    /// <summary>
    /// 发送人姓名
    /// </summary>
    [Column(StringLength = 50)]
    public string? SenderName { get; set; }

    /// <summary>
    /// 接收人ID
    /// </summary>
    public long ReceiverId { get; set; }

    /// <summary>
    /// 是否已读
    /// </summary>
    public bool IsRead { get; set; }

    /// <summary>
    /// 读取时间
    /// </summary>
    public DateTime? ReadTime { get; set; }

    /// <summary>
    /// 业务类型
    /// </summary>
    [Column(StringLength = 50)]
    public string? BusinessType { get; set; }

    /// <summary>
    /// 业务ID
    /// </summary>
    public long? BusinessId { get; set; }

    /// <summary>
    /// 跳转链接
    /// </summary>
    [Column(StringLength = 500)]
    public string? Url { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }
}
