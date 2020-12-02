using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.IServices
{
    [Serializable]
    public class ServicesException : Exception
    {
        public ServicesException()
        {
        }

        public ServicesException(string message) : base(message)
        {
        }
    }
}
