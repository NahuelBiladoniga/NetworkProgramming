using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public enum LogType
    {
        Information,
        Debug,
        Exception
    }

    public enum LogPriority
    {
        Low,
        Medium,
        High
    }

    public class Log
    {
        public Guid Id { get; }
        public string Event { get; set; }
        public string Author { get; set; }
        public DateTime DateTime { get; set; }
        public string Detail { get; set; }
        public LogPriority? Priority { get; set; }
        public LogType? Type { get; set; }

        public Log()
        {
            Id = Guid.NewGuid();
            DateTime = DateTime.Now;
        }

        public Log(string @event, string author) : this()
        {
            Event = @event;
            Author = author;
        }

        public Log(string @event, string author, string detail) : this(@event, author)
        {
            Detail = detail;
        }

        public Log(string @event, string author, string detail, LogPriority? priority, LogType? type) : this(@event, author, detail)
        {
            Priority = priority;
            Type = type;
        }

        public override bool Equals(object obj)
        {
            return obj is Log log &&
                   Id.Equals(log.Id);
        }

        public override int GetHashCode()
        {
            return 1464574591 + Id.GetHashCode();
        }
    }
}
