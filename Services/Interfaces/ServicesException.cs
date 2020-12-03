﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces
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
