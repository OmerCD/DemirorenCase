using DemirorenCase.Core.Abstractions;

namespace DemirorenCase.LoggerWorkerService.ValueObjects
{
    public class Log : ILog
    {
        public string Message { get; set; }
    }
}