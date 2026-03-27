using CWHT.OA.Domain.Entities.Workflow;
using FreeSql;

namespace CWHT.OA.Seed.Generators;

public static class WorkflowDataGenerator
{
    public static async Task SeedAsync(IFreeSql fsql)
    {
        // 检查是否已存在数据
        var defCount = await fsql.Select<WorkflowDefinition>().CountAsync();
        if (defCount > 0)
        {
            Console.WriteLine("Workflow data already exists, skipping...");
            return;
        }

        // 创建请假审批流程
        var leaveFlow = new WorkflowDefinition
        {
            Name = "请假审批流程",
            Code = "LEAVE_APPROVAL",
            Version = 1,
            Status = 1,
            FormConfig = """{"fields":[{"name":"type","label":"请假类型","type":"select","required":true},{"name":"startTime","label":"开始时间","type":"datetime","required":true},{"name":"endTime","label":"结束时间","type":"datetime","required":true},{"name":"days","label":"请假天数","type":"number","required":true},{"name":"reason","label":"请假原因","type":"textarea","required":true}]}""",
            FlowConfig = """{"nodes":[{"code":"start","name":"开始","type":1},{"code":"dept_leader","name":"部门负责人审批","type":3},{"code":"hr_review","name":"人事审核","type":3},{"code":"end","name":"结束","type":2}]}""",
            CreateBy = 1,
            CreateTime = DateTime.Now
        };
        await fsql.Insert(leaveFlow).ExecuteAffrowsAsync();

        // 创建流程节点
        var leaveNodes = new List<WorkflowNode>
        {
            new() { DefinitionId = leaveFlow.Id, Name = "开始", Code = "start", Type = 1, Sort = 1, PositionX = 100, PositionY = 200 },
            new() { DefinitionId = leaveFlow.Id, Name = "部门负责人审批", Code = "dept_leader", Type = 3, AssigneeType = 3, ApproveType = 1, Sort = 2, PositionX = 300, PositionY = 200 },
            new() { DefinitionId = leaveFlow.Id, Name = "人事审核", Code = "hr_review", Type = 3, AssigneeType = 2, AssigneeConfig = """{"roleCode":"HR"}""", ApproveType = 1, Sort = 3, PositionX = 500, PositionY = 200 },
            new() { DefinitionId = leaveFlow.Id, Name = "结束", Code = "end", Type = 2, Sort = 4, PositionX = 700, PositionY = 200 }
        };
        await fsql.Insert(leaveNodes).ExecuteAffrowsAsync();

        // 创建流转关系
        var leaveTransitions = new List<WorkflowTransition>
        {
            new() { DefinitionId = leaveFlow.Id, FromNodeId = leaveNodes[0].Id, ToNodeId = leaveNodes[1].Id, Sort = 1 },
            new() { DefinitionId = leaveFlow.Id, FromNodeId = leaveNodes[1].Id, ToNodeId = leaveNodes[2].Id, Sort = 2 },
            new() { DefinitionId = leaveFlow.Id, FromNodeId = leaveNodes[2].Id, ToNodeId = leaveNodes[3].Id, Sort = 3 }
        };
        await fsql.Insert(leaveTransitions).ExecuteAffrowsAsync();

        // 创建出差审批流程
        var businessTripFlow = new WorkflowDefinition
        {
            Name = "出差审批流程",
            Code = "BUSINESS_TRIP_APPROVAL",
            Version = 1,
            Status = 1,
            FormConfig = """{"fields":[{"name":"reason","label":"出差事由","type":"textarea","required":true},{"name":"fromCity","label":"出发城市","type":"input","required":true},{"name":"toCity","label":"目的城市","type":"input","required":true},{"name":"startTime","label":"开始时间","type":"datetime","required":true},{"name":"endTime","label":"结束时间","type":"datetime","required":true},{"name":"transportType","label":"交通工具","type":"select","required":true},{"name":"estimatedCost","label":"预计费用","type":"number","required":true}]}""",
            FlowConfig = """{"nodes":[{"code":"start","name":"开始","type":1},{"code":"dept_leader","name":"部门负责人审批","type":3},{"code":"finance_review","name":"财务审核","type":3},{"code":"end","name":"结束","type":2}]}""",
            CreateBy = 1,
            CreateTime = DateTime.Now
        };
        await fsql.Insert(businessTripFlow).ExecuteAffrowsAsync();

        // 创建费用报销流程
        var expenseFlow = new WorkflowDefinition
        {
            Name = "费用报销流程",
            Code = "EXPENSE_APPROVAL",
            Version = 1,
            Status = 1,
            FormConfig = """{"fields":[{"name":"type","label":"费用类型","type":"select","required":true},{"name":"amount","label":"费用金额","type":"number","required":true},{"name":"description","label":"费用说明","type":"textarea","required":true},{"name":"attachments","label":"附件","type":"upload","required":false}]}""",
            FlowConfig = """{"nodes":[{"code":"start","name":"开始","type":1},{"code":"dept_leader","name":"部门负责人审批","type":3},{"code":"finance_review","name":"财务审核","type":3},{"code":"end","name":"结束","type":2}]}""",
            CreateBy = 1,
            CreateTime = DateTime.Now
        };
        await fsql.Insert(expenseFlow).ExecuteAffrowsAsync();

        Console.WriteLine("Workflow data seeding completed!");
    }
}
