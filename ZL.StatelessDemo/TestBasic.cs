using Stateless;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZL.StatelessDemo
{
    class TestBasic
    {
        public static void Run()
        {
            var stateMachine = new StateMachine<string, string>("请假申请");

            stateMachine.Configure("请假申请").Permit("提交申请", "部门经理审批");
            stateMachine.Configure("部门经理审批").Permit("需要修改", "请假申请");
            stateMachine.Configure("部门经理审批").Permit("审批完成", "结束");

            stateMachine.OnUnhandledTrigger((state, trigger) => { });

            while (true)
            {
                Console.WriteLine(stateMachine.State);
                Console.WriteLine("输入命令");
                var command = Console.ReadLine();
                stateMachine.Fire(command);

                if (stateMachine.State == "结束") break;
            }
            Console.WriteLine(stateMachine.State);
            Console.WriteLine("流程结束");
        }
    }
}
