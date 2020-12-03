using Domain.Entities;
using Domain.Helpers;
using Domain.Responses;
using Repositories.Dtos;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _logRepository;

        public LogService(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task<IEnumerable<Log>> GetLogsAsync()
        {
            IEnumerable<LogDto> logsDto = await _logRepository.GetLogsAsync();
            return logsDto.Select(logDto => MapLogDtoToDomain(logDto)).ToList();
        }

        public async Task<PaginatedResponse<Log>> GetLogsAsync(int page, int pageSize)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return null;
            }

            int totalLogs = await _logRepository.GetTotalLogs();
            if (totalLogs > 0)
            {
                IEnumerable<LogDto> repoLogs = await _logRepository.GetLogsAsync(page, pageSize);
                if (repoLogs == null || !repoLogs.Any())
                {
                    return null;
                }

                var logs = new List<Log>();
                foreach (var logDto in repoLogs)
                {
                    logs.Add(MapLogDtoToDomain(logDto));
                }

                return PaginationHelper<Log>.GeneratePaginatedResponse(pageSize, totalLogs, logs);
            }

            return null;
        }

        public async Task<Log> SaveLogAsync(Log log)
        {
            LogDto LogDto = MapLogDomainToDto(log);
            var responseLogDto = await _logRepository.SaveLogAsync(LogDto);
            return MapLogDtoToDomain(responseLogDto);
        }

        private LogDto MapLogDomainToDto(Log log)
        {
            return new LogDto
            {
                Author = log.Author,
                DateTime = log.DateTime,
                Detail = log.Detail,
                Event = log.Event
            };
        }

        private Log MapLogDtoToDomain(LogDto logDto)
        {
            return new Log
            {
                Author = logDto.Author,
                DateTime = logDto.DateTime,
                Detail = logDto.Detail,
                Event = logDto.Event
            };
        }
    }
}
