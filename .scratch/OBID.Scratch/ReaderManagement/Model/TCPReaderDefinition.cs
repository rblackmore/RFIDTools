namespace OBID.Scratch.ReaderManagement.Model;

using FEDM;

public class TCPReaderDefinition : ReaderDefinition
{
  public TCPReaderDefinition(string ipAddress, int port)
  {
    this.Connector = Connector.createTcpConnector(ipAddress, port);
  }
  protected override Connector Connector { get; set; }

  public override CommsInterface CommsInterface => CommsInterface.TCP;
  public string IPAddress => this.Connector.tcpIpAddress();
  public int Port => this.Connector.tcpPort();
  public void ChangeIPAddress(string ipAddress) => this.Connector.setTcpIpAddress(ipAddress);
  public void ChangePort(int port) => this.Connector.setTcpPort(port);

}
