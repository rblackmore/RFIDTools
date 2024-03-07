namespace TagShelfLocator.UI.Services.InventoryService;

using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

using TagShelfLocator.UI.Core.Model;

public interface ITagInventoryService
{
  bool IsNotRunning { get; }
  bool IsRunning { get; }

  Task StartAsync(Channel<TagEntry> channel, string message = "", CancellationToken cancellationToken = default);
  Task StopAsync(string message, CancellationToken cancellationToken = default);
}
