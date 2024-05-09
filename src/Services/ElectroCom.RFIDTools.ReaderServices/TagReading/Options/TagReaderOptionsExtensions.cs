namespace ElectroCom.RFIDTools.ReaderServices;

public static class TagReaderOptionsExtensions
{
  public static TagReaderOptions UseAntennas(
    this TagReaderOptions options,
    byte Antennas)
  {
    //Validation Here.
    options.Antennas = Antennas;
    return options;
  }

  public static TagReaderOptions UseReaderMode(
    this TagReaderOptions options,
    TagReaderMode mode)
  {
    // Validation Here.
    options.TagReaderMode = mode;
    return options;
  }
}
