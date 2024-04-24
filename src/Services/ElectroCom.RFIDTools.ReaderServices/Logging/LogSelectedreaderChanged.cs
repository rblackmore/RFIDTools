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
    this.logger.LogInformation("Selected Reader changed to {DeviceID}: {ReaderName}", notification.DeviceName, notification.ReaderName);
    return Task.CompletedTask;
  }
}
