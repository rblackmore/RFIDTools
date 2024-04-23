namespace OBID.Scratch;

using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

public class FakeTagReadingWorker : ITagReadingWorker
{
  private ChannelWriter<string> channelWriter;
  private CancellationTokenSource cancellationTokenSource;
  private Task runningTask = Task.CompletedTask;

  public FakeTagReadingWorker(ChannelWriter<string> channelWriter)
  {
    this.channelWriter = channelWriter;
  }

  public Task StartAsync(CancellationToken token = default)
  {
    if (!runningTask.IsCompleted)
      return Task.CompletedTask;

    this.cancellationTokenSource = new();

    runningTask = Task.Run(async () =>
    {
      await RunAsync(cancellationTokenSource.Token);
    });

    return Task.CompletedTask;

  }

  public async Task StopAsync(CancellationToken token = default)
  {
    if (runningTask.IsCompleted)
      return;

    cancellationTokenSource?.Cancel();

    await runningTask;
  }

  private async Task RunAsync(CancellationToken token)
  {
    while (!token.IsCancellationRequested)
    {
      await Task.Delay(Random.Shared.Next(5) * 1000);
      await this.channelWriter.WriteAsync("Tag Read");
    }

    this.channelWriter.Complete();
  }
}
