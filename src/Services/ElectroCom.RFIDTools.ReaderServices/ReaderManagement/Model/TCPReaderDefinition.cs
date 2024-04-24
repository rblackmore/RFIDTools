namespace ElectroCom.RFIDTools.ReaderServices;

using FEDM;

public class TCPReaderDefinition : ReaderDefinition
{
  public TCPReaderDefinition(string ipAddress, int port)
  {
    this.Connector = Connector.createTcpConnector(ipAddress, port);
    this.CommsInterface = CommsInterface.TCP;
  }
  public string IPAddress => this.Connector?.tcpIpAddress() ?? string.Empty;
  public int Port => this.Connector?.tcpPort() ?? 0;

  public void ChangeIPAddress(string ipAddress) => this.Connector?.setTcpIpAddress(ipAddress);
  public void ChangePort(int port) => this.Connector?.setTcpPort(port);

}
