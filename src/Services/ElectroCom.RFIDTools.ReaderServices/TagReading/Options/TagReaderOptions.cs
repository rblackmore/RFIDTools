namespace ElectroCom.RFIDTools.ReaderServices.TagReading;

public class TagReaderOptions
{
  public TagReaderMode TagReaderMode { get; internal set; }

  public byte Antennas { get; internal set; } = 0x00;

  public static TagReaderOptions Create(TagReaderMode mode = TagReaderMode.HostMode)
  {
    var options = new TagReaderOptions();

    options.TagReaderMode = mode;

    return options;
  }
}
