namespace ElectroCom.RFIDTools.ReaderServices.TagReaders.InventoryMode;
public class InventoryTagReaderOptions
{
  public static InventoryTagReaderOptions Default = new();

  public byte Antennas { get; set; } = 0x00;
}
