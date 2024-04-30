namespace OBID.Scratch;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using FEDM;

using Microsoft.Extensions.Hosting;

public class UsbListener : IHostedService, IUsbListener
{
  public ReaderModule ReaderModule { get; }

  public UsbListener(ReaderModule readerModule)
  {
    ReaderModule = readerModule;
  }

  public void onUsbEvent()
  {
    var infos = new List<UsbScanInfo>();

    var scanInfo = UsbManager.popDiscover();

    while (scanInfo.isValid())
    {
      infos.Add(scanInfo);
      scanInfo = UsbManager.popDiscover();
    }

    // Remove Duplicate Events for same DeviceId.
    var uniqueScans = infos
      .GroupBy(scan => scan.deviceId())
      .Select(y => y.First());

    foreach (var scan in uniqueScans)
      ProcessUsbEventAsync(scan);
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

  private void ProcessUsbEventAsync(
    UsbScanInfo scanInfo,
    CancellationToken cancellationToken = default)
  {
    if (scanInfo.isNewReader())
      OnReaderDiscovered(scanInfo);

    if (scanInfo.isReaderGone())
      OnReaderGone(scanInfo);
  }

  private void OnReaderDiscovered(UsbScanInfo scanInfo)
  {
    var connector = scanInfo.connector();
    this.ReaderModule.connect(connector);
  }

  private void OnReaderGone(UsbScanInfo scanInfo)
  {
    if (!this.ReaderModule.isConnected())
    {
      return;
    }

    var scannedId = scanInfo.deviceId();
    var readerId = this.ReaderModule.info().deviceId();

    if (scannedId != readerId)
    {
      return;
    }

    this.ReaderModule.disconnect();
  }
}
