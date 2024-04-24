namespace ElectroCom.RFIDTools.ReaderServices.Logging;

using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.Extensions.Logging;

internal class LogReaderRegistered : INotificationHandler<ReaderRegistered>
{
  private readonly ILogger<LogReaderRegistered> logger;

  public LogReaderRegistered(ILogger<LogReaderRegistered> logger)
  {
    this.logger = logger;
  }

  public Task Handle(ReaderRegistered notification, CancellationToken cancellationToken)
  {
    this.logger.LogInformation("New Reader Registered {DeviceId}: {ReaderName}", notification.DeviceID, notification.DeviceName);
    return Task.CompletedTask;
  }
}
