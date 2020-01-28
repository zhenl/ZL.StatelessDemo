using System;
using Stateless;

namespace ZL.StatelessDemo
{
    class TestParallel
    {
        private static bool b1 = false;
        private static bool b2 = false;
        public static void Run()
        {
            var myState = new MyState { StateName = "请假申请" };
            var stateMachine = new StateMachine<string, string>(() => myState.StateName, s => myState.StateName = s);

            stateMachine.Configure("请假申请").Permit("提交申请", "部门经理审批");
            stateMachine.Configure("请假申请").OnEntry(T => { OnEntry(T); });
            stateMachine.Configure("请假申请").OnExit(() => { OnExit(stateMachine.State); });

            var substate1 = new StateMachine<string, string>("开始");
            substate1.Configure("开始").Permit("审批1", "结束");
            substate1.Configure("结束").OnEntry(() => { b1 = true; OnEntry(substate1.State); });
            substate1.OnUnhandledTrigger((state, trigger) => { });

            var substate2 = new StateMachine<string, string>("开始");
            substate2.Configure("开始").Permit("审批2", "结束");
            substate2.Configure("结束").OnEntry(() => { b2 = true; ; OnEntry(substate2.State); });
            substate2.OnUnhandledTrigger((state, trigger) => { });

            stateMachine.Configure("部门经理审批").OnEntry(() => { OnEntry(stateMachine.State); substate1.Fire("审批1"); substate2.Fire("审批2"); });
            stateMachine.Configure("部门经理审批").OnExit(() => { OnExit(stateMachine.State); });

            //var b1 = false;
            //var b2 = false;

            //stateMachine.Configure("部门经理审批1").SubstateOf("部门经理审批").Permit("审批1", "部门经理审批汇总");
            //stateMachine.Configure("部门经理审批2").SubstateOf("部门经理审批").Permit("审批2", "部门经理审批汇总");

            

            //stateMachine.Configure("部门经理审批汇总").SubstateOf("部门经理审批").PermitIf("审批完成", "结束",()=> { return b1 && b2; });

            stateMachine.Configure("部门经理审批").PermitIf("需要修改", "请假申请", () => { return b1 && b2; });
            stateMachine.Configure("部门经理审批").PermitIf("审批完成", "结束", () => { return b1 && b2; });

            stateMachine.OnUnhandledTrigger((state, trigger) => { });

            

            var info = stateMachine.GetInfo();

            while (true)
            {
                //Console.WriteLine(stateMachine.State);
                Console.WriteLine(myState.StateName);
                Console.WriteLine("输入命令");
                var command = Console.ReadLine();
                //if (command == "b1") b1 = true;
                //if (command == "b2") b2 = true;
                stateMachine.Fire(command);

                if (stateMachine.State == "结束") break;
            }
            Console.WriteLine(stateMachine.State);
            Console.WriteLine("流程结束");
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
