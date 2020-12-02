using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.IServices
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
