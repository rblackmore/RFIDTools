namespace ElectroCom.RFIDTools.ReaderServices.Model;

using ElectroCom.RFIDTools.ReaderServices.Model.Tag;

using FEDM;

public class TagEntry : ValueObject
{
  private TagItem tagItem;
  private List<Antenna> antennas;

  internal TagEntry(TagItem tagItem)
  {
    this.tagItem = tagItem;

    TagType = TransponderType.toString(tagItem.trType());
    SerialNumber = tagItem.iddToHexString();

    this.antennas = GetAntennas(tagItem);

    if (tagItem.isEpcClass1Gen2())
      SetEPCClass1Gen2(tagItem);

    if (tagItem.isIso15693())
      SetISO15693(tagItem);

    if (tagItem.isIso14443A())
      SetISO14443A(tagItem);
  }

  // Tag Details
  public string TagType { get; private set; }
  public string SerialNumber { get; private set; }
  public IReadOnlyCollection<Antenna> Antennas => antennas.AsReadOnly();

  // EPC Class 1 Gen 2
  private void SetEPCClass1Gen2(TagItem tagItem)
  {
    ProtocolControl = tagItem.epcC1G2_Pc();
    EPCHex = tagItem.epcC1G2_EpcToHexString();

    if (tagItem.epcC1G2_IsEpcAndTid())
    {
      TIDHex = tagItem.epcC1G2_TidToHexString();
      ModelNumber = tagItem.epcC1G2_TagModelNumber();
      DesignerName = tagItem.epcC1G2_MaskDesignerName();
    }
  }

  public ushort ProtocolControl { get; private set; } = 0;
  public string EPCHex { get; private set; } = string.Empty;
  public string TIDHex { get; private set; } = string.Empty;
  public int ModelNumber { get; private set; } = 0;
  public string DesignerName { get; private set; } = string.Empty;

  // ISO15693
  private void SetISO15693(TagItem tagItem)
  {
    Afi = tagItem.iso15693_Afi();
    ManufacturerName = tagItem.manufacturerName();
  }

  public int Afi { get; private set; }

  // ISO14443-A
  private void SetISO14443A(TagItem tagItem)
  {
    ManufacturerName = tagItem.manufacturerName();
  }

  public string ManufacturerName { get; private set; } = string.Empty;

  private List<Antenna> GetAntennas(TagItem tagItem)
  {
    List<Antenna> ants = new List<Antenna>();

    var rssiValues = tagItem.rssiValues();

    if (rssiValues is null)
      return ants;

    foreach (var rssi in rssiValues)
    {
      ants.Add(new Antenna
      {
        AntennaNo = rssi.antennaNumber(),
        RSSI = rssi.rssi()
      });
    }

    return ants;
  }

  protected override IEnumerable<object> GetEqualityComponents()
  {
    yield return this.SerialNumber;
  }
  // Maybe use Specification Pattern Here, and pass in an ISpecification object.
  // This way I can make any number of specification implementations
  // To get data from a Tag item, these implementations exist within this project.
  // Consumers can just new up the one they want, and pass it in here.
  // OR... I could just add more properties to this class 🤷‍
  public T GetTagItemData<T>(Func<TagItem, T> factory)
  {
    return factory(this.tagItem);
  }

  internal TagItem TagItem => tagItem;
}
