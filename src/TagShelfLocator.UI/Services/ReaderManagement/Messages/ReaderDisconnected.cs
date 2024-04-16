namespace TagShelfLocator.UI.Services.ReaderManagement;

using System;

public delegate void ReaderDisconnectedEventHandler(object sender, ReaderDisconnectedEventArgs e);

public class ReaderDisconnectedEventArgs : EventArgs
{
  public ReaderDisconnectedEventArgs(uint deviceID)
  {
    DeviceID = deviceID;
  }

  public uint DeviceID { get; }
}
