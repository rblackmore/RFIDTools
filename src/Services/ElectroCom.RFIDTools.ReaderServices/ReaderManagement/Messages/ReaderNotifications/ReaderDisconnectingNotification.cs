namespace ElectroCom.RFIDTools.ReaderServices.ReaderManagement;

using System;

using MediatR;

public class ReaderDisconnecting : INotification
{

  public ReaderDisconnecting(uint DeviceID)
  {
    this.DeviceID = DeviceID;
  }

  public uint DeviceID { get; }
}
