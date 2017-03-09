using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using WhosOnFirst.Data.Sql.Interfaces;
using WhosOnFirst.Data.Sql.Models;

namespace WhosOnFirst.Data.Sql.Controllers
{
    public class PersonManager : IPersonManager
    {
        private readonly string connectionString;

        public PersonManager(string connectionString)
        {
            if (connectionString == null) throw new ArgumentNullException(nameof(connectionString));
            this.connectionString = connectionString;
        }

        #region Add
        public void Add(Person person)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText =
                        "INSERT INTO Person (FirstName, LastName, PhoneNumber, EMail, IsPlayer, IsCoach, IsValid, IsAdmin, TeamId) " +
                        "VALUES (@firstName, @lastName, @phoneNumber, @eMail, @IsPlayer, @IsCoach, @IsValid, @IsAdmin, @TeamId)";
                    command.Parameters.AddWithValue("@firstName", person.FirstName);
                    command.Parameters.AddWithValue("@lastName", person.LastName);
                    command.Parameters.AddWithValue("@phoneNumber", person.PhoneNumber);
                    command.Parameters.AddWithValue("@eMail", person.EMail);
                    command.Parameters.AddWithValue("@IsPlayer", person.IsPlayer);
                    command.Parameters.AddWithValue("@IsCoach", person.IsCoach);
                    command.Parameters.AddWithValue("@IsValid", person.IsValid);
                    command.Parameters.AddWithValue("@IsAdmin", person.IsAdmin);
                    command.Parameters.AddWithValue("@TeamId", person.TeamId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public async void AddAsync(Person person)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText =
                        "INSERT INTO Person (FirstName, LastName, PhoneNumber, EMail, IsPlayer, IsCoach, IsValid, IsAdmin, TeamId) " +
                        "VALUES (@firstName, @lastName, @phoneNumber, @eMail, @IsPlayer, @IsCoach, @IsValid, @IsAdmin, @TeamId)";
                    command.Parameters.AddWithValue("@firstName", person.FirstName);
                    command.Parameters.AddWithValue("@lastName", person.LastName);
                    command.Parameters.AddWithValue("@phoneNumber", person.PhoneNumber);
                    command.Parameters.AddWithValue("@eMail", person.EMail);
                    command.Parameters.AddWithValue("@IsPlayer", person.IsPlayer);
                    command.Parameters.AddWithValue("@IsCoach", person.IsCoach);
                    command.Parameters.AddWithValue("@IsValid", person.IsValid);
                    command.Parameters.AddWithValue("@IsAdmin", person.IsAdmin);
                    command.Parameters.AddWithValue("@TeamId", person.TeamId);
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
                    command.CommandText = "DELETE Person WHERE PersonId = @Id";
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
                    command.CommandText = "DELETE Person WHERE PersonId = @Id";
                    command.Parameters.AddWithValue("@Id", Id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        #endregion

        #region GetAll
        public IEnumerable<Person> GetAll()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM Person";
                    using (var reader = command.ExecuteReader())
                    {
                        var list = new List<Person>();
                        while (reader.Read())
                        {
                            var person = new Person();
                            person.PersonId = reader.GetInt32(0);
                            person.FirstName = reader.GetString(1);
                            person.LastName = reader.GetString(2);
                            person.PhoneNumber = reader.GetString(3);
                            person.EMail = reader.GetString(4);
                            person.IsPlayer = reader.GetBoolean(5);
                            person.IsCoach = reader.GetBoolean(6);
                            person.IsValid = reader.GetBoolean(7);
                            person.IsAdmin = false;
                            person.TeamId = reader.GetInt32(9);
                            list.Add(person);
                        }
                        return list;
                    }
                }
            }
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM Person";
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var list = new List<Person>();
                        while (await reader.ReadAsync())
                        {
                            var person = new Person();
                            person.PersonId = reader.GetInt32(0);
                            person.FirstName = reader.GetString(1);
                            person.LastName = reader.GetString(2);
                            person.PhoneNumber = reader.GetString(3);
                            person.EMail = reader.GetString(4);
                            person.IsPlayer = reader.GetBoolean(5);
                            person.IsCoach = reader.GetBoolean(6);
                            person.IsValid = reader.GetBoolean(7);
                            person.IsAdmin = false;
                            person.TeamId = reader.GetInt32(9);
                            list.Add(person);
                        }
                        return list;
                    }
                }
            }
        }
        #endregion

        #region Exists
        public bool Exists(string firstName, string eMail)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM Person WHERE FirstName = @firstName AND EMail = @eMail";
                    command.Parameters.AddWithValue("@firstName", firstName);
                    command.Parameters.AddWithValue("@eMail", eMail);
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

        public async Task<bool> ExistsAsync(string firstName, string eMail)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM Person WHERE FirstName = @firstName AND EMail = @eMail";
                    command.Parameters.AddWithValue("@firstName", firstName);
                    command.Parameters.AddWithValue("@eMail", eMail);
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
        public Person Retrieve(string firstName, string eMail)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText =
                        "SELECT * FROM Person WHERE LOWER (FirstName) = @firstName AND LOWER (EMail) = @eMail";
                    command.Parameters.AddWithValue("@firstName", firstName.ToLower());
                    command.Parameters.AddWithValue("@eMail", eMail.ToLower());
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        var person = new Person();
                        person.PersonId = reader.GetInt32(0);
                        person.FirstName = reader.GetString(1);
                        person.LastName = reader.GetString(2);
                        person.PhoneNumber = reader.GetString(3);
                        person.EMail = reader.GetString(4);
                        person.IsPlayer = reader.GetBoolean(5);
                        person.IsCoach = reader.GetBoolean(6);
                        person.IsValid = reader.GetBoolean(7);
                        person.IsAdmin = reader.GetBoolean(8);
                        person.TeamId = reader.GetInt32(9);
                        return person;
                    }
                }
            }
        }

        public Person Retrieve(int Id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM Person WHERE PersonId = @Id";
                    command.Parameters.AddWithValue("@Id", Id);
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        var person = new Person();
                        person.PersonId = reader.GetInt32(0);
                        person.FirstName = reader.GetString(1);
                        person.LastName = reader.GetString(2);
                        person.PhoneNumber = reader.GetString(3);
                        person.EMail = reader.GetString(4);
                        person.IsPlayer = reader.GetBoolean(5);
                        person.IsCoach = reader.GetBoolean(6);
                        person.IsValid = reader.GetBoolean(7);
                        person.IsAdmin = reader.GetBoolean(8);
                        person.TeamId = reader.GetInt32(9);
                        return person;
                    }
                }
            }
        }

        public async Task<Person> RetrieveAsync(string firstName, string eMail)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText =
                        "SELECT * FROM Person WHERE LOWER (FirstName) = @firstName AND LOWER (EMail) = @eMail";
                    command.Parameters.AddWithValue("@firstName", firstName.ToLower());
                    command.Parameters.AddWithValue("@eMail", eMail.ToLower());
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        await reader.ReadAsync();
                        var person = new Person();
                        person.PersonId = reader.GetInt32(0);
                        person.FirstName = reader.GetString(1);
                        person.LastName = reader.GetString(2);
                        person.PhoneNumber = reader.GetString(3);
                        person.EMail = reader.GetString(4);
                        person.IsPlayer = reader.GetBoolean(5);
                        person.IsCoach = reader.GetBoolean(6);
                        person.IsValid = reader.GetBoolean(7);
                        person.IsAdmin = reader.GetBoolean(8);
                        person.TeamId = reader.GetInt32(9);
                        return person;
                    }
                }
            }
        }

        public async Task<Person> RetrieveAsync(int Id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM Person WHERE PersonId = @Id";
                    command.Parameters.AddWithValue("@Id", Id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        await reader.ReadAsync();
                        var person = new Person();
                        person.PersonId = reader.GetInt32(0);
                        person.FirstName = reader.GetString(1);
                        person.LastName = reader.GetString(2);
                        person.PhoneNumber = reader.GetString(3);
                        person.EMail = reader.GetString(4);
                        person.IsPlayer = reader.GetBoolean(5);
                        person.IsCoach = reader.GetBoolean(6);
                        person.IsValid = reader.GetBoolean(7);
                        person.IsAdmin = reader.GetBoolean(8);
                        person.TeamId = reader.GetInt32(9);
                        return person;
                    }
                }
            }
        }
        #endregion

        #region Update
        public void Update(Person person)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "UPDATE Person SET FirstName = @firstName, LastName = @lastName, " +
                                          "PhoneNumber = @phoneNumber, EMail = @eMail, IsPlayer = @IsPlayer, " +
                                          "IsCoach = @IsCoach, IsValid = @IsValid, IsAdmin = @IsAdmin, " +
                                          "TeamId = @TeamId WHERE PersonId = @personId;";
                    command.Parameters.AddWithValue("@personId", person.PersonId);
                    command.Parameters.AddWithValue("@firstName", person.FirstName);
                    command.Parameters.AddWithValue("@lastName", person.LastName);
                    command.Parameters.AddWithValue("@phoneNumber", person.PhoneNumber);
                    command.Parameters.AddWithValue("@eMail", person.EMail);
                    command.Parameters.AddWithValue("@IsPlayer", person.IsPlayer);
                    command.Parameters.AddWithValue("@IsCoach", person.IsCoach);
                    command.Parameters.AddWithValue("@IsValid", person.IsValid);
                    command.Parameters.AddWithValue("@IsAdmin", person.IsAdmin);
                    command.Parameters.AddWithValue("@TeamId", person.TeamId);
                    command.ExecuteNonQuery();

                }
            }
        }

        public async void UpdateAsync(Person person)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "UPDATE Person SET FirstName = @firstName, LastName = @lastName, " +
                                          "PhoneNumber = @phoneNumber, EMail = @eMail, IsPlayer = @IsPlayer, " +
                                          "IsCoach = @IsCoach, IsValid = @IsValid, IsAdmin = @IsAdmin, " +
                                          "TeamId = @TeamId WHERE PersonId = @personId;";
                    command.Parameters.AddWithValue("@personId", person.PersonId);
                    command.Parameters.AddWithValue("@firstName", person.FirstName);
                    command.Parameters.AddWithValue("@lastName", person.LastName);
                    command.Parameters.AddWithValue("@phoneNumber", person.PhoneNumber);
                    command.Parameters.AddWithValue("@eMail", person.EMail);
                    command.Parameters.AddWithValue("@IsPlayer", person.IsPlayer);
                    command.Parameters.AddWithValue("@IsCoach", person.IsCoach);
                    command.Parameters.AddWithValue("@IsValid", person.IsValid);
                    command.Parameters.AddWithValue("@IsAdmin", person.IsAdmin);
                    command.Parameters.AddWithValue("@TeamId", person.TeamId);
                    await command.ExecuteNonQueryAsync();

                }
            }
        }
        #endregion


    }
}
