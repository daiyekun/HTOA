using FreeSql.DataAnnotations;

namespace CWHT.OA.Domain.Entities.System;

/// <summary>
/// 字典数据表
/// </summary>
[Table(Name = "sys_dict_data")]
public class DictData
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public long Id { get; set; }

    /// <summary>
    /// 字典类型ID
    /// </summary>
    public long DictTypeId { get; set; }

    /// <summary>
    /// 字典标签
    /// </summary>
    [Column(StringLength = 100)]
    public string Label { get; set; } = string.Empty;

    /// <summary>
    /// 字典值
    /// </summary>
    [Column(StringLength = 100)]
    public string Value { get; set; } = string.Empty;

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

    [Navigate(nameof(DictTypeId))]
    public DictType? DictType { get; set; }
}
