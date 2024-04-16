namespace TagShelfLocator.UI.Services.ReaderManagement;

using System;
using System.Collections.Generic;
using System.Linq;

using CommunityToolkit.Mvvm.Messaging;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using TagShelfLocator.UI.Services.ReaderManagement.Model;

public class ReaderManager :
  IReaderManager,
  IRecipient<USBReaderDiscovered>,
  IRecipient<USBReaderGone>,
  IDisposable
{
  private Dictionary<uint, ReaderDescription> readers = new();

  private readonly ILogger<ReaderManager> logger;
  private readonly IMessenger messenger;
  private readonly IHostApplicationLifetime appLifetime;

  private uint selectedReaderId;

  public ReaderManager(
    ILogger<ReaderManager> logger,
    IMessenger messenger,
    IHostApplicationLifetime appLifetime)
  {
    this.logger = logger;
    this.messenger = messenger;
    this.appLifetime = appLifetime;

    this.messenger.RegisterAll(this);
  }

  public ReaderDescription this[uint deviceID] => this.readers[deviceID];

  public IReadOnlyList<ReaderDescription> GetReaderDescriptions()
  {
    return this.readers.Values.ToList().AsReadOnly();
  }

  public ReaderDescription SelectedReader => (this.readers.TryGetValue(this.selectedReaderId, out ReaderDescription? r)) ? r : null!;

  public bool SetSelectedReader(uint readerId)
  {
    if (!this.readers.ContainsKey(readerId))
      return false;

    this.selectedReaderId = readerId;

    return true;
  }

  public uint[] GetReaderIDs()
  {
    return this.readers.Keys.ToArray();
  }

  public bool TryGetReaderByDeviceID(uint deviceID, out ReaderDescription reader)
  {
    return this.readers.TryGetValue(deviceID, out reader!);
  }

  public void Receive(USBReaderDiscovered message)
  {
    var rd = ReaderDescription.CreateUSB(message.Reader);

    this.readers[message.DeviceID] = rd;

    this.logger.LogInformation("New USB Reader Discovered ({deviceId})", message.DeviceID);
  }

  public void Receive(USBReaderGone message)
  {
    if (!this.readers.ContainsKey(message.DeviceID))
      return;

    this.readers.Remove(message.DeviceID);

    this.logger.LogInformation("USB Reader Gone ({deviceId})", message.DeviceID);
  }

  public void Dispose()
  {
    //this.usbListener.ReaderDiscovered -= UsbListener_ReaderDiscovered;
    //this.usbListener.ReaderGone -= UsbListener_ReaderGone;
    this.messenger.UnregisterAll(this);
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
