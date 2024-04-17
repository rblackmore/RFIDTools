namespace TagShelfLocator.UI.MVVM.Modal;

using FEDM;

public class ISO15693_TagEntry : TagEntry
{
  private int afi;

  public int Afi
  {
    get => afi;
    private set => SetProperty(ref afi, value);
  }

  public static TagEntry CreateFrom(TagItem tagItem)
  {
    var tagEntry = new ISO15693_TagEntry();

    var afi = tagItem.iso15693_Afi();

    tagEntry.Afi = afi;

    return tagEntry;
  }
}
