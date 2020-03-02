using System;
using System.Collections.Generic;
using System.Text;

namespace PW.Services.Exceptions
{
    public class PWException : Exception
    {
        public PWException(string message) : base(message)
        {

        }
    }
}
