namespace ElectroCom.RFIDTools.ReaderServices;

using ElectroCom.RFIDTools.ReaderServices.Model;

public class TagReadDataReport
{
  private List<TagEntry> tagEntries = new List<TagEntry>();
  
  
  public IReadOnlyCollection<TagEntry> Tags => this.tagEntries.AsReadOnly();

}
