using Repositories.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserDto>> GetUsersAsync();
        Task<IEnumerable<UserDto>> GetUsersAsync(int page, int pageSize);
        Task<UserDto> GetUserByIdAsync(int id);
        Task<UserDto> SaveUserAsync(UserDto user);
        Task<UserDto> UpdateUserAsync(UserDto user);
        Task DeleteUserAsync(UserDto user);
        Task<int> GetTotalUsers();
    }
}
