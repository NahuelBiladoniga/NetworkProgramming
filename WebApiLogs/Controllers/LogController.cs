using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiLogs.Repository;

namespace WebApiLogs.Controllers
{
    public class LogController
    {
        [Route("logs")]
        [ApiController]
        public class LogsController : ControllerBase
        {
            private readonly LogRepository _logRepository;

            public LogsController()
            {
                _logRepository = LogRepository.GetInstance();
            }

            [HttpGet]
            public async Task<IEnumerable<Log>> GetLogsAsync()
            {
                return (IEnumerable<Log>)Ok(_logRepository.GetLogEntries());
            }
        }
    }
}
