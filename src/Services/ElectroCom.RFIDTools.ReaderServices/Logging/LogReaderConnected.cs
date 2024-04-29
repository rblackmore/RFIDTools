namespace ElectroCom.RFIDTools.ReaderServices.Logging;

using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.Extensions.Logging;

internal class LogReaderConnected : INotificationHandler<ReaderConnected>
{
  private readonly ILogger<LogReaderConnected> logger;

  public LogReaderConnected(ILogger<LogReaderConnected> logger)
  {
    this.logger = logger;
  }

  public Task Handle(ReaderConnected notification, CancellationToken cancellationToken)
  {
    var messageFormat = "Reader Connected {DeviceId}: {DeviceName}";

    this.logger.LogInformation(messageFormat, notification.DeviceID, notification.DeviceName);

    return Task.CompletedTask;
  }
}
