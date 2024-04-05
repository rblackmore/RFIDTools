namespace TagShelfLocator.UI.Services.InventoryService;

using System.Threading;
using System.Threading.Tasks;

public interface ITagInventoryService
{
  bool IsNotRunning { get; }
  bool IsRunning { get; }

  // TODO: I likely should change this to have the channel writer, not the whole channel.
  Task StartAsync(string message = "", CancellationToken cancellationToken = default);
  Task StopAsync(string message = "", CancellationToken cancellationToken = default);
}
