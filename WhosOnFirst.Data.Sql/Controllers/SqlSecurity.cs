using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Threading.Tasks;
using WhosOnFirst.Data.Sql.Interfaces;
using WhosOnFirst.Data.Sql.Models;

namespace WhosOnFirst.Data.Sql.Controllers
{
    public class SqlSecurity : ISecurity
    {
        private readonly string connectionString;

        public SqlSecurity(string connectionString)
        {
            if (connectionString == null) throw new ArgumentNullException(nameof(connectionString));
            this.connectionString = connectionString;
        }


        #region Authenticate
        public UserValidation Authenticate(string username, string password) //
        {

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM UserValidation WHERE LOWER(UserName) = @UserName AND Password = @Password";
                    command.Parameters.AddWithValue("@UserName", username.ToLower());
                    command.Parameters.AddWithValue("@Password", password);
                    using (var reader = command.ExecuteReader())
                    {
                        var person = new UserValidation();
                        if (!reader.HasRows)
                        {
                            person.UserValidationId = 0;
                            person.UserName = "";
                            person.PersonId = 0;
                        }
                        else
                        {
                            reader.Read();
                            person.UserValidationId = reader.GetInt32(0);
                            person.UserName = reader.GetString(1);
                            person.PersonId = reader.GetInt32(2);
                        }
                        return person;

                    }

                }
            }
        }

        public async Task<UserValidation> AuthenticateAsync(string username, string password) //
        {

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM UserValidation WHERE LOWER(UserName) = @UserName AND Password = @Password";
                    command.Parameters.AddWithValue("@UserName", username.ToLower());
                    command.Parameters.AddWithValue("@Password", password);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var person = new UserValidation();
                        if (!reader.HasRows)
                        {
                            person.UserValidationId = 0;
                            person.UserName = "";
                            person.PersonId = 0;
                        }
                        else
                        {
                            await reader.ReadAsync();
                            person.UserValidationId = reader.GetInt32(0);
                            person.UserName = reader.GetString(1);
                            person.PersonId = reader.GetInt32(2);
                        }
                        return person;

                    }

                }
            }
        }
        #endregion

        #region RegisterUser
        public void RegisterUser(UserValidation userValidation, Person person)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO Person (FirstName, LastName, PhoneNumber, EMail, IsPlayer, IsCoach, IsAdmin, IsValid) " +
                                         "VALUES (@firstName, @lastName, @phoneNumber, @eMail, @IsPlayer, @IsCoach, @IsAdmin, @IsValid)";
                    command.Parameters.AddWithValue("@firstName", person.FirstName);
                    command.Parameters.AddWithValue("@lastName", person.LastName);
                    command.Parameters.AddWithValue("@phoneNumber", person.PhoneNumber);
                    command.Parameters.AddWithValue("@eMail", person.EMail);
                    command.Parameters.AddWithValue("@IsPlayer", person.IsPlayer);
                    command.Parameters.AddWithValue("@IsCoach", person.IsCoach);
                    command.Parameters.AddWithValue("@IsAdmin", person.IsAdmin);
                    command.Parameters.AddWithValue("@IsValid", person.IsValid);
                    command.ExecuteNonQuery();
                    command.CommandText = "SELECT * FROM Person WHERE FirstName = @firstName AND LastName = @lastName " +
                        "AND PhoneNumber = @phoneNumber AND EMAIL = @eMail";
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        person.PersonId = reader.GetInt32(0);
                    }
                    command.CommandText =
                        "INSERT INTO UserValidation (UserName, PersonId, Password) " +
                        "VALUES (@userName, @personId, @password)";
                    command.Parameters.AddWithValue("@userName", userValidation.UserName);
                    command.Parameters.AddWithValue("@personId", person.PersonId);
                    command.Parameters.AddWithValue("@password", userValidation.Password);
                    command.ExecuteNonQuery();
                }
            }
        }

        public async void RegisterUserAsync(UserValidation userValidation, Person person)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO Person (FirstName, LastName, PhoneNumber, EMail, IsPlayer, IsCoach, IsAdmin, IsValid) " +
                                         "VALUES (@firstName, @lastName, @phoneNumber, @eMail, @IsPlayer, @IsCoach, @IsAdmin, @IsValid)";
                    command.Parameters.AddWithValue("@firstName", person.FirstName);
                    command.Parameters.AddWithValue("@lastName", person.LastName);
                    command.Parameters.AddWithValue("@phoneNumber", person.PhoneNumber);
                    command.Parameters.AddWithValue("@eMail", person.EMail);
                    command.Parameters.AddWithValue("@IsPlayer", person.IsPlayer);
                    command.Parameters.AddWithValue("@IsCoach", person.IsCoach);
                    command.Parameters.AddWithValue("@IsAdmin", person.IsAdmin);
                    command.Parameters.AddWithValue("@IsValid", person.IsValid);
                    await command.ExecuteNonQueryAsync();
                    command.CommandText = "SELECT * FROM Person WHERE FirstName = @firstName AND LastName = @lastName " +
                        "AND PhoneNumber = @phoneNumber AND EMAIL = @eMail";
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        await reader.ReadAsync();
                        person.PersonId = reader.GetInt32(0);
                    }
                    command.CommandText =
                        "INSERT INTO UserValidation (UserName, PersonId, Password) " +
                        "VALUES (@userName, @personId, @password)";
                    command.Parameters.AddWithValue("@userName", userValidation.UserName);
                    command.Parameters.AddWithValue("@personId", person.PersonId);
                    command.Parameters.AddWithValue("@password", userValidation.Password);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        #endregion

        #region GetAll
        public IEnumerable<UserValidation> GetAll()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM UserValidation";
                    using (var reader = command.ExecuteReader())
                    {
                        var list = new List<UserValidation>();
                        while (reader.Read())
                        {
                            var userValidation = new UserValidation();
                            userValidation.UserValidationId = reader.GetInt32(0);
                            userValidation.UserName = reader.GetString(1);
                            userValidation.PersonId = reader.GetInt32(2);
                            list.Add(userValidation);
                        }
                        return list;
                    }
                }
            }
        }

        public async Task<IEnumerable<UserValidation>> GetAllAsync()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM UserValidation";
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var list = new List<UserValidation>();
                        while (await reader.ReadAsync())
                        {
                            var userValidation = new UserValidation();
                            userValidation.UserValidationId = reader.GetInt32(0);
                            userValidation.UserName = reader.GetString(1);
                            userValidation.PersonId = reader.GetInt32(2);
                            list.Add(userValidation);
                        }
                        return list;
                    }
                }
            }
        }
        #endregion

        #region Retrieve
        public UserValidation Retrieve(int Id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM UserValidation WHERE PersonId = @personId";
                    command.Parameters.AddWithValue("@personId", Id);
                    using (var reader = command.ExecuteReader())
                    {
                        var userValidation = new UserValidation();
                        if (!reader.HasRows)
                        {
                            userValidation.UserValidationId = 0;
                            userValidation.UserName = "";
                        }
                        else
                        {
                            reader.Read();
                            userValidation.UserValidationId = reader.GetInt32(0);
                            userValidation.UserName = reader.GetString(1);
                        }
                        return userValidation;
                    }
                }
            }
        }

        public async Task<UserValidation> RetrieveAsync(int Id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM UserValidation WHERE PersonId = @personId";
                    command.Parameters.AddWithValue("@personId", Id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var userValidation = new UserValidation();
                        if (!reader.HasRows)
                        {
                            userValidation.UserValidationId = 0;
                            userValidation.UserName = "";
                        }
                        else
                        {
                            await reader.ReadAsync();
                            userValidation.UserValidationId = reader.GetInt32(0);
                            userValidation.UserName = reader.GetString(1);
                        }
                        return userValidation;
                    }
                }
            }
        }
        #endregion

        #region Update
        public void Update(UserValidation userValidation, bool passwordExists)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    if (passwordExists)
                    {
                        command.CommandText = "UPDATE UserValidation SET UserName = @userName, Password = @password " +
                                              "WHERE UserValidationId = @userValidationId;";

                        command.Parameters.AddWithValue("@password", GetPasswordHash(userValidation.Password));
                    }
                    else
                    {
                        command.CommandText = "UPDATE UserValidation SET UserName = @userName " +
                                              "WHERE UserValidationId = @userValidationId;";
                    }
                    command.Parameters.AddWithValue("@userValidationId", userValidation.UserValidationId);
                    command.Parameters.AddWithValue("@userName", userValidation.UserName);
                    command.ExecuteNonQuery();

                }
            }
        }

        public async void UpdateAsync(UserValidation userValidation, bool passwordExists)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    if (passwordExists)
                    {
                        command.CommandText = "UPDATE UserValidation SET UserName = @userName, Password = @password " +
                                              "WHERE UserValidationId = @userValidationId;";

                        command.Parameters.AddWithValue("@password", GetPasswordHash(userValidation.Password));
                    }
                    else
                    {
                        command.CommandText = "UPDATE UserValidation SET UserName = @userName " +
                                              "WHERE UserValidationId = @userValidationId;";
                    }
                    command.Parameters.AddWithValue("@userValidationId", userValidation.UserValidationId);
                    command.Parameters.AddWithValue("@userName", userValidation.UserName);
                    await command.ExecuteNonQueryAsync();

                }
            }
        }
        #endregion

        #region PasswordHash
        public static string GetPasswordHash(string password)
        {

            //STEP 1 Create the salt value with a cryptographic PRNG:
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            //STEP 2 Create the Rfc2898DeriveBytes and get the hash value:
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            //STEP 3 Combine the salt and password bytes for later use:
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            //STEP 4 Turn the combined salt+hash into a string for storage
            string savedPasswordHash = Convert.ToBase64String(hashBytes);

            return savedPasswordHash;

        }

        public static bool ComparePasswordHash(string password, string passwordHash)
        {

            /* Fetch the stored value */
            string savedPasswordHash = passwordHash;
            /* Extract the bytes */
            byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);
            /* Get the salt */
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            /* Compute the hash on the password the user entered */
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            /* Compare the results */
            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                {
                    return false;
                }
            }

            return true;
        }
        #endregion

        #region CheckDbConnection
        public bool CheckDbConnection()
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    return true;
                }
            }
            catch (Exception)
            {
                return false; // any error is considered as db connection error for now
            }
        }

        public async Task<bool> CheckDbConnectionAsync()
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    return true;
                }
            }
            catch (Exception)
            {
                return false; // any error is considered as db connection error for now
            }
        }
        #endregion

    }
}
