using c__SQL.AgentDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sql_agents
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Test();
        }

        static void Test()
        {
            AgentDAL dal = new AgentDAL();


            ////  get all agents
            //List<Agent> agentLst = dal.GetAllAgents();
            //foreach (var agent in agentLst)
            //{
            //    Console.WriteLine(agent);
            //}

            //// update location
            //dal.UpdateAgentLocation(5, "At work");


            //// update status
            //dal.UpdateAgentStatus(1, "Retired");

            ////  delete agent
            //dal.DeleteAgent(1);


            ////  search agent by code name
            //Agent foundAgent = dal.SearchAgentsByCode("aaa");
            //Console.WriteLine(foundAgent);


            //// add  succsesfull missions to a agent.  
            //dal.AddMissionCount(3, 2);


            ////  see how many agents there are in each status.
            //Dictionary<string, int> dict =  dal.CountAgentsByStatus();
            //foreach (var item in dict)
            //{
            //    Console.Write(item.Key + " ");
            //    Console.WriteLine(item.Value);
            //}
        }
    }
}
