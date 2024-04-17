namespace TagShelfLocator.UI.Services.ReaderManagement.Messages.Handlers;

using System.Threading;
using System.Threading.Tasks;

using MediatR;

using TagShelfLocator.UI.Services.ReaderManagement.Model;

public class USBReaderDiscoveredHandler : INotificationHandler<USBReaderDiscovered>
{
  private readonly IReaderManager readerManager;

  public USBReaderDiscoveredHandler(IReaderManager readerManager)
  {
    this.readerManager = readerManager;
  }

  public Task Handle(USBReaderDiscovered notification, CancellationToken cancellationToken)
  {
    var deviceId = notification.DeviceID;
    var readerType = notification.ReaderType;
    var comms = CommunicationInterface.USB;

    var rd = new ReaderDescription(deviceId, readerType, comms);

    this.readerManager.AddReaderDescription(deviceId, rd);

    return Task.CompletedTask;
  }
}
