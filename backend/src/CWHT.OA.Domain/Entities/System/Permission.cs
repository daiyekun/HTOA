using FreeSql.DataAnnotations;

namespace CWHT.OA.Domain.Entities.System;

/// <summary>
/// 权限表
/// </summary>
[Table(Name = "sys_permission")]
public class Permission
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public long Id { get; set; }

    /// <summary>
    /// 权限名称
    /// </summary>
    [Column(StringLength = 50)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 权限编码
    /// </summary>
    [Column(StringLength = 100)]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// 父权限ID
    /// </summary>
    public long? ParentId { get; set; }

    /// <summary>
    /// 类型 1-模块 2-功能 3-操作
    /// </summary>
    public int Type { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 状态 0-禁用 1-启用
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    [Navigate(nameof(RolePermission.PermissionId))]
    public List<RolePermission>? RolePermissions { get; set; }
}
