using Dapper;
using Microsoft.Data.SqlClient;
using ProjectMap.WebApi.Models;

namespace ProjectMap.WebApi.Repositories
{
    public class Object2DRepository : IObject2DRepository
    {
        private readonly string sqlConnectionString;

        public Object2DRepository(string sqlConnectionString)
        {
            this.sqlConnectionString = sqlConnectionString;
        }

        public async Task<Object2D> InsertAsync(Object2D object2D)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                var environmentId = await sqlConnection.ExecuteAsync("INSERT INTO [Object2D] (Id, ObjectType, PositionX, PositionY, Width, Length, Direction, RotationZ, SortingLayer)" +
                                                                                    "VALUES (@Id, @ObjectType, @PositionX, @PositionY, @Width, @Length, @Direction, @RotationZ, @SortingLayer)", object2D);
                return object2D;
            }
        }

        //BlaBlaBla

        public async Task<Object2D?> ReadAsync(Guid id)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QuerySingleOrDefaultAsync<Object2D>("SELECT * FROM [Object2D] WHERE Id = @Id", new { id });
            }
        }

        public async Task<IEnumerable<Object2D>> ReadAsync()
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QueryAsync<Object2D>("SELECT * FROM [Object2D]");
            }
        }
        public async Task UpdateAsync(Object2D object2D)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("UPDATE [Object2D] SET " +
                                                 "PositionX = @PositionX, " +
                                                 "PositionY = @PositionY, " +
                                                 "Width = @Width, " +
                                                 "Length = @Length, " +
                                                 "Direction = @Direction, " +
                                                 "RotationZ = @RotationZ, " +
                                                 "SortingLayer = @SortingLayer"
                                                 , object2D);

            }
        }

        public async Task DeleteAsync(Guid id)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("DELETE FROM [Object2D] WHERE Id = @Id", new { id });
            }
        }

    }
}