using DemirorenCase.Common.Services;
using DemirorenCase.Core.Abstractions;
using DemirorenCase.LoggerWorkerService.ValueObjects;

namespace DemirorenCase.LoggerWorkerService
{
    public class LogQueue : RabbitQueue<ILog>
    {

    }
}