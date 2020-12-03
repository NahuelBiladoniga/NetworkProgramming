using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryClient;
using Domain;
using RepositoryClient.Dto;
using System.Collections.Generic;
using WebApiAdmin.Services;

namespace WebApiAdmin.Controllers
{
    [Route("users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly RepositoryHandler _repositoryClient;
        private readonly LogService _logService;

        public UsersController(
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _repositoryClient = new RepositoryHandler();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsersAsync()
        {
            return Ok(await _repositoryClient.GetUsersAsync());
        }

        [HttpPost]
        public async Task<ActionResult<User>> SaveUserAsync(User user)
        {
            var dto = new UserDto()
            {
                Email = user.Email,
                Name = user.Name,
                Password = user.Password
            };
            var responseUser = await _repositoryClient.AddUserAsync(dto);
            _logService.SendLogs("User " + user.Email + " was saved");

            return new CreatedResult(string.Empty, responseUser);
        }

        [HttpPut]
        public async Task<ActionResult<User>> UpdateUserAsync(User user)
        {
            var dto = new UserDto()
            {
                Email = user.Email,
                Password = user.Password,
                Name = user.Name
            };
            
            var responseUser = await _repositoryClient.ModifyUserAsync(dto);
            _logService.SendLogs("User " + user.Email + " was updated");
            return Ok(responseUser);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUserAsync([FromQuery] string email)
        {
            var user = new UserDto()
            {
                Email = email
            };
            _logService.SendLogs("User " + user.Email + " was deleted");
            return Ok(await _repositoryClient.RemoveUserAsync(user));
        }
    }
}
