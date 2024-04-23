namespace ElectroCom.RFIDTools.ReaderServices;

using FEDM;

public abstract class ReaderDefinition
{
  private ReaderModule? readerModule;
  protected uint? deviceId = null!;

  internal ReaderModule ReaderModule
  {
    get
    {
      this.readerModule ??= new ReaderModule(RequestMode.UniDirectional);

      return readerModule!;
    }
  }

  protected abstract Connector Connector { get; set; }

  public abstract CommsInterface CommsInterface { get; }

  public uint DeviceID
  {
    get
    {
      if (!this.deviceId.HasValue || this.deviceId is 0)
      {
        DetectReader();

        this.deviceId = this.ReaderModule.info().deviceId();
      }

      return this.deviceId!.Value;
    }
  }

  public bool DetectReader()
  {
    if (this.ReaderModule.isConnected())
      return true;

    if (Connect())
    {
      Disconnect();
      return true;
    }

    return false;
  }

  public bool IsValid()
  {
    return this.Connector.isValid();
  }

  public bool Connect()
  {
    if (!this.Connector.isValid())
      return false;

    var status = this.ReaderModule.connect(this.Connector);

    if (status is not ErrorCode.Ok)
      return false;

    return this.ReaderModule.isConnected();
  }

  public bool Disconnect()
  {
    if (this.readerModule!.isConnected())
      this.readerModule.disconnect();

    return !this.readerModule.isConnected();
  }
}
