namespace OBID.Scratch;

public interface ITagReadingWorker
{
  Task StartAsync(CancellationToken token = default);
  Task StopAsync(CancellationToken token = default);
}
