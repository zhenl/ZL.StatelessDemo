using Stateless;
using System;


namespace ZL.StatelessDemo
{
    class TestBasic
    {
        public static void Run()
        {
            var myState = new MyState { StateName= "填写申请表" };
            var stateMachine = new StateMachine<string, string>(()=>myState.StateName,s=>myState.StateName=s);

            stateMachine.Configure("请假申请").Permit("提交申请", "部门经理审批");
            stateMachine.Configure("请假申请").OnEntry(T => { OnEntry(T); });
            stateMachine.Configure("请假申请").OnExit(() => { OnExit(stateMachine.State); });
            stateMachine.Configure("填写申请表").SubstateOf("请假申请").Permit("填写完成", "部门经理审批");
            stateMachine.Configure("填写申请表").OnEntry(() => { OnEntry(stateMachine.State); });
            stateMachine.Configure("填写申请表").OnExit(() => { OnExit(stateMachine.State); });
            stateMachine.Configure("部门经理审批").OnEntry(() => { OnEntry(stateMachine.State); });
            stateMachine.Configure("部门经理审批").OnExit(() => {OnExit(stateMachine.State); });
            stateMachine.Configure("部门经理审批").Permit("需要修改", "请假申请");
            stateMachine.Configure("部门经理审批").Permit("审批完成", "结束");

            stateMachine.OnUnhandledTrigger((state, trigger) => { });

            var info = stateMachine.GetInfo();

            while (true)
            {
                //Console.WriteLine(stateMachine.State);
                Console.WriteLine(myState.StateName);
                Console.WriteLine("输入命令");
                var command = Console.ReadLine();
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
            Console.WriteLine("进入"+stateName);
        }

        private static void OnExit(string stateName)
        {
            Console.WriteLine("离开" + stateName);
        }
    }

    class MyState
    {
        public string StateName { get; set; }
    }
}
