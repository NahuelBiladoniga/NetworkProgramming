using Domain.Entities;
using Domain.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiLogs.Helpers;
using WebApiLogs.Responses;

namespace WebApiLogs.Controllers
{
    public class LogController
    {
        [Route("logs")]
        [ApiController]
        public class LogsController : ControllerBase
        {
            private readonly ILogService _logService;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public LogsController(
                ILogService logService,
                IHttpContextAccessor httpContextAccessor)
            {
                _httpContextAccessor = httpContextAccessor;
                _logService = logService;
            }

            [HttpGet]
            public async Task<ActionResult<WebPaginatedResponse<Log>>> GetLogsAsync(int page = 1, int pageSize = 15)
            {
                if (page <= 0 || pageSize <= 0)
                {
                    return BadRequest();
                }

                PaginatedResponse<Log> logsPaginatedResponse =
                    await _logService.GetLogsAsync(page, pageSize);
                if (logsPaginatedResponse == null)
                {
                    return NoContent();
                }

                string route = _httpContextAccessor.HttpContext.Request.Host.Value +
                               _httpContextAccessor.HttpContext.Request.Path;
                WebPaginatedResponse<Log> response =
                    WebPaginationHelper<Log>.GenerateWebPaginatedResponse(logsPaginatedResponse, page, pageSize, route);

                return Ok(response);
            }
        }
    }
}
