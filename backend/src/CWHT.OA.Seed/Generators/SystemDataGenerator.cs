using CWHT.OA.Domain.Entities.System;
using FreeSql;

namespace CWHT.OA.Seed.Generators;

public static class SystemDataGenerator
{
    public static async Task SeedAsync(IFreeSql fsql)
    {
        // 检查是否已存在数据
        var userCount = await fsql.Select<User>().CountAsync();
        if (userCount > 0)
        {
            Console.WriteLine("System data already exists, skipping...");
            return;
        }

        // 创建部门
        var departments = new List<Department>
        {
            new() { Name = "总公司", ParentId = null, Sort = 1, Status = 1, Leader = "张总", Phone = "13800000001", CreateTime = DateTime.Now },
            new() { Name = "技术部", ParentId = 1, Sort = 1, Status = 1, Leader = "李经理", Phone = "13800000002", CreateTime = DateTime.Now },
            new() { Name = "产品部", ParentId = 1, Sort = 2, Status = 1, Leader = "王经理", Phone = "13800000003", CreateTime = DateTime.Now },
            new() { Name = "人事部", ParentId = 1, Sort = 3, Status = 1, Leader = "赵经理", Phone = "13800000004", CreateTime = DateTime.Now },
            new() { Name = "财务部", ParentId = 1, Sort = 4, Status = 1, Leader = "钱经理", Phone = "13800000005", CreateTime = DateTime.Now },
            new() { Name = "市场部", ParentId = 1, Sort = 5, Status = 1, Leader = "孙经理", Phone = "13800000006", CreateTime = DateTime.Now },
            new() { Name = "研发一组", ParentId = 2, Sort = 1, Status = 1, Leader = "周组长", Phone = "13800000007", CreateTime = DateTime.Now },
            new() { Name = "研发二组", ParentId = 2, Sort = 2, Status = 1, Leader = "吴组长", Phone = "13800000008", CreateTime = DateTime.Now }
        };
        await fsql.Insert(departments).ExecuteAffrowsAsync();
        Console.WriteLine($"Created {departments.Count} departments");

        // 创建岗位
        var positions = new List<Position>
        {
            new() { Name = "总经理", Code = "CEO", Sort = 1, Status = 1, CreateTime = DateTime.Now },
            new() { Name = "部门经理", Code = "MANAGER", Sort = 2, Status = 1, CreateTime = DateTime.Now },
            new() { Name = "项目经理", Code = "PM", Sort = 3, Status = 1, CreateTime = DateTime.Now },
            new() { Name = "技术总监", Code = "CTO", Sort = 4, Status = 1, CreateTime = DateTime.Now },
            new() { Name = "高级工程师", Code = "SENIOR_ENGINEER", Sort = 5, Status = 1, CreateTime = DateTime.Now },
            new() { Name = "中级工程师", Code = "ENGINEER", Sort = 6, Status = 1, CreateTime = DateTime.Now },
            new() { Name = "初级工程师", Code = "JUNIOR_ENGINEER", Sort = 7, Status = 1, CreateTime = DateTime.Now },
            new() { Name = "产品经理", Code = "PRODUCT_MANAGER", Sort = 8, Status = 1, CreateTime = DateTime.Now },
            new() { Name = "人事专员", Code = "HR_SPECIALIST", Sort = 9, Status = 1, CreateTime = DateTime.Now },
            new() { Name = "财务专员", Code = "FINANCE_SPECIALIST", Sort = 10, Status = 1, CreateTime = DateTime.Now }
        };
        await fsql.Insert(positions).ExecuteAffrowsAsync();
        Console.WriteLine($"Created {positions.Count} positions");

        // 创建用户 (密码: admin123)
        var salt = Guid.NewGuid().ToString("N")[..8];
        var users = new List<User>
        {
            new() { Username = "admin", Password = HashPassword("admin123", salt), Salt = salt, RealName = "系统管理员", Phone = "13800000001", Email = "admin@cwht.com", Gender = 1, Status = 1, DepartmentId = 1, PositionId = 1, IsAdmin = true, CreateTime = DateTime.Now },
            new() { Username = "zhangsan", Password = HashPassword("123456", salt), Salt = salt, RealName = "张三", Phone = "13800000002", Email = "zhangsan@cwht.com", Gender = 1, Status = 1, DepartmentId = 2, PositionId = 4, CreateTime = DateTime.Now },
            new() { Username = "lisi", Password = HashPassword("123456", salt), Salt = salt, RealName = "李四", Phone = "13800000003", Email = "lisi@cwht.com", Gender = 1, Status = 1, DepartmentId = 2, PositionId = 5, CreateTime = DateTime.Now },
            new() { Username = "wangwu", Password = HashPassword("123456", salt), Salt = salt, RealName = "王五", Phone = "13800000004", Email = "wangwu@cwht.com", Gender = 2, Status = 1, DepartmentId = 3, PositionId = 8, CreateTime = DateTime.Now },
            new() { Username = "zhaoliu", Password = HashPassword("123456", salt), Salt = salt, RealName = "赵六", Phone = "13800000005", Email = "zhaoliu@cwht.com", Gender = 2, Status = 1, DepartmentId = 4, PositionId = 9, CreateTime = DateTime.Now },
            new() { Username = "qianqi", Password = HashPassword("123456", salt), Salt = salt, RealName = "钱七", Phone = "13800000006", Email = "qianqi@cwht.com", Gender = 1, Status = 1, DepartmentId = 5, PositionId = 10, CreateTime = DateTime.Now },
            new() { Username = "sunba", Password = HashPassword("123456", salt), Salt = salt, RealName = "孙八", Phone = "13800000007", Email = "sunba@cwht.com", Gender = 1, Status = 1, DepartmentId = 7, PositionId = 7, CreateTime = DateTime.Now },
            new() { Username = "zhoujiu", Password = HashPassword("123456", salt), Salt = salt, RealName = "周九", Phone = "13800000008", Email = "zhoujiu@cwht.com", Gender = 2, Status = 1, DepartmentId = 8, PositionId = 6, CreateTime = DateTime.Now }
        };
        await fsql.Insert(users).ExecuteAffrowsAsync();
        Console.WriteLine($"Created {users.Count} users");

        // 创建角色
        var roles = new List<Role>
        {
            new() { Name = "超级管理员", Code = "SUPER_ADMIN", Sort = 1, DataScope = 1, Status = 1, CreateTime = DateTime.Now },
            new() { Name = "管理员", Code = "ADMIN", Sort = 2, DataScope = 1, Status = 1, CreateTime = DateTime.Now },
            new() { Name = "部门经理", Code = "DEPT_MANAGER", Sort = 3, DataScope = 2, Status = 1, CreateTime = DateTime.Now },
            new() { Name = "普通员工", Code = "EMPLOYEE", Sort = 4, DataScope = 4, Status = 1, CreateTime = DateTime.Now },
            new() { Name = "人事专员", Code = "HR", Sort = 5, DataScope = 3, Status = 1, CreateTime = DateTime.Now },
            new() { Name = "财务专员", Code = "FINANCE", Sort = 6, DataScope = 3, Status = 1, CreateTime = DateTime.Now }
        };
        await fsql.Insert(roles).ExecuteAffrowsAsync();
        Console.WriteLine($"Created {roles.Count} roles");

        // 分配用户角色
        var userRoles = new List<UserRole>
        {
            new() { UserId = 1, RoleId = 1 },
            new() { UserId = 2, RoleId = 2 },
            new() { UserId = 2, RoleId = 3 },
            new() { UserId = 3, RoleId = 4 },
            new() { UserId = 4, RoleId = 4 },
            new() { UserId = 5, RoleId = 5 },
            new() { UserId = 6, RoleId = 6 },
            new() { UserId = 7, RoleId = 4 },
            new() { UserId = 8, RoleId = 4 }
        };
        await fsql.Insert(userRoles).ExecuteAffrowsAsync();
        Console.WriteLine($"Created {userRoles.Count} user-role mappings");

        // 创建菜单
        var menus = new List<Menu>
        {
            new() { Name = "系统管理", ParentId = null, Sort = 1, Path = "/system", Component = "Layout", Type = 1, Icon = "setting", IsVisible = 1, Status = 1, CreateTime = DateTime.Now },
            new() { Name = "用户管理", ParentId = 1, Sort = 1, Path = "user", Component = "/system/user/index", Type = 2, Permission = "system:user:list", Icon = "user", IsVisible = 1, Status = 1, CreateTime = DateTime.Now },
            new() { Name = "角色管理", ParentId = 1, Sort = 2, Path = "role", Component = "/system/role/index", Type = 2, Permission = "system:role:list", Icon = "peoples", IsVisible = 1, Status = 1, CreateTime = DateTime.Now },
            new() { Name = "菜单管理", ParentId = 1, Sort = 3, Path = "menu", Component = "/system/menu/index", Type = 2, Permission = "system:menu:list", Icon = "tree-table", IsVisible = 1, Status = 1, CreateTime = DateTime.Now },
            new() { Name = "部门管理", ParentId = 1, Sort = 4, Path = "dept", Component = "/system/dept/index", Type = 2, Permission = "system:dept:list", Icon = "tree", IsVisible = 1, Status = 1, CreateTime = DateTime.Now },
            new() { Name = "岗位管理", ParentId = 1, Sort = 5, Path = "position", Component = "/system/position/index", Type = 2, Permission = "system:position:list", Icon = "post", IsVisible = 1, Status = 1, CreateTime = DateTime.Now },
            new() { Name = "字典管理", ParentId = 1, Sort = 6, Path = "dict", Component = "/system/dict/index", Type = 2, Permission = "system:dict:list", Icon = "dict", IsVisible = 1, Status = 1, CreateTime = DateTime.Now },
            new() { Name = "审批管理", ParentId = null, Sort = 2, Path = "/approval", Component = "Layout", Type = 1, Icon = "approval", IsVisible = 1, Status = 1, CreateTime = DateTime.Now },
            new() { Name = "请假申请", ParentId = 8, Sort = 1, Path = "leave", Component = "/approval/leave/index", Type = 2, Permission = "approval:leave:list", Icon = "leave", IsVisible = 1, Status = 1, CreateTime = DateTime.Now },
            new() { Name = "出差申请", ParentId = 8, Sort = 2, Path = "business-trip", Component = "/approval/business-trip/index", Type = 2, Permission = "approval:business-trip:list", Icon = "trip", IsVisible = 1, Status = 1, CreateTime = DateTime.Now },
            new() { Name = "费用申请", ParentId = 8, Sort = 3, Path = "expense", Component = "/approval/expense/index", Type = 2, Permission = "approval:expense:list", Icon = "money", IsVisible = 1, Status = 1, CreateTime = DateTime.Now },
            new() { Name = "报销申请", ParentId = 8, Sort = 4, Path = "reimbursement", Component = "/approval/reimbursement/index", Type = 2, Permission = "approval:reimbursement:list", Icon = "invoice", IsVisible = 1, Status = 1, CreateTime = DateTime.Now },
            new() { Name = "我的待办", ParentId = 8, Sort = 5, Path = "todo", Component = "/approval/todo/index", Type = 2, Permission = "approval:todo:list", Icon = "todo", IsVisible = 1, Status = 1, CreateTime = DateTime.Now },
            new() { Name = "我的已办", ParentId = 8, Sort = 6, Path = "done", Component = "/approval/done/index", Type = 2, Permission = "approval:done:list", Icon = "done", IsVisible = 1, Status = 1, CreateTime = DateTime.Now },
            new() { Name = "公告通知", ParentId = null, Sort = 3, Path = "/notice", Component = "Layout", Type = 1, Icon = "notice", IsVisible = 1, Status = 1, CreateTime = DateTime.Now },
            new() { Name = "公告管理", ParentId = 15, Sort = 1, Path = "announcement", Component = "/notice/announcement/index", Type = 2, Permission = "notice:announcement:list", Icon = "announcement", IsVisible = 1, Status = 1, CreateTime = DateTime.Now },
            new() { Name = "我的消息", ParentId = 15, Sort = 2, Path = "message", Component = "/notice/message/index", Type = 2, Permission = "notice:message:list", Icon = "message", IsVisible = 1, Status = 1, CreateTime = DateTime.Now },
            new() { Name = "日程管理", ParentId = null, Sort = 4, Path = "/schedule", Component = "Layout", Type = 1, Icon = "schedule", IsVisible = 1, Status = 1, CreateTime = DateTime.Now },
            new() { Name = "我的日程", ParentId = 18, Sort = 1, Path = "calendar", Component = "/schedule/calendar/index", Type = 2, Permission = "schedule:calendar:list", Icon = "calendar", IsVisible = 1, Status = 1, CreateTime = DateTime.Now },
            new() { Name = "考勤管理", ParentId = null, Sort = 5, Path = "/attendance", Component = "Layout", Type = 1, Icon = "attendance", IsVisible = 1, Status = 1, CreateTime = DateTime.Now },
            new() { Name = "考勤记录", ParentId = 20, Sort = 1, Path = "record", Component = "/attendance/record/index", Type = 2, Permission = "attendance:record:list", Icon = "record", IsVisible = 1, Status = 1, CreateTime = DateTime.Now },
            new() { Name = "工作流管理", ParentId = null, Sort = 6, Path = "/workflow", Component = "Layout", Type = 1, Icon = "workflow", IsVisible = 1, Status = 1, CreateTime = DateTime.Now },
            new() { Name = "流程定义", ParentId = 22, Sort = 1, Path = "definition", Component = "/workflow/definition/index", Type = 2, Permission = "workflow:definition:list", Icon = "definition", IsVisible = 1, Status = 1, CreateTime = DateTime.Now },
            new() { Name = "流程实例", ParentId = 22, Sort = 2, Path = "instance", Component = "/workflow/instance/index", Type = 2, Permission = "workflow:instance:list", Icon = "instance", IsVisible = 1, Status = 1, CreateTime = DateTime.Now }
        };
        await fsql.Insert(menus).ExecuteAffrowsAsync();
        Console.WriteLine($"Created {menus.Count} menus");

        // 创建字典类型
        var dictTypes = new List<DictType>
        {
            new() { Name = "用户性别", Code = "sys_gender", Status = 1, CreateTime = DateTime.Now },
            new() { Name = "菜单状态", Code = "sys_menu_status", Status = 1, CreateTime = DateTime.Now },
            new() { Name = "数据状态", Code = "sys_common_status", Status = 1, CreateTime = DateTime.Now },
            new() { Name = "请假类型", Code = "leave_type", Status = 1, CreateTime = DateTime.Now },
            new() { Name = "审批状态", Code = "approval_status", Status = 1, CreateTime = DateTime.Now },
            new() { Name = "交通工具", Code = "transport_type", Status = 1, CreateTime = DateTime.Now }
        };
        await fsql.Insert(dictTypes).ExecuteAffrowsAsync();
        Console.WriteLine($"Created {dictTypes.Count} dict types");

        // 创建字典数据
        var dictDatas = new List<DictData>
        {
            new() { DictTypeId = 1, Label = "未知", Value = "0", Sort = 1, Status = 1, CreateTime = DateTime.Now },
            new() { DictTypeId = 1, Label = "男", Value = "1", Sort = 2, Status = 1, CreateTime = DateTime.Now },
            new() { DictTypeId = 1, Label = "女", Value = "2", Sort = 3, Status = 1, CreateTime = DateTime.Now },
            new() { DictTypeId = 2, Label = "禁用", Value = "0", Sort = 1, Status = 1, CreateTime = DateTime.Now },
            new() { DictTypeId = 2, Label = "启用", Value = "1", Sort = 2, Status = 1, CreateTime = DateTime.Now },
            new() { DictTypeId = 3, Label = "正常", Value = "0", Sort = 1, Status = 1, CreateTime = DateTime.Now },
            new() { DictTypeId = 3, Label = "停用", Value = "1", Sort = 2, Status = 1, CreateTime = DateTime.Now },
            new() { DictTypeId = 4, Label = "事假", Value = "1", Sort = 1, Status = 1, CreateTime = DateTime.Now },
            new() { DictTypeId = 4, Label = "病假", Value = "2", Sort = 2, Status = 1, CreateTime = DateTime.Now },
            new() { DictTypeId = 4, Label = "年假", Value = "3", Sort = 3, Status = 1, CreateTime = DateTime.Now },
            new() { DictTypeId = 4, Label = "调休", Value = "4", Sort = 4, Status = 1, CreateTime = DateTime.Now },
            new() { DictTypeId = 4, Label = "婚假", Value = "5", Sort = 5, Status = 1, CreateTime = DateTime.Now },
            new() { DictTypeId = 4, Label = "产假", Value = "6", Sort = 6, Status = 1, CreateTime = DateTime.Now },
            new() { DictTypeId = 4, Label = "陪产假", Value = "7", Sort = 7, Status = 1, CreateTime = DateTime.Now },
            new() { DictTypeId = 4, Label = "丧假", Value = "8", Sort = 8, Status = 1, CreateTime = DateTime.Now },
            new() { DictTypeId = 5, Label = "草稿", Value = "0", Sort = 1, Status = 1, CreateTime = DateTime.Now },
            new() { DictTypeId = 5, Label = "审批中", Value = "1", Sort = 2, Status = 1, CreateTime = DateTime.Now },
            new() { DictTypeId = 5, Label = "已通过", Value = "2", Sort = 3, Status = 1, CreateTime = DateTime.Now },
            new() { DictTypeId = 5, Label = "已驳回", Value = "3", Sort = 4, Status = 1, CreateTime = DateTime.Now },
            new() { DictTypeId = 5, Label = "已撤销", Value = "4", Sort = 5, Status = 1, CreateTime = DateTime.Now },
            new() { DictTypeId = 6, Label = "飞机", Value = "1", Sort = 1, Status = 1, CreateTime = DateTime.Now },
            new() { DictTypeId = 6, Label = "火车", Value = "2", Sort = 2, Status = 1, CreateTime = DateTime.Now },
            new() { DictTypeId = 6, Label = "汽车", Value = "3", Sort = 3, Status = 1, CreateTime = DateTime.Now },
            new() { DictTypeId = 6, Label = "自驾", Value = "4", Sort = 4, Status = 1, CreateTime = DateTime.Now }
        };
        await fsql.Insert(dictDatas).ExecuteAffrowsAsync();
        Console.WriteLine($"Created {dictDatas.Count} dict data items");

        Console.WriteLine("System data seeding completed!");
    }

    private static string HashPassword(string password, string salt)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var combinedBytes = System.Text.Encoding.UTF8.GetBytes(password + salt);
        var hashBytes = sha256.ComputeHash(combinedBytes);
        return Convert.ToBase64String(hashBytes);
    }
}
