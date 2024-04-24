namespace ElectroCom.RFIDTools.ReaderServices;
using FEDM;

internal class NullReaderDefinition : ReaderDefinition
{
  public NullReaderDefinition()
  {
    this.CommsInterface = CommsInterface.None;
  }

  public override uint DeviceID => 0;
  public override string DeviceName => String.Empty;
  public override bool IsConnected => false;
}
