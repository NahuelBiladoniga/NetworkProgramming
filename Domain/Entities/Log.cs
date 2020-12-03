using System;

namespace Domain.Entities
{
    public enum LogType
    {
        Administration,
        Instaphoto
    }

    public class Log
    {
        public Guid Id { get; }
        public string Event { get; set; }
        public string Author { get; set; }
        public DateTime DateTime { get; set; }
        public string Detail { get; set; }
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

        public Log(string @event, string author, string detail, LogType? type) : this(@event, author, detail)
        {
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
