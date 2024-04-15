namespace TagShelfLocator.UI.Services.ReaderManagement;

using System;
using System.Collections.Generic;
using System.Linq;

using CommunityToolkit.Mvvm.Messaging;

using FEDM;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using TagShelfLocator.UI.Services.ReaderManagement.Messages;

public class ReaderManager : IReaderManager, IDisposable
{
  private Dictionary<uint, ReaderModule> readerModules = new();

  private readonly ILogger<ReaderManager> logger;
  private readonly IMessenger messenger;
  private readonly IHostApplicationLifetime appLifetime;
  private readonly UsbListener usbListener;

  private uint selectedReaderId;

  public ReaderManager(
    ILogger<ReaderManager> logger,
    IMessenger messenger,
    IHostApplicationLifetime appLifetime,
    UsbListener usbListener)
  {
    this.logger = logger;
    this.messenger = messenger;
    this.appLifetime = appLifetime;
    this.usbListener = usbListener;

    this.usbListener.ReaderDiscovered += UsbListener_ReaderDiscovered;
    this.usbListener.ReaderGone += UsbListener_ReaderGone;
  }

  public ReaderModule this[uint deviceID] => this.readerModules[deviceID];

  public ReaderModule SelectedReader => this.readerModules[selectedReaderId];

  public bool SetSelectedReader(uint readerId)
  {
    if (!this.readerModules.ContainsKey(readerId))
      return false;

    this.selectedReaderId = readerId;

    return true;
  }

  public uint[] GetReaderIDs()
  {
    return this.readerModules.Keys.ToArray();
  }

  public bool TryGetReaderByDeviceID(uint deviceID, out ReaderModule reader)
  {
    return this.readerModules.TryGetValue(deviceID, out reader!);
  }

  private void UsbListener_ReaderDiscovered(object sender, ReaderDiscoveredEventArgs e)
  {
    this.readerModules[e.DeviceID] = e.Reader;

    var connectedMessage =
      new ReaderConnected(e.DeviceID, e.Reader.info().readerTypeToString());

    this.messenger.Send(connectedMessage);
  }

  private void UsbListener_ReaderGone(object sender, ReaderGoneEventArgs e)
  {
    if (this.readerModules.ContainsKey(e.DeviceID))
      this.readerModules.Remove(e.DeviceID);
  }

  public void Dispose()
  {
    this.usbListener.ReaderDiscovered -= UsbListener_ReaderDiscovered;
    this.usbListener.ReaderGone -= UsbListener_ReaderGone;
  }
}

// This is code for logging the reader protocol details to a file.
// This was originally configured in teh App Startup, but this is likely where it should belong int he future.
//var timestamp = DateTime.Now.ToString("dd-HHmmss");

//string logFile = $"protocollog{timestamp}.log";
//var appLoggingParams = AppLoggingParam.createFileLogger(logFile);

//if (builder.Environment.IsDevelopment())
//{
//  Serilog.Log.Logger.Information("Protocol Log File: {logfile}", logFile);
//  reader.log().startLogging(appLoggingParams);
//}
