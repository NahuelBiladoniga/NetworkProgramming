using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiLogs.Repository;

namespace WebApiLogs.Controllers
{
    [ApiController]
    [Route("logs")]
    public class LogsController : ControllerBase
    {
        private readonly LogRepository _logRepository;

        public LogsController()
        {
            _logRepository = LogRepository.GetInstance();
        }

        [HttpGet]
        public async Task<IActionResult> GetLogsAsync()
        {
            return Ok(_logRepository.GetLogEntries());
        }
    }
}
