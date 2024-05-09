namespace ElectroCom.RFIDTools.ReaderServices;

using ElectroCom.RFIDTools.ReaderServices.Model;

public class TagReaderDataReport
{
  private readonly List<TagEntry> tagEntries = [];
  private readonly string message = string.Empty;

  public TagReaderDataReport(List<TagEntry> tagEntries)
  {
    this.tagEntries = tagEntries;
  }
  public TagReaderDataReport(string message)
  {
    this.message = message;
  }

  public TagReaderDataReport(List<TagEntry> tagEntries, string message)
  {
    this.tagEntries = tagEntries;
    this.message = message;
  }

  public IReadOnlyCollection<TagEntry> Tags => this.tagEntries.AsReadOnly();

  public string Message => this.message;
  public bool HasMessage => !string.IsNullOrEmpty(this.Message);
  public bool HasData => this.tagEntries is not null && this.tagEntries.Count != 0;
}
