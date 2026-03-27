using FreeSql.DataAnnotations;

namespace CWHT.OA.Domain.Entities.System;

/// <summary>
/// 角色权限关联表
/// </summary>
[Table(Name = "sys_role_permission")]
public class RolePermission
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public long Id { get; set; }

    public long RoleId { get; set; }

    public long PermissionId { get; set; }

    [Navigate(nameof(RoleId))]
    public Role? Role { get; set; }

    [Navigate(nameof(PermissionId))]
    public Permission? Permission { get; set; }
}
