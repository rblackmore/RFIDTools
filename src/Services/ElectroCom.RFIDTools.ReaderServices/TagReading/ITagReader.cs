namespace ElectroCom.RFIDTools.ReaderServices;

using System.Threading;
using System.Threading.Tasks;

public interface ITagReader
{
  bool IsRunning { get; }
  Task<TagReaderChannels> StartReadingAsync(CancellationToken token = default);
  Task StopReadingAsync(CancellationToken token = default);
}
