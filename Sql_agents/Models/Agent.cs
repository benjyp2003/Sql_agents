using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sql_agents
{
    internal class Agent
    {
        public string CodeName;
        public string RealName;
        public string CurrentLocation;
        public string Status;
        public int NumberOfMissionsCompleted;

        public Agent(string codeName, string realName, string currentLocation, string status, int numberOfMissionsCompleted)
        {
            CodeName = codeName;
            RealName = realName;
            CurrentLocation = currentLocation;
            Status = status;
            NumberOfMissionsCompleted = numberOfMissionsCompleted;
        }

        public override string ToString()
        {
            return $"\n  Agent info: \n" +
                   $"Code name: {CodeName} \n" +
                   $"Real Name: {RealName} \n" +
                   $"Current Location: {CurrentLocation}\n" +
                   $"Status: {Status} \n" +
                   $"Missions Completed: {NumberOfMissionsCompleted}";
        }
    }

}
