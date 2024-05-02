namespace OBID.Scratch;

public interface ITagReader
{
  bool IsRunning { get; }

  Task StartReadingAsync(CancellationToken token = default);

  Task StopReadingAsync(CancellationToken token = default);

}
