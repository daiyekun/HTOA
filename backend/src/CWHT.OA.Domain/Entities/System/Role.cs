using FreeSql.DataAnnotations;

namespace CWHT.OA.Domain.Entities.System;

/// <summary>
/// 角色表
/// </summary>
[Table(Name = "sys_role")]
public class Role
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public long Id { get; set; }

    /// <summary>
    /// 角色名称
    /// </summary>
    [Column(StringLength = 50)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 角色编码
    /// </summary>
    [Column(StringLength = 50)]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 数据范围 1-全部 2-本部门及以下 3-本部门 4-仅本人 5-自定义
    /// </summary>
    public int DataScope { get; set; }

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

    [Navigate(nameof(UserRole.RoleId))]
    public List<UserRole>? UserRoles { get; set; }

    [Navigate(nameof(RoleMenu.RoleId))]
    public List<RoleMenu>? RoleMenus { get; set; }

    [Navigate(nameof(RolePermission.RoleId))]
    public List<RolePermission>? RolePermissions { get; set; }
}
