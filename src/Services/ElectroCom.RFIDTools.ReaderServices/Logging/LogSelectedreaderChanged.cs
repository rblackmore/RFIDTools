namespace ElectroCom.RFIDTools.ReaderServices.Logging;

using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.Extensions.Logging;

internal class LogSelectedreaderChanged : INotificationHandler<SelectedReaderChanged>
{
  private readonly ILogger<LogSelectedreaderChanged> logger;

  public LogSelectedreaderChanged(ILogger<LogSelectedreaderChanged> logger)
  {
    this.logger = logger;
  }

  public Task Handle(SelectedReaderChanged notification, CancellationToken cancellationToken)
  {
    var messageFormat = "Reader Selected {DeviceId}: {DeviceName}";

    this.logger.LogInformation(messageFormat, notification.DeviceID, notification.DeviceName);
    return Task.CompletedTask;
  }
}
