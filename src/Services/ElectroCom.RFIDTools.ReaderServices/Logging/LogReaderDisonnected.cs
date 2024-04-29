namespace ElectroCom.RFIDTools.ReaderServices.Logging;

using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.Extensions.Logging;

internal class LogReaderDisonnected : INotificationHandler<ReaderDisconnected>
{
  private readonly ILogger<LogReaderDisonnected> logger;

  public LogReaderDisonnected(ILogger<LogReaderDisonnected> logger)
  {
    this.logger = logger;
  }

  public Task Handle(ReaderDisconnected notification, CancellationToken cancellationToken)
  {
    var messageFormat = "Reader Disconnected {DeviceId}: {DeviceName}";

    this.logger.LogInformation(messageFormat, notification.DeviceID, notification.DeviceName);

    return Task.CompletedTask;
  }
}
