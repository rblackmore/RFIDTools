namespace TagShelfLocator.UI.Services.ReaderManagement;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using FEDM;

using MediatR;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class UsbListener : IHostedService, IUsbListener
{
  private readonly ILogger<UsbListener> logger;
  private readonly IMediator mediator;

  public UsbListener(ILogger<UsbListener> logger, IMediator mediator)
  {
    this.logger = logger;
    this.mediator = mediator;
  }

  public async void onUsbEvent()
  {
    var infos = new List<UsbScanInfo>();

    var scanInfo = UsbManager.popDiscover();

    while (scanInfo.isValid())
    {
      infos.Add(scanInfo);
      scanInfo = UsbManager.popDiscover();
    }
    // This shoudl not be necessary, but for some reason, disconnection events are triggered twice.
    var uniqueScans = infos.GroupBy(scan => scan.deviceId()).Select(y => y.First());

    foreach (var scan in uniqueScans)
      await ProcessUsbEventAsync(scan);
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

  private async Task ProcessUsbEventAsync(UsbScanInfo scanInfo, CancellationToken cancellationToken = default)
  {
    if (scanInfo.isNewReader())
      await OnReaderDiscovered(scanInfo);

    if (scanInfo.isReaderGone())
      await OnReaderGone(scanInfo);
  }

  private async Task OnReaderDiscovered(UsbScanInfo scanInfo)
  {
    var usbConnector = scanInfo.connector();
    await this.mediator.Publish(new USBReaderDiscovered(scanInfo.deviceId(), scanInfo.readerType()));
  }

  private async Task OnReaderGone(UsbScanInfo scanInfo)
  {
    await this.mediator.Publish(new USBReaderGone(scanInfo.deviceId()));
  }
}
