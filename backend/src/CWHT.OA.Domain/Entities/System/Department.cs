using FreeSql.DataAnnotations;

namespace CWHT.OA.Domain.Entities.System;

/// <summary>
/// 部门表
/// </summary>
[Table(Name = "sys_department")]
public class Department
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public long Id { get; set; }

    /// <summary>
    /// 部门名称
    /// </summary>
    [Column(StringLength = 50)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 父部门ID
    /// </summary>
    public long? ParentId { get; set; }

    /// <summary>
    /// 祖级列表
    /// </summary>
    [Column(StringLength = 500)]
    public string? Ancestors { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 负责人
    /// </summary>
    [Column(StringLength = 50)]
    public string? Leader { get; set; }

    /// <summary>
    /// 联系电话
    /// </summary>
    [Column(StringLength = 20)]
    public string? Phone { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    [Column(StringLength = 100)]
    public string? Email { get; set; }

    /// <summary>
    /// 状态 0-禁用 1-启用
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime? UpdateTime { get; set; }

    [Navigate(nameof(ParentId))]
    public Department? Parent { get; set; }

    [Navigate(nameof(User.DepartmentId))]
    public List<User>? Users { get; set; }
}
