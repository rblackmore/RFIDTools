namespace TagShelfLocator.UI.Services.ReaderManagement;
using System;
using System.Threading;
using System.Threading.Tasks;

using FEDM;

using Microsoft.Extensions.Hosting;

public delegate void ReaderDiscoveredEventHandler(object sender, ReaderDiscoveredEventArgs e);
public delegate void ReaderGoneEventHandler(object sender, ReaderGoneEventArgs e);

public class UsbListener : IHostedService, IUsbListener
{
  public event ReaderDiscoveredEventHandler? ReaderDiscovered;
  public event ReaderGoneEventHandler? ReaderGone;

  public async void onUsbEvent()
  {
    var scanInfo = UsbManager.popDiscover();

    while (scanInfo.isValid())
    {
      await ProcessUsbEventAsync(scanInfo);
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

    var connector = scanInfo.connector();

    newReader.connect(connector);

    if (this.ReaderDiscovered is not null)
      this.ReaderDiscovered(this, new ReaderDiscoveredEventArgs(newReader, scanInfo.deviceId()));
  }

  private void OnReaderGone(UsbScanInfo scanInfo)
  {
    if (this.ReaderGone is not null)
      this.ReaderGone(this, new ReaderGoneEventArgs(scanInfo.deviceId()));
  }
}

public class ReaderDiscoveredEventArgs : EventArgs
{
  public ReaderDiscoveredEventArgs(ReaderModule reader, uint deviceID)
  {
    Reader = reader;
    DeviceID = deviceID;
  }
  public ReaderModule Reader { get; init; }
  public uint DeviceID { get; set; }
}

public class ReaderGoneEventArgs : EventArgs
{
  public ReaderGoneEventArgs(uint deviceID)
  {
    DeviceID = deviceID;
  }

  public uint DeviceID { get; set; }
}
