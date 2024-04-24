using FEDM;

var readerModule = new ReaderModule(RequestMode.UniDirectional);


var tcpConnector = Connector.createTcpConnector("192.168.10.10");

var info = readerModule.info();

PrintDetails(info);

readerModule.connect(tcpConnector);

readerModule.disconnect();

info = readerModule.info();

PrintDetails(info);



void PrintDetails(ReaderInfo info)
{
  var deviceId = info.deviceId();
  var deviceType = info.readerTypeToString();
  Console.WriteLine($"{deviceId}: {deviceType}");
}

