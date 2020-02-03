using Stateless;
using System;
using System.Collections.Generic;

namespace ZL.StatelessDemo
{
    public class TestTrigger
    {
        private static List<StateMachine<string, string>.TriggerWithParameters<string>> lstTrigger;
        public static void Run()
        {
            var stateMachine = GetStateMachine();

            stateMachine.OnUnhandledTrigger((state, trigger) => { });

            Console.WriteLine(stateMachine.State);

            //stateMachine.Fire("提交");
            stateMachine.Fire(lstTrigger[0], "zzd");

            Console.WriteLine(stateMachine.State);

            stateMachine.Fire(lstTrigger[1], "zl");

            Console.WriteLine(stateMachine.State);


        }

        private static StateMachine<string,string> GetStateMachine()
        {
            var stateMachine = new StateMachine<string, string>("请假申请");
            lstTrigger = new List<StateMachine<string, string>.TriggerWithParameters<string>>();
            var assignTrigger = stateMachine.SetTriggerParameters<string>("提交");
            var assignTrigger1 = stateMachine.SetTriggerParameters<string>("提交1");
            lstTrigger.Add(assignTrigger);
            lstTrigger.Add(assignTrigger1);

            var lstConfig = new List<StateMachine<string, string>.StateConfiguration>();

            lstConfig.Add(stateMachine.Configure("请假申请"));
            lstConfig.Add(stateMachine.Configure("部门经理审批"));
            lstConfig.Add(stateMachine.Configure("结束"));



            lstConfig[0].PermitIf("提交", "部门经理审批",()=> { return true; });
            lstConfig[1].PermitIf("提交1", "结束", () => { return true; });

            for (var i = 0; i < 3; i++)
            {
                var config = lstConfig[i];
                config.OnEntry(T => { OnEntry(T); });
                config.OnExit(() => { OnExit(stateMachine.State); });
                if (i > 0)
                {
                    var trigger = lstTrigger[i - 1];
                    config.OnEntryFrom(trigger, (username, T) => {
                        SetUserName(T, username);
                    });
                }
            }
            return stateMachine;
        }

        private static void SetUserName(Stateless.StateMachine<string, string>.Transition t,string username)
        {
            Console.WriteLine("用户:" + username);
            Console.WriteLine("进入" + t.Destination);
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
