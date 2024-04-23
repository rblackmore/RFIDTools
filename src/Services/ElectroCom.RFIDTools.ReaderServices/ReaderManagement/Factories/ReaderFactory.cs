namespace ElectroCom.RFIDTools.ReaderServices;

public static class ReaderFactory
{
  public static ReaderDefinition CreateReader(
    CommsInterface commsInterface,
    uint deviceId = 0,
    string ipAddress = "192.168.10.10",
    int tcpPort = 10001,
    string comPort = null!,
    string frame = "8E1",
    int baudrate = 38400,
    int busAddress = 255
    )
  {
    return commsInterface switch
    {
      CommsInterface.USB => new USBReaderDefinition(deviceId),
      CommsInterface.TCP => new TCPReaderDefinition(ipAddress, tcpPort),
      CommsInterface.COM => new COMReaderDefinition(comPort, frame, baudrate, busAddress),
      _ => throw new NotImplementedException("Invalid Comms Interface"),
    };
  }
}
