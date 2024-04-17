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
  }

  public string TagType { get; private set; }
  public string SerialNumber { get; private set; }
  public IReadOnlyCollection<Antenna> Antennas => antennas.AsReadOnly();

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

  internal TagItem TagItem => tagItem;

}
