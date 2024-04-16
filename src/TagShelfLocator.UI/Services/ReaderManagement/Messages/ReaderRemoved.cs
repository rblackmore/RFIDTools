namespace TagShelfLocator.UI.Services.ReaderManagement.Messages;
using System;

public delegate void ReaderRemovedEventHandler(object sender, ReaderRemovedEventArgs e);

public class ReaderRemovedEventArgs : EventArgs
{
  private readonly uint deviceId;

  public ReaderRemovedEventArgs(uint deviceId)
  {
    this.deviceId = deviceId;
  }

  public uint DeviceId => this.deviceId;
}
