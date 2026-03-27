using FreeSql.DataAnnotations;

namespace CWHT.OA.Domain.Entities.System;

/// <summary>
/// 岗位表
/// </summary>
[Table(Name = "sys_position")]
public class Position
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public long Id { get; set; }

    /// <summary>
    /// 岗位名称
    /// </summary>
    [Column(StringLength = 50)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 岗位编码
    /// </summary>
    [Column(StringLength = 50)]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 状态 0-禁用 1-启用
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [Column(StringLength = 500)]
    public string? Remark { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime? UpdateTime { get; set; }

    [Navigate(nameof(User.PositionId))]
    public List<User>? Users { get; set; }
}
