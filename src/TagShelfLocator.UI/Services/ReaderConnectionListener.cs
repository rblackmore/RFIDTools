namespace TagShelfLocator.UI.Services;

using System.Threading;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.Messaging;

using FEDM;


using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
      if (scanInfo.isNewReader())
      {
        if (this.reader.isConnected())
          return;

        var usbConnector = scanInfo.connector();

        this.reader.connect(usbConnector);

        var connectionMessage = new ReaderConnectionStateChangedMessage(scanInfo.deviceId(), true);

        this.messenger.Send(connectionMessage);

        this.logger.LogInformation("Reader Connected: {deviceID}", scanInfo.deviceId());
      }

      if (scanInfo.isReaderGone())
      {
        if (this.reader.info().deviceId() == scanInfo.deviceId())
        {
          var disconnectingMessage = new ReaderDisconnecting(scanInfo.deviceId());

          var message = this.messenger.Send(disconnectingMessage);

          await message.RunningTask;

          this.reader.disconnect();

          var connectionMessage = new ReaderConnectionStateChangedMessage(scanInfo.deviceId(), false);

          this.messenger.Send(connectionMessage);

          this.logger.LogInformation("Reader Disconnected: {deviceId}", scanInfo.deviceId());
        }
      }

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
}

public class ReaderDisconnecting
{
  public ReaderDisconnecting(uint deviceID)
  {
    this.DeviceID = deviceID;
  }

  public uint DeviceID { get; }

  public Task RunningTask { get; set; }
}

public record ReaderConnectionStateChangedMessage(uint DeviceID, bool NewConnectionStatus);
