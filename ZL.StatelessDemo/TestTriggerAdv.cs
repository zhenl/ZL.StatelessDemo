using Stateless;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZL.StatelessDemo
{
    public class TestTriggerAdv
    {
        public static void Run()
        {
            var stateMachine = new StateMachine<string, string>("请假申请");
            var assignTrigger = stateMachine.SetTriggerParameters<string,Dictionary<string,string>>("提交");
            stateMachine.Configure("部门经理审批").OnEntryFrom(assignTrigger, (username, dic, T) => SetUserName(T, username, dic));
            stateMachine.Configure("请假申请").PermitIf("提交", "部门经理审批",()=> { return true; });

            

            stateMachine.Configure("部门经理审批").OnEntry(T => { OnEntry(T); });
            stateMachine.Configure("部门经理审批").OnExit(() => { OnExit(stateMachine.State); });



            stateMachine.Configure("部门经理审批").Permit("提交", "结束");
            stateMachine.Configure("结束").OnEntryFrom(assignTrigger, (username, dic, T) => SetUserName(T, username, dic));

            stateMachine.OnUnhandledTrigger((state, trigger) => { });

            Console.WriteLine(stateMachine.State);

            //stateMachine.Fire("提交");
            stateMachine.Fire(assignTrigger, "zzd",new Dictionary<string, string> { {"Days","3" }, { "Name", "ZZD" } });

            Console.WriteLine(stateMachine.State);

            stateMachine.Fire(assignTrigger, "zl", new Dictionary<string, string> { { "Approved", "true" } });

            Console.WriteLine(stateMachine.State);


        }

        private static void SetUserName(Stateless.StateMachine<string, string>.Transition t, string username,Dictionary<string,string> dic)
        {
            Console.WriteLine("用户:" + username);
            foreach(var key in dic.Keys)
            {
                Console.WriteLine(key + ":" + dic[key]);
            }
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
