namespace TagShelfLocator.UI.Services.ReaderManagement;

using System;


/// <summary>
/// Message to let other services know the reader is disconnecting.
/// This allows them to gracefully stop communication before the reader is disposed
/// Within unmanaged code.
/// </summary>
public delegate void ReaderDisconnectingEventHandler(object sender, ReaderDisconnectingEventArgs e);

public class ReaderDisconnectingEventArgs : EventArgs
{

  public ReaderDisconnectingEventArgs(uint DeviceID)
  {
    this.DeviceID = DeviceID;
  }

  public uint DeviceID { get; }
}
