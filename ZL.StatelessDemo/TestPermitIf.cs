using Stateless;
using System;

namespace ZL.StatelessDemo
{
    class TestPermitIf
    {
        public static void Run()
        {
            var days = 0;

            var stateMachine = new StateMachine<string, string>("请假申请");

            stateMachine.Configure("请假申请").PermitIf("提交", "部门经理审批", () => { return days <= 3; });
            stateMachine.Configure("请假申请").PermitIf("提交", "总经理审批", () => { return days >3; });


            stateMachine.Configure("部门经理审批").OnEntry(() => { OnEntry(stateMachine.State); });
            stateMachine.Configure("部门经理审批").OnExit(() => { OnExit(stateMachine.State); });

            stateMachine.Configure("总经理审批").OnEntry(() => { OnEntry(stateMachine.State); });
            stateMachine.Configure("总经理审批").OnExit(() => { OnExit(stateMachine.State); });

            stateMachine.Configure("部门经理审批").Permit("提交", "结束");
            stateMachine.Configure("总经理审批").Permit("提交", "结束");

            stateMachine.OnUnhandledTrigger((state, trigger) => { });


            Console.WriteLine("输入请假天数");
            var command = Console.ReadLine();
            if (int.TryParse(command, out days))
            {
                stateMachine.Fire("提交");
                stateMachine.Fire("提交");

                Console.WriteLine(stateMachine.State);
            }
            else
            {
                Console.WriteLine("输入不正确");
            }

        }
        private static void OnEntry(Stateless.StateMachine<string, string>.Transition t)
        {
            Console.WriteLine("进入" + t.Destination);
        }

        private static void OnEntry(string stateName)
        {
            Console.WriteLine("进入" + stateName);
        }

        private static void OnExit(string stateName)
        {
            Console.WriteLine("离开" + stateName);
        }
    }
}
