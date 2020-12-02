using Domain;
using Domain.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<PaginatedResponse<User>> GetUsersAsync(int page, int pageSize);
        Task<User> GetUserByIdAsync(int id);
        Task<User> SaveUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task DeleteUserAsync(User user);
    }
}
