using Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repositories.Interfaces;
using Repositories.Dtos;
using Domain;
using System.Linq;
using Domain.Responses;
using Domain.Helpers;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            IEnumerable<UserDto> usersDto = await _userRepository.GetUsersAsync();
            return usersDto.Select(userDto => MapStudentDtoToDomain(userDto)).ToList();
        }

        public async Task<PaginatedResponse<User>> GetUsersAsync(int page, int pageSize)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return null;
            }

            int totalStudents = await _userRepository.GetTotalUsers();
            if (totalStudents > 0)
            {
                IEnumerable<UserDto> repoUsers = await _userRepository.GetUsersAsync(page, pageSize);
                if (repoUsers == null || !repoUsers.Any())
                {
                    return null;
                }

                var users = new List<User>();
                foreach (var userDto in repoUsers)
                {
                    users.Add(MapStudentDtoToDomain(userDto));
                }

                return PaginationHelper<User>.GeneratePaginatedResponse(pageSize, totalStudents, users);
            }

            return null;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            UserDto userDto = await _userRepository.GetUserByIdAsync(id);
            if (userDto != null)
            {
                return MapStudentDtoToDomain(userDto);
            }

            return null;
        }

        public async Task<User> SaveUserAsync(User user)
        {
            UserDto userDto = MapStudentDomainToDto(user);
            var responseUserDto = await _userRepository.SaveUserAsync(userDto);
            return MapStudentDtoToDomain(responseUserDto);
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            UserDto userDto = MapStudentDomainToDto(user);
            var responseUserDto = await _userRepository.UpdateUserAsync(userDto);
            return MapStudentDtoToDomain(responseUserDto);
        }

        public async Task DeleteStudentAsync(User user)
        {
            UserDto userDto = MapStudentDomainToDto(user);
            await _userRepository.DeleteUserAsync(userDto);
        }

        private UserDto MapStudentDomainToDto(User user)
        {
            return new UserDto
            {
                Email = user.Email,
                Id = user.Id,
                Name = user.Name
            };
        }

        private User MapStudentDtoToDomain(UserDto userDto)
        {
            return new User
            {
                Email = userDto.Email,
                Id = userDto.Id,
                Name = userDto.Name
            };
        }

        public Task DeleteUserAsync(User user)
        {
            throw new System.NotImplementedException();
        }
    }
}
