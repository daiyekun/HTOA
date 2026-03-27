using FreeSql.DataAnnotations;

namespace CWHT.OA.Domain.Entities.System;

/// <summary>
/// 字典类型表
/// </summary>
[Table(Name = "sys_dict_type")]
public class DictType
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public long Id { get; set; }

    /// <summary>
    /// 字典类型名称
    /// </summary>
    [Column(StringLength = 100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 字典类型编码
    /// </summary>
    [Column(StringLength = 100)]
    public string Code { get; set; } = string.Empty;

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

    [Navigate(nameof(DictData.DictTypeId))]
    public List<DictData>? DictDatas { get; set; }
}
