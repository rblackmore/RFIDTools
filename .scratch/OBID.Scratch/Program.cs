using FEDM;

using Microsoft.Extensions.DependencyInjection;

using OBID.Scratch;


await ServiceManager.ConfigureAndRunHost();

var services = ServiceManager.ServiceProvider;

var readerModule = services.GetRequiredService<ReaderModule>();




void PrintDetails(ReaderInfo info)
{
  var deviceId = info.deviceId();
  var deviceType = info.readerTypeToString();
  Console.WriteLine($"{deviceId}: {deviceType}");
}

