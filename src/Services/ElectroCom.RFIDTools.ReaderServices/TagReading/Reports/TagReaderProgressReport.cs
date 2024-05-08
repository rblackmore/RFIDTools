namespace ElectroCom.RFIDTools.ReaderServices;

public class TagReaderProgressReport
{

  public TagReaderProgressReport(
    string message,
    bool isComplete = false,
    bool isFaulted = false,
    bool isCancelled = false)
  {
    Message = message;
    IsComplete = isComplete;
    IsFaulted = isFaulted;
    IsCancelled = isCancelled;
  }

  public string Message { get; private set; }
  public bool IsCancelled { get; private set; }
  public bool IsFaulted { get; private set; }
  public bool IsComplete { get; private set; }
}
