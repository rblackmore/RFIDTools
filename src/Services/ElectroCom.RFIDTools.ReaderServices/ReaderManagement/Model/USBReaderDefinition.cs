namespace ElectroCom.RFIDTools.ReaderServices;

using FEDM;

public sealed class USBReaderDefinition : ReaderDefinition
{
  public USBReaderDefinition(uint deviceId)
  {
    this.deviceId = deviceId;
    this.Connector = Connector.createUsbConnector(deviceId);
  }
  public override CommsInterface CommsInterface => CommsInterface.USB;

  protected override Connector Connector { get; set; }

  public void ChangeDeviceID(uint deviceId) => this.Connector.setUsbDeviceId(deviceId);
}
