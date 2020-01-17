using Stateless;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZL.StatelessDemo
{
    class TestClass
    {
        public static void Test()
        {

            var node = new StateNode { Name = "请假申请" };
            var node1 = new StateNode { Name = "部门经理审批" };
            var node2= new StateNode { Name = "结束" };

            var condition1 = new Condition { Name = "提交申请" };
            var condition2 = new Condition { Name = "需要修改" };
            var condition3 = new Condition { Name = "审批完成" };

            var stateMachine = new StateMachine<StateNode, Condition>(node);
            stateMachine.Configure(node).Permit(condition1, node1);
            stateMachine.Configure(node1).Permit(condition2, node);
            stateMachine.Configure(node1).Permit(condition3, node2);

            stateMachine.OnUnhandledTrigger((state, trigger) => { });

            stateMachine.Fire(new Condition { Name = "提交申请" });
            var state = stateMachine.State;
            Console.WriteLine(state.Name);
            stateMachine.Fire(condition1);
            state = stateMachine.State;
            Console.WriteLine(state.Name);
        }
        
    }
}
