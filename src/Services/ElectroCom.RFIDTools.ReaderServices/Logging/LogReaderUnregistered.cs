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
    var messageFormat = "Reader Unregistered {DeviceId}: {DeviceName}";

    this.logger.LogInformation(messageFormat, notification.DeviceID, notification.DeviceName);

    return Task.CompletedTask;
  }
}
