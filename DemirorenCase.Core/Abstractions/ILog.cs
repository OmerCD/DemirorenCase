using System;

namespace DemirorenCase.Core.Abstractions
{
    public interface ILog
    {
        public string Message { get; set; }
        public DateTime MessageDate { get; set; }
    }
}