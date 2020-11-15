using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess
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
