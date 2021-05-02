using System;
using DemirorenCase.Core.Abstractions;

namespace DemirorenCase.API.ValueObjects
{
    public class Log : ILog
    {
        public string Message { get; set; }
        public DateTime MessageDate { get; set; }
    }
}