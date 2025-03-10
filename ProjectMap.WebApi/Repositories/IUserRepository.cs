using ProjectMap.WebApi.Models;

namespace ProjectMap.WebApi.Repositories
{
    public interface IUserRepository
    {
        Task<User> InsertAsync(User user);
        Task<User?> ReadAsync(string username);
        Task UpdateAsync(User user);
        Task DeleteAsync(string username);
        Task<IEnumerable<User>> ReadAsync();
    }
}
