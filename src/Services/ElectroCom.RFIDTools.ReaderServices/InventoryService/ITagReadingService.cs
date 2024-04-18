namespace ElectroCom.RFIDTools.ReaderServices.InventoryService;

using System.Threading;
using System.Threading.Tasks;

public interface ITagReadingService
{
  bool IsNotRunning { get; }
  bool IsRunning { get; }

  Task StartAsync(CancellationToken cancellationToken = default);
  Task StopAsync(CancellationToken cancellationToken = default);
}
