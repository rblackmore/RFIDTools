namespace ElectroCom.RFIDTools.ReaderServices;

public enum TagReaderProcessState
{
  Started = 0,
  Running = 1,
  Complete = 2,
  Canceled = 3,
  Faulted = 4,
}

public class TagReaderProcessStatusUpdate
{
  private readonly string message = string.Empty;
  private readonly TagReaderProcessState state;

  public TagReaderProcessStatusUpdate(
    string message, TagReaderProcessState state)
  {
    this.message = message;
    this.state = state;
  }

  public TagReaderProcessState State => this.state;
  public string Message => this.message;
  public bool HasMessage => !string.IsNullOrEmpty(this.message);

  public static TagReaderProcessStatusUpdate Started()
  {
    return new TagReaderProcessStatusUpdate("Tag Reading Started", TagReaderProcessState.Started);
  }
}
