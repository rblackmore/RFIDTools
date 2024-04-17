namespace ElectroCom.RFIDTools.UI.Logic.Model;
using FEDM;

public class ISO14443A_TagEntry : TagEntry
{
  private string manufacturerName = string.Empty;

  public string ManufacturerName
  {
    get => manufacturerName;
    private set => SetProperty(ref manufacturerName, value);
  }

  public static TagEntry CreateFrom(TagItem tagItem)
  {
    var tagEntry = new ISO14443A_TagEntry();

    var manufacturerName = tagItem.manufacturerName();

    tagEntry.ManufacturerName = manufacturerName;

    return tagEntry;
  }
}
