using Dapper.Contrib.Extensions;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.Dtos
{
    [Table("logs")]
    public class LogDto
    {
        [Key]
        public Guid Id { get; }
        public string Event { get; set; }
        public string Author { get; set; }
        public DateTime DateTime { get; set; }
        public string Detail { get; set; }
        public LogType? Type { get; set; }
    }
}
