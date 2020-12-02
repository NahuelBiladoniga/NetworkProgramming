using Domain;
using Repositories.Dtos;
using Repositories.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;

namespace Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _dbConnection;

        public UserRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync()
        {
            _dbConnection.Open();
            IEnumerable<UserDto> users = await _dbConnection.GetAllAsync<UserDto>();
            _dbConnection.Close();
            return users;
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync(int page, int pageSize)
        {
            _dbConnection.Open();
            try
            {
                string countSql = "SELECT COUNT(*) FROM Users";
                int totalUsers = await _dbConnection.ExecuteScalarAsync<int>(countSql);
                int offset = (page - 1) * pageSize;
                if (offset > totalUsers)
                {
                    return null;
                }

                string sql = "SELECT * FROM Users LIMIT @PageSize OFFSET @Offset";
                return await _dbConnection.QueryAsync<UserDto>(sql, new { PageSize = pageSize, Offset = offset });
            }
            finally
            {
                _dbConnection.Close();
            }
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            _dbConnection.Open();
            UserDto userDto = await _dbConnection.GetAsync<UserDto>(id);
            _dbConnection.Close();
            return userDto;
        }

        public async Task<UserDto> SaveUserAsync(UserDto userDto)
        {
            _dbConnection.Open();
            int userId = await _dbConnection.InsertAsync(userDto);
            UserDto responseUserDto = await _dbConnection.GetAsync<UserDto>(userId);
            _dbConnection.Close();
            return responseUserDto;
        }

        public async Task<UserDto> UpdateUserAsync(UserDto userDto)
        {
            _dbConnection.Open();
            try
            {
                bool success = await _dbConnection.UpdateAsync(userDto);
                if (success)
                {
                    return await _dbConnection.GetAsync<UserDto>(userDto.Id);
                }

                return null;
            }
            finally
            {
                _dbConnection.Close();
            }
        }

        public async Task DeleteUserAsync(UserDto userDto)
        {
            _dbConnection.Open();
            await _dbConnection.DeleteAsync(userDto);
            _dbConnection.Close();
        }

        public async Task<int> GetTotalUsers()
        {
            _dbConnection.Open();
            string sql = "SELECT COUNT(*) FROM Users";
            int totalUsers = await _dbConnection.ExecuteScalarAsync<int>(sql);
            _dbConnection.Close();
            return totalUsers;
        }

        //public Task<UserDto> UpdateUserAsync(UserDto user)
        //{
        //    throw new System.NotImplementedException();
        //}

        //public Task DeleteUserAsync(UserDto user)
        //{
        //    throw new System.NotImplementedException();
        //}
    }
}
