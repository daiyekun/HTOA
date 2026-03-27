using FreeSql.DataAnnotations;

namespace CWHT.OA.Domain.Entities.System;

/// <summary>
/// 用户表
/// </summary>
[Table(Name = "sys_user")]
public class User
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public long Id { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    [Column(StringLength = 50)]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// 密码
    /// </summary>
    [Column(StringLength = 200)]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// 盐值
    /// </summary>
    [Column(StringLength = 50)]
    public string Salt { get; set; } = string.Empty;

    /// <summary>
    /// 真实姓名
    /// </summary>
    [Column(StringLength = 50)]
    public string RealName { get; set; } = string.Empty;

    /// <summary>
    /// 手机号
    /// </summary>
    [Column(StringLength = 20)]
    public string? Phone { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    [Column(StringLength = 100)]
    public string? Email { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    [Column(StringLength = 500)]
    public string? Avatar { get; set; }

    /// <summary>
    /// 性别 0-未知 1-男 2-女
    /// </summary>
    public int Gender { get; set; }

    /// <summary>
    /// 状态 0-禁用 1-启用
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// 部门ID
    /// </summary>
    public long? DepartmentId { get; set; }

    /// <summary>
    /// 岗位ID
    /// </summary>
    public long? PositionId { get; set; }

    /// <summary>
    /// 是否管理员
    /// </summary>
    public bool IsAdmin { get; set; }

    /// <summary>
    /// 最后登录时间
    /// </summary>
    public DateTime? LastLoginTime { get; set; }

    /// <summary>
    /// 最后登录IP
    /// </summary>
    [Column(StringLength = 50)]
    public string? LastLoginIp { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime? UpdateTime { get; set; }

    /// <summary>
    /// 创建人ID
    /// </summary>
    public long? CreateBy { get; set; }

    /// <summary>
    /// 更新人ID
    /// </summary>
    public long? UpdateBy { get; set; }

    /// <summary>
    /// 逻辑删除 0-未删除 1-已删除
    /// </summary>
    public int IsDeleted { get; set; }

    [Navigate(nameof(DepartmentId))]
    public Department? Department { get; set; }

    [Navigate(nameof(PositionId))]
    public Position? Position { get; set; }

    [Navigate(nameof(UserRole.UserId))]
    public List<UserRole>? UserRoles { get; set; }
}
