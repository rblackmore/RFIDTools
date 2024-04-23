using Microsoft.Extensions.DependencyInjection;

using OBID.Scratch;
using OBID.Scratch.ReaderManagement.Factories;
using OBID.Scratch.ReaderManagement.Model;

await ServiceManager.ConfigureAndRunHost();

var readerManager = ServiceManager.ServiceProvider.GetRequiredService<IReaderManager>();

Console.WriteLine("StartUp");

PrintAllReaderDetails(readerManager);

var tcpReader = ReaderFactory.CreateReader(CommsInterface.TCP);
readerManager.RegisterReader(tcpReader);
Console.WriteLine("TCP Added");
PrintAllReaderDetails(readerManager);
Console.Write("Device ID: ");

if (!UInt32.TryParse(Console.ReadLine(), out uint deviceId))
  Console.WriteLine("Invalid Device ID");

readerManager.SelectReader(deviceId);

readerManager.SelectedReader.Connect();

PrintAllReaderDetails(readerManager);

void PrintAllReaderDetails(IReaderManager readerManager)
{
  foreach (var rd in readerManager.GetReaderDefinitions())
  {
    var deviceId = 0;
    var readerType = rd.ReaderModule.info().readerTypeToString();
    var commsType = rd.CommsInterface.ToString();
    var connected = (rd.ReaderModule.isConnected()) ? "Connected" : "Disconnected";
    Console.WriteLine($"ID: {deviceId} ({connected})");
    Console.WriteLine("------------------");
    Console.WriteLine($"ReaderType: {readerType}");
    Console.WriteLine($"Interface: {commsType}");
    PrintReaderDetails(rd);
    Console.WriteLine();
  }
}

void PrintReaderDetails(ReaderDefinition rd)
{
  if (rd is TCPReaderDefinition tcp)
  {
    Console.WriteLine($"IP Address: {tcp.IPAddress}");
    Console.WriteLine($"IP Port: {tcp.Port}");
  }
}
