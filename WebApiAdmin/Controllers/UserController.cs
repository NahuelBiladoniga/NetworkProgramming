using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryClient;
using Domain;
using RepositoryClient.Dto;
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
            IHttpContextAccessor httpContextAccessor, LogService service)
        {
            _httpContextAccessor = httpContextAccessor;
            _repositoryClient = new RepositoryHandler();
            _logService = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersAsync()
        {
            return Ok(await _repositoryClient.GetUsersAsync());
        }

        [HttpPost]
        public async Task<IActionResult> SaveUserAsync([FromBody] User user)
        {
            var dto = new UserDto()
            {
                Email = user.Email,
                Name = user.Name,
                Password = user.Password
            };
            var responseUser = await _repositoryClient.AddUserAsync(dto);
            _logService.SendLogs("User " + user.Email + " was saved");

            if (responseUser.Status == "OK")
            {
                return Ok(responseUser.Message);
            }
            else
            {
                return BadRequest(responseUser.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserAsync([FromBody] User user)
        {
            var dto = new UserDto()
            {
                Email = user.Email,
                Password = user.Password,
                Name = user.Name
            };
            
            var responseUser = await _repositoryClient.ModifyUserAsync(dto);
            _logService.SendLogs("User " + user.Email + " was updated");

            if (responseUser.Status == "OK")
            {
                return Ok(responseUser.Message);
            }
            else
            {
                return BadRequest(responseUser.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUserAsync([FromQuery] string email)
        {
            var user = new UserDto()
            {
                Email = email
            };

            _logService.SendLogs("User " + user.Email + " was deleted");
            var response = await _repositoryClient.RemoveUserAsync(user);

            if (response.Status == "OK") {
                return Ok(response.Message);
            }
            else
            {
                return BadRequest(response.Message);
            }
        }
    }
}
