using FreeSql.DataAnnotations;

namespace CWHT.OA.Domain.Entities.System;

/// <summary>
/// 用户角色关联表
/// </summary>
[Table(Name = "sys_user_role")]
public class UserRole
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public long Id { get; set; }

    public long UserId { get; set; }

    public long RoleId { get; set; }

    [Navigate(nameof(UserId))]
    public User? User { get; set; }

    [Navigate(nameof(RoleId))]
    public Role? Role { get; set; }
}
