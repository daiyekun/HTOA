using FreeSql.DataAnnotations;

namespace CWHT.OA.Domain.Entities.System;

/// <summary>
/// 角色菜单关联表
/// </summary>
[Table(Name = "sys_role_menu")]
public class RoleMenu
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public long Id { get; set; }

    public long RoleId { get; set; }

    public long MenuId { get; set; }

    [Navigate(nameof(RoleId))]
    public Role? Role { get; set; }

    [Navigate(nameof(MenuId))]
    public Menu? Menu { get; set; }
}
