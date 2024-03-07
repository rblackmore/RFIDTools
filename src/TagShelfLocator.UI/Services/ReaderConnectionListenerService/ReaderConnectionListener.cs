namespace TagShelfLocator.UI.Services.ReaderConnectionListenerService;

using System.Threading;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.Messaging;

using FEDM;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using TagShelfLocator.UI.Services.ReaderConnectionListenerService.Messages;

public class ReaderConnectionListener : IHostedService, IUsbListener
{
  private readonly ILogger<ReaderConnectionListener> logger;
  private readonly IMessenger messenger;

  private readonly ReaderModule reader;

  public ReaderConnectionListener(
    ILogger<ReaderConnectionListener> logger,
    IMessenger messenger,
    ReaderModule reader)
  {
    this.logger = logger;
    this.reader = reader;
    this.messenger = messenger;
  }

  public async void onUsbEvent()
  {
    var scanInfo = UsbManager.popDiscover();

    while (scanInfo.isValid())
    {
      await ProcessUsbEventAsync(scanInfo);

      scanInfo = UsbManager.popDiscover();
    }
  }

  public Task StartAsync(CancellationToken cancellationToken)
  {
    UsbManager.startDiscover(this);
    return Task.CompletedTask;
  }

  public Task StopAsync(CancellationToken cancellationToken)
  {
    UsbManager.stopDiscover();
    return Task.CompletedTask;
  }

  private async Task ProcessUsbEventAsync(UsbScanInfo scanInfo)
  {
    if (scanInfo.isNewReader())
      HandleNewReader(scanInfo);

    if (isReaderDisconnected(scanInfo))
      await HandleDisconnectedReader(scanInfo);
  }
  private void HandleNewReader(UsbScanInfo scanInfo)
  {
    if (reader.isConnected())
      return;

    var usbConnector = scanInfo.connector();

    reader.connect(usbConnector);
    reader.readReaderInfo();

    var connectionMessage =
      new ReaderConnected(scanInfo.deviceId(), reader.info().readerTypeToString());

    messenger.Send(connectionMessage);

    logger.LogInformation("Reader Connected: {deviceID}", scanInfo.deviceId());
  }

  private async Task HandleDisconnectedReader(UsbScanInfo scanInfo)
  {
    // I need to send this before calling 'disconnect()' so I can wait any tasks using the reader.
    var message = messenger.Send(new ReaderDisconnecting(scanInfo.deviceId()));

    // I'm not sure this will totally work,
    // I may receive the message back before all handlers have dealt with it.
    if (message.RunningTasks is not null)
      await Task.WhenAll(message.RunningTasks);

    reader.disconnect();

    this.messenger.Send(new ReaderDisconnected(scanInfo.deviceId()));

    logger.LogInformation("Reader Disconnected: {deviceId}", scanInfo.deviceId());
  }

  private bool isReaderDisconnected(UsbScanInfo scanInfo)
  {
    return
      scanInfo.isReaderGone() &&
      reader.info().deviceId() == scanInfo.deviceId();
  }
}
