namespace OBID.Scratch;

using System;
using System.Threading;
using System.Threading.Tasks;

internal class BRMTagReaderWorker : ITagReader
{
  public Task StartReadingAsync(CancellationToken token = default)
  {
    throw new NotImplementedException();
  }

  public Task StopReadingAsync(CancellationToken token = default)
  {
    throw new NotImplementedException();
  }
}
