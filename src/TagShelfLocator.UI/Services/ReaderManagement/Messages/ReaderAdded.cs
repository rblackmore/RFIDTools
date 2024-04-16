namespace TagShelfLocator.UI.Services.ReaderManagement.Messages;

using System;

public delegate void ReaderAddedEventHandler(object sender, ReaderAddedEventArgs e);

public class ReaderAddedEventArgs : EventArgs
{
  private readonly uint deviceId;

  public ReaderAddedEventArgs(uint deviceId)
  {
    this.deviceId = deviceId;
  }

  public uint DeviceID => this.deviceId;
}
