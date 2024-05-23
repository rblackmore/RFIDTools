namespace ElectroCom.RFIDTools.ReaderServices;

using System.Threading.Channels;

public class TagReaderChannels
{
  public TagReaderChannels(
    ChannelReader<TagReaderDataReport> dataChannel,
    ChannelReader<TagReaderProcessStatusUpdate> statusChannel)
  {
    DataChannel = dataChannel;
    StatusChannel = statusChannel;
  }

  public ChannelReader<TagReaderDataReport> DataChannel { get; private set; }
  public ChannelReader<TagReaderProcessStatusUpdate> StatusChannel { get; private set; }

  internal static (Channel<TagReaderDataReport>, Channel<TagReaderProcessStatusUpdate>) CreateChannels()
  {
    var dataChannel = Channel.CreateUnbounded<TagReaderDataReport>(
      new UnboundedChannelOptions
      {
        SingleReader = true,
        SingleWriter = true,
      });

    var statusChannel = Channel.CreateUnbounded<TagReaderProcessStatusUpdate>(
      new UnboundedChannelOptions
      {
        SingleReader = true,
        SingleWriter = true,
      });

    return (dataChannel, statusChannel);
  }
}
