using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Responses
{
    public class PaginatedResponse<T> where T : class
    {
        public int TotalPages { get; set; }
        public int TotalElements { get; set; }
        public IEnumerable<T> Elements { get; set; }
    }
}
