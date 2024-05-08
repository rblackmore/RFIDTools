namespace ElectroCom.RFIDTools.ReaderServices;

using ElectroCom.RFIDTools.ReaderServices.Model;

public class TagReadDataReport
{
  private List<TagEntry> tagEntries = [];
  private string message = string.Empty;

  public TagReadDataReport(List<TagEntry> tagEntries)
  {
    this.tagEntries = tagEntries;
  }
  public TagReadDataReport(string message)
  {
    this.message = message;
  }

  public TagReadDataReport(List<TagEntry> tagEntries, string message)
  {
    this.tagEntries = tagEntries;
    this.message = message;
  }

  public IReadOnlyCollection<TagEntry> Tags => this.tagEntries.AsReadOnly();

  public string Message => this.message;
}
