namespace ElectroCom.RFIDTools.ReaderServices.TagReading;

using ElectroCom.RFIDTools.ReaderServices.Model;

public class TagReadDataReport
{
  private List<TagEntry> tagEntries = new List<TagEntry>();
  
  
  public IReadOnlyCollection<TagEntry> Tags => this.tagEntries.AsReadOnly();

}
