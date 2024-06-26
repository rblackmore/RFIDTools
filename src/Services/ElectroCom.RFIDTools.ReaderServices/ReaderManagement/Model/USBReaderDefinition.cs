﻿namespace ElectroCom.RFIDTools.ReaderServices;

using FEDM;

public sealed class USBReaderDefinition : ReaderDefinition
{
  public USBReaderDefinition(uint deviceId)
  {
    this.deviceId = deviceId;
    this.Connector = Connector.createUsbConnector(deviceId);
    this.CommsInterface = CommsInterface.USB;
  }

  public void ChangeDeviceID(uint deviceId)
  {
    if (deviceId <= 0)
      return;

    this.deviceId = deviceId;
    this.Connector?.setUsbDeviceId(deviceId);
  }

  public override bool IsValid()
  {
    if (this.DeviceID == 0)
      return false;

    return base.IsValid();
  }
}
