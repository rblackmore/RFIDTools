namespace OBID.Scratch;

using System;
using System.Threading;
using System.Threading.Tasks;

internal class BRMTagReaderWorker : ITagReadingWorker
{
  public Task StartAsync(CancellationToken token = default)
  {
    throw new NotImplementedException();
  }

  public Task StopAsync(CancellationToken token = default)
  {
    throw new NotImplementedException();
  }
}
