using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using WhosOnFirst.Data.Sql.Interfaces;
using WhosOnFirst.Data.Sql.Models;

namespace WhosOnFirst.Data.Sql.Controllers
{
    public class PlayerManager : IPlayerManager
    {
        private readonly string connectionString;

        public PlayerManager(string connectionString)
        {
            if (connectionString == null) throw new ArgumentNullException(nameof(connectionString));
            this.connectionString = connectionString;
        }

        #region Exists
        public bool Exists(int Id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM Players WHERE PersonId = @personId";
                    command.Parameters.AddWithValue("@personId", Id);
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

        public async Task<bool> ExistsAsync(int Id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM Players WHERE PersonId = @personId";
                    command.Parameters.AddWithValue("@personId", Id);
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

        #region Add
        public void Add(Person person, Players player)
        {
            var personManager = new PersonManager(connectionString);

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    if (!personManager.Exists(person.FirstName, person.EMail))
                    {
                        personManager.Add(person);
                    }

                    person = personManager.Retrieve(person.FirstName, person.EMail);

                    if (!Exists(person.PersonId))
                    {
                        command.CommandText = "INSERT INTO Players (PersonId, PositionRequested, TeamRequested, JerseyNumber, Note, IsPitcher)" +
                            " VALUES (@PersonId, @PositionRequested, @TeamRequested, @JerseyNumber, @Note, @IsPitcher)";
                        command.Parameters.AddWithValue("@PersonId", person.PersonId);
                        command.Parameters.AddWithValue("@PositionRequested", player.PositionRequested);
                        command.Parameters.AddWithValue("@TeamRequested", player.TeamRequested);
                        command.Parameters.AddWithValue("@JerseyNumber", player.JerseyNumber);
                        command.Parameters.AddWithValue("@Note", player.Note);
                        command.Parameters.AddWithValue("@IsPitcher", player.IsPitcher);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public async void AddAsync(Person person, Players player)
        {
            var personManager = new PersonManager(connectionString);

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    if (!personManager.Exists(person.FirstName, person.EMail))
                    {
                        personManager.Add(person);
                    }

                    person = personManager.Retrieve(person.FirstName, person.EMail);

                    if (!await ExistsAsync(person.PersonId))
                    {
                        command.CommandText = "INSERT INTO Players (PersonId, PositionRequested, TeamRequested, JerseyNumber, Note, IsPitcher)" +
                            " VALUES (@PersonId, @PositionRequested, @TeamRequested, @JerseyNumber, @Note, @IsPitcher)";
                        command.Parameters.AddWithValue("@PersonId", person.PersonId);
                        command.Parameters.AddWithValue("@PositionRequested", player.PositionRequested);
                        command.Parameters.AddWithValue("@TeamRequested", player.TeamRequested);
                        command.Parameters.AddWithValue("@JerseyNumber", player.JerseyNumber);
                        command.Parameters.AddWithValue("@Note", player.Note);
                        command.Parameters.AddWithValue("@IsPitcher", player.IsPitcher);
                        await command.ExecuteNonQueryAsync();
                    }
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
                    command.CommandText = "DELETE Players WHERE PersonId = @Id";
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
                    command.CommandText = "DELETE Players WHERE PersonId = @Id";
                    command.Parameters.AddWithValue("@Id", Id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        #endregion

        #region Retrieve
        public Players Retrieve(int Id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM Players WHERE PersonId = @Id";
                    command.Parameters.AddWithValue("@Id", Id);
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        var player = new Players();
                        player.PlayerId = reader.GetInt32(0);
                        player.PersonId = reader.GetInt32(1);
                        player.PositionRequested = reader.GetString(2);
                        player.TeamRequested = reader.GetString(3);
                        player.JerseyNumber = reader.GetInt32(4);
                        player.Note = reader.GetString(5);
                        player.IsPitcher = reader.GetBoolean(6);
                        return player;
                    }
                }
            }
        }

        public async Task<Players> RetrieveAsync(int Id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM Players WHERE PersonId = @Id";
                    command.Parameters.AddWithValue("@Id", Id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        await reader.ReadAsync();
                        var player = new Players();
                        player.PlayerId = reader.GetInt32(0);
                        player.PersonId = reader.GetInt32(1);
                        player.PositionRequested = reader.GetString(2);
                        player.TeamRequested = reader.GetString(3);
                        player.JerseyNumber = reader.GetInt32(4);
                        player.Note = reader.GetString(5);
                        player.IsPitcher = reader.GetBoolean(6);
                        return player;
                    }
                }
            }
        }
        #endregion

        #region Update
        public void Update(Players player)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "UPDATE Players SET PositionRequested = @positionRequested, " +
                                          "TeamRequested = @teamRequested, JerseyNumber = @jerseyNumber, Note = @note " +
                                          "WHERE PlayerId = @playerId";
                    command.Parameters.AddWithValue("@playerId", player.PlayerId);
                    command.Parameters.AddWithValue("@positionRequested", player.PositionRequested);
                    command.Parameters.AddWithValue("@teamRequested", player.TeamRequested);
                    command.Parameters.AddWithValue("@note", player.Note);
                    command.Parameters.AddWithValue("@jerseyNumber", player.JerseyNumber);
                    command.ExecuteNonQuery();
                }
            }
        }

        public async void UpdateAsync(Players player)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "UPDATE Players SET PositionRequested = @positionRequested, " +
                                          "TeamRequested = @teamRequested, Note = @note, JerseyNumber = @jerseyNumber " +
                                          "WHERE PlayerId = @playerId";
                    command.Parameters.AddWithValue("@playerId", player.PlayerId);
                    command.Parameters.AddWithValue("@positionRequested", player.PositionRequested);
                    command.Parameters.AddWithValue("@teamRequested", player.TeamRequested);
                    command.Parameters.AddWithValue("@note", player.Note);
                    command.Parameters.AddWithValue("@jerseyNumber", player.JerseyNumber);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        #endregion

        #region GetAll
        public IEnumerable<Players> GetAll()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM Players";
                    using (var reader = command.ExecuteReader())
                    {
                        var list = new List<Players>();
                        while (reader.Read())
                        {
                            var player = new Players();
                            player.PlayerId = reader.GetInt32(0);
                            player.PersonId = reader.GetInt32(1);
                            player.PositionRequested = reader.GetString(2);
                            player.TeamRequested = reader.GetString(3);
                            player.JerseyNumber = reader.GetInt32(4);
                            player.Note = reader.GetString(5);
                            player.IsPitcher = reader.GetBoolean(6);
                            list.Add(player);
                        }
                        return list;
                    }
                }
            }
        }

        public async Task<IEnumerable<Players>> GetAllAsync()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM Players";
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var list = new List<Players>();
                        while (await reader.ReadAsync())
                        {
                            var player = new Players();
                            player.PlayerId = reader.GetInt32(0);
                            player.PersonId = reader.GetInt32(1);
                            player.PositionRequested = reader.GetString(2);
                            player.TeamRequested = reader.GetString(3);
                            player.JerseyNumber = reader.GetInt32(4);
                            player.Note = reader.GetString(5);
                            player.IsPitcher = reader.GetBoolean(6);
                            list.Add(player);
                        }
                        return list;
                    }
                }
            }
        }
        #endregion
    }
}
