using System.Collections.Generic;

namespace WebApiLogs.Repository
{
    public class LogRepository
    {
        private static LogRepository _instance;

        private readonly List<string> _logs;
        
        protected LogRepository() {
            _logs = new List<string>();
        }

        public static LogRepository GetInstance()
        {
            if (_instance == null)
            {
                _instance = new LogRepository();
                
            }
            return _instance;
        }

        public List<string> GetLogEntries()
        {
            return _logs;
        }

        public void AddLogEntry(string log)
        {
            _logs.Add(log);
        }
    }
}