using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories
{
    public class DataAccessException : Exception
    {
        public DataAccessException()
        {
        }

        public DataAccessException(string message) : base(message)
        {
        }
    }
}
