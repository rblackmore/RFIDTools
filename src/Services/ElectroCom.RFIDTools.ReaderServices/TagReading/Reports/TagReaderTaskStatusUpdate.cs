namespace ElectroCom.RFIDTools.ReaderServices;

public enum TagReaderProcessState
{
  Running = 0,
  Complete = 1,
  Canceled = 2,
  Faulted = 3,
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
}
