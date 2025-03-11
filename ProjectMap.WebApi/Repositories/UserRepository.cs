using Dapper;
using Microsoft.Data.SqlClient;
using ProjectMap.WebApi.Models;

namespace ProjectMap.WebApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string sqlConnectionString;

        public UserRepository(string sqlConnectionString)
        {
            this.sqlConnectionString = sqlConnectionString;
        }

        public async Task<User> InsertAsync(User user)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                var username = await sqlConnection.ExecuteAsync("INSERT INTO [WorldEditorUser] (Username, Password) VALUES (@Username, @Password)", user);
                return user;
            }
        }

        public async Task<User?> ReadAsync(string username)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QuerySingleOrDefaultAsync<User>("SELECT * FROM [WorldEditorUser] WHERE Username = @Username", new { username });
            }
        }

        public async Task UpdateAsync(User user)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("UPDATE [WorldEditorUser] SET " +
                                                 "Username = @Username, " +
                                                 "Password = @Password, "
                                                 , user);

            }
        }

        public async Task DeleteAsync(string username)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("DELETE FROM [WorldEditorUser] WHERE Username = @Username", new { username });
            }
        }

        public async Task<IEnumerable<User>> ReadAsync()
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QueryAsync<User>("SELECT * FROM [WorldEditorUser]");
            }
        }
    }
}
