using System.Threading.Tasks;
using Services.Interfaces;
using WebApiAdmin.Responses;
using Domain.Responses;
using Domain;
using WebApiAdmin.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApiAdmin.Controllers
{
    [Route("users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsersController(
            IUserService userService,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<WebPaginatedResponse<User>>> GetUsersAsync(int page = 1, int pageSize = 15)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return BadRequest();
            }

            PaginatedResponse<User> usersPaginatedResponse =
                await _userService.GetUsersAsync(page, pageSize);
            if (usersPaginatedResponse == null)
            {
                return NoContent();
            }

            string route = _httpContextAccessor.HttpContext.Request.Host.Value +
                           _httpContextAccessor.HttpContext.Request.Path;
            WebPaginatedResponse<User> response =
                WebPaginationHelper<User>.GenerateWebPaginatedResponse(usersPaginatedResponse, page, pageSize, route);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserByIdAsync(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user != null)
            {
                return Ok(user);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<User>> SaveUserAsync(User user)
        {
            var responseUser = await _userService.SaveUserAsync(user);
            return new CreatedResult(string.Empty, responseUser);
        }

        [HttpPut]
        public async Task<ActionResult<User>> UpdateUserAsync(User user)
        {
            var responseUser = await _userService.UpdateUserAsync(user);
            return Ok(responseUser);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userService.DeleteUserAsync(user);
            return NoContent();
        }
    }
}
