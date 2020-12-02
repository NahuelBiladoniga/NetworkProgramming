using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces
{
    public class PrivilegesException : Exception
    {
        public PrivilegesException()
        {
        }

        public PrivilegesException(string message) : base(message)
        {
        }
    }
}
