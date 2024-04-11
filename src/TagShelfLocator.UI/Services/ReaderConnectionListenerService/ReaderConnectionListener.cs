namespace TagShelfLocator.UI.Services.ReaderConnectionListenerService;

using System.Threading;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.Messaging;

using FEDM;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using TagShelfLocator.UI.Services.ReaderConnectionListenerService.Messages;
using TagShelfLocator.UI.Services.ReaderManagement;

public class ReaderConnectionListener : IHostedService, IUsbListener
{
  private readonly ILogger<ReaderConnectionListener> logger;
  private readonly IMessenger messenger;
  private readonly IHostApplicationLifetime lifetime;
  private readonly IReaderManager readerManager;

  public ReaderConnectionListener(
    ILogger<ReaderConnectionListener> logger,
    IMessenger messenger,
    IHostApplicationLifetime lifetime,
    IReaderManager readerManager)
  {
    this.logger = logger;
    this.messenger = messenger;
    this.lifetime = lifetime;
    this.readerManager = readerManager;
    this.lifetime.ApplicationStopping.Register(HandleGracefulShutdown);
  }

  private async void HandleGracefulShutdown()
  {
    if (!this.Reader.isConnected())
      return;

    var deviceId = this.Reader.info().deviceId();

    await SendDisconnectingNotification(deviceId);

    this.Reader.disconnect();

    this.SendDisconnectedNotification(deviceId);

    this.Reader.Dispose();
  }

  private ReaderModule Reader => this.readerManager.SelectedReader;

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
    if (Reader.isConnected())
      return;

    var usbConnector = scanInfo.connector();

    Reader.connect(usbConnector);
    Reader.readReaderInfo();

    var connectionMessage =
      new ReaderConnected(scanInfo.deviceId(), Reader.info().readerTypeToString());

    messenger.Send(connectionMessage);

    logger.LogInformation("Reader Connected: {deviceID}", scanInfo.deviceId());
  }

  private async Task HandleDisconnectedReader(UsbScanInfo scanInfo)
  {
    await SendDisconnectingNotification(scanInfo.deviceId());

    Reader.disconnect();

    logger.LogInformation("Reader Disconnected: {deviceId}", scanInfo.deviceId());

    this.SendDisconnectedNotification(scanInfo.deviceId());
  }

  private async Task SendDisconnectingNotification(uint deviceId)
  {
    var message = messenger.Send(new ReaderDisconnecting(deviceId));

    // I'm not sure this will totally work,
    // I may receive the message back before all handlers have dealt with it.
    if (message.RunningTasks is not null)
      await Task.WhenAll(message.RunningTasks);
  }

  private void SendDisconnectedNotification(uint deviceId)
  {
    this.messenger.Send(new ReaderDisconnected(deviceId));
  }

  private bool isReaderDisconnected(UsbScanInfo scanInfo)
  {
    return
      scanInfo.isReaderGone() &&
      Reader.info().deviceId() == scanInfo.deviceId();
  }
}
