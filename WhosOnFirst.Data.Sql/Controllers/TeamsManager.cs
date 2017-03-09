using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using WhosOnFirst.Data.Sql.Interfaces;
using WhosOnFirst.Data.Sql.Models;

namespace WhosOnFirst.Data.Sql.Controllers
{
    public class TeamsManager : ITeamsManager
    {
        private readonly string connectionString;

        public TeamsManager(string connectionString)
        {
            if (connectionString == null) throw new ArgumentNullException(nameof(connectionString));
            this.connectionString = connectionString;
        }

        #region Add
        public void Add(string TeamName)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO Teams (TeamName) VALUES (@TeamName)";
                    command.Parameters.AddWithValue("@TeamName", TeamName);
                    command.ExecuteNonQuery();
                }
            }
        }

        public async void AddAsync(string TeamName)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO Teams (TeamName) VALUES (@TeamName)";
                    command.Parameters.AddWithValue("@TeamName", TeamName);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        #endregion

        #region Remove
        public void Remove(int Id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "DELETE Teams WHERE TeamsId = @Id";
                    command.Parameters.AddWithValue("@Id", Id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public async void RemoveAsync(int Id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "DELETE Teams WHERE TeamsId = @Id";
                    command.Parameters.AddWithValue("@Id", Id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        #endregion

        #region Exists
        public bool Exists(string teamName)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT TeamName FROM Teams WHERE LOWER(TeamName) = @teamName";
                    command.Parameters.AddWithValue("@teamName", teamName.ToLower());
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
        }

        public async Task<bool> ExistsAsync(string teamName)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT TeamName FROM Teams WHERE LOWER(TeamName) = @teamName";
                    command.Parameters.AddWithValue("@teamName", teamName.ToLower());
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (!reader.HasRows)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
        }
        #endregion

        #region Retrieve
        public Teams Retrieve(string teamName)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM Teams WHERE LOWER(TeamName) = @teamName";
                    command.Parameters.AddWithValue("@teamName", teamName.ToLower());
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            var team = new Teams();
                            team.TeamsId = 0;
                            team.TeamName = "";
                            return team;
                        }
                        else
                        {
                            reader.Read();
                            var team = new Teams();
                            team.TeamsId = reader.GetInt32(0);
                            team.TeamName = reader.GetString(1);
                            return team;
                        }
                    }
                }
            }
        }

        public async Task<Teams> RetrieveAsync(string teamName)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM Teams WHERE LOWER(TeamName) = @teamName";
                    command.Parameters.AddWithValue("@teamName", teamName.ToLower());
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (!reader.HasRows)
                        {
                            var team = new Teams();
                            team.TeamsId = 0;
                            team.TeamName = "";
                            return team;
                        }
                        else
                        {
                            await reader.ReadAsync();
                            var team = new Teams();
                            team.TeamsId = reader.GetInt32(0);
                            team.TeamName = reader.GetString(1);
                            return team;
                        }
                    }
                }
            }
        }
        #endregion

        #region GetAll
        public IEnumerable<Teams> GetAll()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM Teams";
                    using (var reader = command.ExecuteReader())
                    {
                        var list = new List<Teams>();
                        while (reader.Read())
                        {
                            var team = new Teams();
                            team.TeamsId = reader.GetInt32(0);
                            team.TeamName = reader.GetString(1);
                            list.Add(team);
                        }
                        return list;
                    }
                }
            }
        }

        public async Task<IEnumerable<Teams>> GetAllAsync()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM Teams";
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var list = new List<Teams>();
                        while (await reader.ReadAsync())
                        {
                            var team = new Teams();
                            team.TeamsId = reader.GetInt32(0);
                            team.TeamName = reader.GetString(1);
                            list.Add(team);
                        }
                        return list;
                    }
                }
            }
        }
        #endregion

    }
}
