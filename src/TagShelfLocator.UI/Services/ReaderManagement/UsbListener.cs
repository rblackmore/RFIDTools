namespace TagShelfLocator.UI.Services.ReaderManagement;
using System.Threading;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.Messaging;

using FEDM;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class UsbListener : IHostedService, IUsbListener
{
  private readonly ILogger<UsbListener> logger;
  private readonly IMessenger messenger;

  public UsbListener(ILogger<UsbListener> logger, IMessenger messenger)
  {
    this.logger = logger;
    this.messenger = messenger;
  }

  public async void onUsbEvent()
  {
    this.logger.LogInformation("USB Events: {readerCount}", UsbManager.readerCount());
    var scanInfo = UsbManager.popDiscover();

    while (scanInfo.isValid())
    {
      await ProcessUsbEventAsync(scanInfo);
      scanInfo = UsbManager.popDiscover();
    }
  }

  public Task StartAsync(CancellationToken cancellationToken = default)
  {
    UsbManager.startDiscover(this);
    return Task.CompletedTask;
  }

  public Task StopAsync(CancellationToken cancellationToken = default)
  {
    UsbManager.stopDiscover();
    return Task.CompletedTask;
  }

  private Task ProcessUsbEventAsync(UsbScanInfo scanInfo, CancellationToken cancellationToken = default)
  {
    if (scanInfo.isNewReader())
      OnReaderDiscovered(scanInfo);

    if (scanInfo.isReaderGone())
      OnReaderGone(scanInfo);

    return Task.CompletedTask;
  }

  private void OnReaderDiscovered(UsbScanInfo scanInfo)
  {
    var newReader = new ReaderModule(RequestMode.UniDirectional);

    this.messenger.Send(new USBReaderDiscovered(newReader, scanInfo.deviceId()));
  }

  private void OnReaderGone(UsbScanInfo scanInfo)
  {
    this.messenger.Send(new USBReaderGone(scanInfo.deviceId()));
  }
}
