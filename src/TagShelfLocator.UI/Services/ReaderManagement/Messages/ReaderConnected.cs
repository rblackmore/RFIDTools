namespace TagShelfLocator.UI.Services.ReaderManagement;

using System;

public delegate void ReaderConnectedEventHandler(object sender, ReaderConnectedEventArgs e);

public class ReaderConnectedEventArgs : EventArgs
{
  public ReaderConnectedEventArgs(uint deviceID, string deviceName)
  {
    DeviceID = deviceID;
    DeviceName = deviceName;
  }

  public uint DeviceID { get; }
  public string DeviceName { get; }
}
