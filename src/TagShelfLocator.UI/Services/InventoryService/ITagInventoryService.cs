namespace TagShelfLocator.UI.Services.InventoryService;

using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

using TagShelfLocator.UI.Core.Model;

public interface ITagInventoryService
{
  bool IsNotRunning { get; }
  bool IsRunning { get; }

  // TODO: I likely should change this to have the channel writer, not the whole channel.
  Task StartAsync(string message = "", CancellationToken cancellationToken = default);
  Task StopAsync(string message = "", CancellationToken cancellationToken = default);
}
