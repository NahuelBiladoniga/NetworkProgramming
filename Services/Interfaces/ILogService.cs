using Domain.Entities;
using Domain.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ILogService
    {
        Task<IEnumerable<Log>> GetLogsAsync();
        Task<PaginatedResponse<Log>> GetLogsAsync(int page, int pageSize);
        Task<Log> SaveLogAsync(Log log);
    }
}
