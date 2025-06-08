using MySql.Data.MySqlClient;
using Sql_agents;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.Remoting.Messaging;

namespace c__SQL.AgentDAL
{
    internal class AgentDAL
    {
        private string connStr = "server=localhost;user=root;password=;database=eagleEyeDB";
        private MySqlConnection _conn;

        public MySqlConnection openConnection()
        {
            if (_conn == null)
            {
                _conn = new MySqlConnection(connStr);
            }

            if (_conn.State != System.Data.ConnectionState.Open)
            {
                _conn.Open();
                Console.WriteLine("Connection successful.");
            }

            return _conn;
        }

        public void closeConnection()
        {
            if (_conn != null && _conn.State == System.Data.ConnectionState.Open)
            {
                _conn.Close();
                _conn = null;
            }
        }

        public AgentDAL()
        {
            try
            {
                openConnection();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
            }
        }

        public void AddAgent(Agent agent)
        {
            try
            {
                using (var conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    var query = @"INSERT INTO agents (code_name, real_name, current_location, status, missions_completed)
                      VALUES (@codeName, @realName, @location, @status, @missions)";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@codeName", agent.CodeName);
                        cmd.Parameters.AddWithValue("@realName", agent.RealName);
                        cmd.Parameters.AddWithValue("@location", agent.CurrentLocation);
                        cmd.Parameters.AddWithValue("@status", agent.Status);
                        cmd.Parameters.AddWithValue("@missions", agent.NumberOfMissionsCompleted);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
            }
        }

        public List<Agent> GetAllAgents()
        {
            try
            {
                List<Agent> agents = new List<Agent>();
                using (var conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    var query = "SELECT * FROM agents";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Agent agent = new Agent(
                                    reader.GetString("code_name"),
                                    reader.GetString("real_name"),
                                    reader.GetString("current_location"),
                                    reader.GetString("status"),
                                    reader.GetInt32("missions_completed"));

                                agents.Add(agent);

                            }
                        }
                    }
                }
                return agents;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
            }
            return null;
        }


        public void UpdateAgentLocation(int agentId, string newLocation)
        {

            using (var conn = new MySqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    var query = @"UPDATE `agents` SET 
                            current_location = @location
                            WHERE agent_id = @id";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@location", newLocation);
                        cmd.Parameters.AddWithValue("@id", agentId);

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine($"MySQL Error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"General Error: {ex.Message}");
                }
            }
        }

        public void UpdateAgentStatus(int agentId, string newStatus)
        {
            try
            {
                using (var conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    var query = @"UPDATE agents SET 
                            status = @status
                            WHERE agent_id = @id";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@status", newStatus);
                        cmd.Parameters.AddWithValue("@id", agentId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
            }
        }


        public void AddMissionCount(int agentId, int missionsToAdd)
        {
            try
            {
                using (var conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    var query = @"
                             UPDATE agents
                         SET missions_completed = missions_completed + @count
                         WHERE agent_id = @id";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@count", missionsToAdd);
                        cmd.Parameters.AddWithValue("@id", agentId);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
            }

        }


        public Agent SearchAgentsByCode(string partialCode)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    var query = @" SELECT * 
                                   FROM `Agents`
                                   WHERE code_name = @code ";

                    using (MySqlCommand cmd = new MySqlCommand( query, conn))
                    {
                        cmd.Parameters.AddWithValue("@code", partialCode);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Agent agent = new Agent(
                                reader.GetString("code_name"),
                                reader.GetString("real_name"),
                                reader.GetString("current_location"),
                                reader.GetString("status"),
                                reader.GetInt32("missions_completed"));
                                return agent;
                            }
                        }
                    }
                    
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
            }
            return null;

        }


        public Dictionary<string, int> CountAgentsByStatus()
        {
            try
            {
                Dictionary<string, int> statusDict = new Dictionary<string, int>();
                using (var conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    var query = @" SELECT status, COUNT(*) count
                                   FROM Agents
                                   GROUP BY status";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                statusDict.Add(reader.GetString("status"), reader.GetInt32("count"));
                            }

                            return statusDict;
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
            }
            return null;
        }
        

        public void DeleteAgent(int agentId)
        {
            try
            {
                using (var conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    var query = "DELETE FROM agents WHERE agent_id = @id";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", agentId);
                        cmd.ExecuteNonQuery();

                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
            }
        }
    
    }
}
