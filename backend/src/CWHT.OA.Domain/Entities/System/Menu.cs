using FreeSql.DataAnnotations;

namespace CWHT.OA.Domain.Entities.System;

/// <summary>
/// 菜单表
/// </summary>
[Table(Name = "sys_menu")]
public class Menu
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public long Id { get; set; }

    /// <summary>
    /// 菜单名称
    /// </summary>
    [Column(StringLength = 50)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 父菜单ID
    /// </summary>
    public long? ParentId { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 路由地址
    /// </summary>
    [Column(StringLength = 200)]
    public string? Path { get; set; }

    /// <summary>
    /// 组件路径
    /// </summary>
    [Column(StringLength = 200)]
    public string? Component { get; set; }

    /// <summary>
    /// 菜单类型 1-目录 2-菜单 3-按钮
    /// </summary>
    public int Type { get; set; }

    /// <summary>
    /// 权限标识
    /// </summary>
    [Column(StringLength = 100)]
    public string? Permission { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    [Column(StringLength = 100)]
    public string? Icon { get; set; }

    /// <summary>
    /// 是否可见 0-隐藏 1-显示
    /// </summary>
    public int IsVisible { get; set; }

    /// <summary>
    /// 状态 0-禁用 1-启用
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// 是否外链 0-否 1-是
    /// </summary>
    public int IsExternal { get; set; }

    /// <summary>
    /// 是否缓存 0-否 1-是
    /// </summary>
    public int IsCache { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime? UpdateTime { get; set; }

    [Navigate(nameof(ParentId))]
    public Menu? Parent { get; set; }

    [Navigate(nameof(RoleMenu.MenuId))]
    public List<RoleMenu>? RoleMenus { get; set; }
}
