// using Dapper;
// using Dapper.Contrib.Extensions;
// using Repositories.Interfaces;
// using System.Collections.Generic;
// using System.Data;
// using System.Threading.Tasks;
//
// namespace Repositories
// {
//     public class LogRepository : ILogRepository
//     {
//         private readonly IDbConnection _dbConnection;
//
//         public LogRepository(IDbConnection dbConnection)
//         {
//             _dbConnection = dbConnection;
//         }
//
//         public async Task<IEnumerable<LogDto>> GetLogsAsync()
//         {
//             _dbConnection.Open();
//             IEnumerable<LogDto> logs = await _dbConnection.GetAllAsync<LogDto>();
//             _dbConnection.Close();
//             return logs;
//         }
//
//         public async Task<IEnumerable<LogDto>> GetLogsAsync(int page, int pageSize)
//         {
//             _dbConnection.Open();
//             try
//             {
//                 string countSql = "SELECT COUNT(*) FROM Logs";
//                 int totalLogs = await _dbConnection.ExecuteScalarAsync<int>(countSql);
//                 int offset = (page - 1) * pageSize;
//                 if (offset > totalLogs)
//                 {
//                     return null;
//                 }
//
//                 string sql = "SELECT * FROM Logs LIMIT @PageSize OFFSET @Offset";
//                 return await _dbConnection.QueryAsync<LogDto>(sql, new { PageSize = pageSize, Offset = offset });
//             }
//             finally
//             {
//                 _dbConnection.Close();
//             }
//         }
//
//         public async Task<int> GetTotalLogs()
//         {
//             _dbConnection.Open();
//             string sql = "SELECT COUNT(*) FROM Logs";
//             int totalLogs = await _dbConnection.ExecuteScalarAsync<int>(sql);
//             _dbConnection.Close();
//             return totalLogs;
//         }
//
//         public async Task<LogDto> SaveLogAsync(LogDto logDto)
//         {
//             _dbConnection.Open();
//             int logId = await _dbConnection.InsertAsync(logDto);
//             LogDto responseLogDto = await _dbConnection.GetAsync<LogDto>(logId);
//             _dbConnection.Close();
//             return responseLogDto;
//         }
//     }
// }
