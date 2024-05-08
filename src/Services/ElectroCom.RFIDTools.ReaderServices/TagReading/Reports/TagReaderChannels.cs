namespace ElectroCom.RFIDTools.ReaderServices;

using System.Threading.Channels;

public class TagReaderChannels
{
  public TagReaderChannels(
    ChannelReader<TagReadDataReport> dataChannel,
    ChannelReader<TagReaderTaskStatusUpdate> statusChannel)
  {
    DataChannel = dataChannel;
    StatusChannel = statusChannel;
  }

  public ChannelReader<TagReadDataReport> DataChannel { get; private set; }
  public ChannelReader<TagReaderTaskStatusUpdate> StatusChannel { get; private set; }
}
