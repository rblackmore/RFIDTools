namespace ElectroCom.RFIDTools.ReaderServices;

using System.Diagnostics.CodeAnalysis;

using FEDM;

public abstract class ReaderDefinition
{
  private ReaderModule readerModule = new ReaderModule(RequestMode.UniDirectional);
  private ReaderInfo? readerInfo;
  protected uint? deviceId = null!;
  public CommsInterface CommsInterface { get; protected set; } = CommsInterface.None;

  internal ReaderModule ReaderModule => this.readerModule;

  protected virtual Connector? Connector { get; set; }

  public virtual uint DeviceID
  {
    get
    {
      if (deviceId.HasValue && this.deviceId is not 0)
        return this.deviceId.Value;

      if (!IsInfoValid())
        this.readerInfo = ReadReaderInfo();

      this.deviceId = this.readerInfo.deviceId();

      return this.deviceId.Value;
    }
  }

  public virtual string DeviceName
  {
    get
    {
      if (!IsInfoValid())
        this.readerInfo = ReadReaderInfo();

      return this.readerInfo.readerTypeToString();
    }
  }

  public virtual bool IsConnected => this.ReaderModule.isConnected();

  public virtual bool IsValid()
  {
    return this.Connector?.isValid() ?? false;
  }

  [MemberNotNullWhen(true, nameof(readerInfo))]
  private bool IsInfoValid()
  {
    if (this.readerInfo is null)
      return false;

    if (this.readerInfo.deviceId() == 0)
      return false;

    var readerType = this.readerInfo.readerTypeToString();

    if (String.IsNullOrEmpty(readerType))
      return false;

    return true;
  }

  /// <summary>
  /// Connects to the reader.
  /// </summary>
  /// <returns>True if reader is connected. False if Connection is unsuccessful.</returns>
  public bool Connect()
  {
    if (!this.Connector?.isValid() ?? true)
      return false;

    var status = this.ReaderModule.connect(this.Connector);

    if (status is not ErrorCode.Ok)
      return false;

    return this.ReaderModule.isConnected();
  }

  /// <summary>
  /// Disconnects the Reader.
  /// </summary>
  /// <returns>True if reader is disconnected. False if reader is still connected.</returns>
  public bool Disconnect()
  {
    if (this.ReaderModule.isConnected())
      this.ReaderModule.disconnect();

    return !this.ReaderModule.isConnected();
  }

  /// <summary>
  /// Reads Info and Returns it.
  /// If read is not already connected, then it connects and disconnects to read the info first.
  /// </summary>
  /// <returns>ReaderInfo, or null if unable to connect to reader.</returns>
  private ReaderInfo ReadReaderInfo()
  {
    if (this.ReaderModule.isConnected())
    {
      this.ReaderModule.readReaderInfo();
      return this.ReaderModule.info();
    }

    if (Connect())
    {
      this.ReaderModule.readReaderInfo();
      Disconnect();
      return this.ReaderModule.info();
    }

    return this.ReaderModule.info();
  }
}
