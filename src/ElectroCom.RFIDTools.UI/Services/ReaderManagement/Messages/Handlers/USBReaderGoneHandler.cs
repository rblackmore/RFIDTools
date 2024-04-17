namespace TagShelfLocator.UI.Services.ReaderManagement.Messages.Handlers;

using System.Threading;
using System.Threading.Tasks;

using MediatR;

public class USBReaderGoneHandler : INotificationHandler<USBReaderGone>
{
  private readonly IReaderManager readerManager;

  public USBReaderGoneHandler(IReaderManager readerManager)
  {
    this.readerManager = readerManager;
  }

  public Task Handle(USBReaderGone notification, CancellationToken cancellationToken)
  {
    var deviceId = notification.DeviceID;

    this.readerManager.RemoveReaderDescription(deviceId);

    return Task.CompletedTask;
  }
}
