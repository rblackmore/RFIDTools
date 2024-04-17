namespace ElectroCom.RFIDTools.UI.Logic.Model;

using FEDM;

public class EPCClass1Gen2_TagEntry : TagEntry
{
  private uint protocolControl = 0;
  private string ePCHex = string.Empty;
  private string tIDHex = string.Empty;
  private int tagModelNumber = 0;
  private string tagDesignerName = string.Empty;

  public uint ProtocolControl
  {
    get => protocolControl;
    private set => SetProperty(ref protocolControl, value);
  }
  public string EPCHex
  {
    get => ePCHex;
    private set => SetProperty(ref ePCHex, value);
  }
  public string TIDHex
  {
    get => tIDHex;
    private set => SetProperty(ref tIDHex, value);
  }
  public int TagModelNumber
  {
    get => tagModelNumber;
    private set => SetProperty(ref tagModelNumber, value);
  }
  public string TagDesignerName
  {
    get => tagDesignerName;
    private set => SetProperty(ref tagDesignerName, value);
  }

  public static TagEntry CreateFrom(TagItem tagItem)
  {
    var tagEntry = new EPCClass1Gen2_TagEntry();

    var pc = tagItem.epcC1G2_Pc();
    var epchex = tagItem.epcC1G2_EpcToHexString();

    var tidhex = string.Empty;
    var tagModelNumber = 0;
    var tagDesignerName = string.Empty;

    if (tagItem.epcC1G2_IsEpcAndTid())
    {
      tidhex = tagItem.epcC1G2_TidToHexString();
      tagModelNumber = tagItem.epcC1G2_TagModelNumber();
      tagDesignerName = tagItem.epcC1G2_MaskDesignerName();
    }

    tagEntry.ProtocolControl = pc;
    tagEntry.EPCHex = epchex;
    tagEntry.TIDHex = tidhex;
    tagEntry.TagModelNumber = tagModelNumber;
    tagEntry.TagDesignerName = tagDesignerName;

    return tagEntry;
  }
}
