using System;

namespace DemirorenCase.Domain.Exceptions
{
    public class NewsOrderException : Exception
    {
        public NewsOrderException(string message) : base(message)
        {
            
        }
    }
}