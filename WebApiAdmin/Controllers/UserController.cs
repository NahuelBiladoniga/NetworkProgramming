using System.Threading.Tasks;
using WebApiAdmin.Responses;
using Domain.Responses;
using Domain;
using WebApiAdmin.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryClient;
using RepositoryClient.Dto;

namespace WebApiAdmin.Controllers
{
    [Route("users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly RepositoryHandler _repositoryClient;
        
        public UsersController(
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _repositoryClient = new global::RepositoryClient.RepositoryHandler();
        }

        [HttpGet]
        public async Task<ActionResult<WebPaginatedResponse<User>>> GetUsersAsync(int page = 1, int pageSize = 15)
        {
            // if (page <= 0 || pageSize <= 0)
            // {
            //     return BadRequest();
            // }
            //
            // PaginatedResponse<User> usersPaginatedResponse =
            //     await _repositoryClient.GetUsersAsync(page, pageSize);
            // if (usersPaginatedResponse == null)
            // {
            //     return NoContent();
            // }
            //
            // var route = _httpContextAccessor.HttpContext.Request.Host.Value +
            //                _httpContextAccessor.HttpContext.Request.Path;
            // WebPaginatedResponse<User> response =
            //     WebPaginationHelper<User>.GenerateWebPaginatedResponse(usersPaginatedResponse, page, pageSize, route);
            //
            // return Ok(response);
            return Ok();
        }

        // [HttpGet("{id}")]
        // public async Task<ActionResult<User>> GetUserByIdAsync(int id)
        // {
        //     var user = await _userService.GetUserByIdAsync(id);
        //     if (user != null)
        //     {
        //         return Ok(user);
        //     }
        //
        //     return NotFound();
        // }

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
            return Ok(responseUser);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUserAsync([FromQuery] string email)
        {
            var user = new UserDto()
            {
                Email = email
            };
            return Ok(await _repositoryClient.RemoveUserAsync(user));
        }
    }
}
