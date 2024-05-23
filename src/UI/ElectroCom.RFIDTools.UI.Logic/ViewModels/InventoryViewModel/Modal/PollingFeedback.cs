namespace ElectroCom.RFIDTools.UI.Logic.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;

public class PollingFeedback : ObservableObject
{
  private DateTime timeStamp;
  private string message;

  public PollingFeedback(string message)
  {
    this.TimeStamp = DateTime.Now;
    this.Message = message;
  }

  public DateTime TimeStamp
  {
    get { return timeStamp; }
    set { SetProperty(ref this.timeStamp, value); }
  }

  public string Message
  {
    get { return message; }
    set { SetProperty(ref this.message, value); }
  }
}
