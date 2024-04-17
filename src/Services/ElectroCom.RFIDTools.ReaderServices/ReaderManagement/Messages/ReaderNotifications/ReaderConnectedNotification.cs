namespace ElectroCom.RFIDTools.ReaderServices.ReaderManagement;

using System;

using MediatR;

public class ReaderConnected : INotification
{
  public ReaderConnected(uint deviceID, string deviceName)
  {
    DeviceID = deviceID;
    DeviceName = deviceName;
  }

  public uint DeviceID { get; }
  public string DeviceName { get; }
}
