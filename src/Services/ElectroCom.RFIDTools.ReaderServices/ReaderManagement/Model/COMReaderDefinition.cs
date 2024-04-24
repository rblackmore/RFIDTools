namespace ElectroCom.RFIDTools.ReaderServices;

using FEDM;

public class COMReaderDefinition : ReaderDefinition
{
  public COMReaderDefinition(string port, string frame = "8E1", int baudrate = 38400, int busAddress = 255)
  {
    this.Connector = Connector.createComConnector(port, frame, baudrate, busAddress);
    this.CommsInterface = CommsInterface.COM;
  }

  public string PortName => this.Connector?.comPort() ?? string.Empty;
  public string Frame => this.Connector?.comFrame() ?? string.Empty;
  public int Baudrate => this.Connector?.comBaudrate() ?? 0;
  public int BusAddress => this.Connector?.comBusAddress() ?? 0;

  public void ChangePort(string port) => this.Connector?.setComPort(port);
  public void ChangeFrame(string frame) => this.Connector?.setComFrame(frame);
  public void ChangeBaud(int baudrate) => this.Connector?.setComBaudrate(baudrate);
  public void ChangeBusAddress(int busAddress) => this.Connector?.setComBusAddress(busAddress);
}
