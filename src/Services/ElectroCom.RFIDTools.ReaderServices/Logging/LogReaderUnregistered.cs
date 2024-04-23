namespace ElectroCom.RFIDTools.ReaderServices.Logging;

using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.Extensions.Logging;

internal class LogReaderUnregistered : INotificationHandler<ReaderUnregistered>
{
  private readonly ILogger<LogReaderUnregistered> logger;

  public LogReaderUnregistered(ILogger<LogReaderUnregistered> logger)
  {
    this.logger = logger;
  }

  public Task Handle(ReaderUnregistered notification, CancellationToken cancellationToken)
  {
    this.logger.LogInformation("Reader Unregistered {deviceId}: {ReaderType}", notification.DeviceID, notification.ReaderType);
    return Task.CompletedTask;
  }
}
