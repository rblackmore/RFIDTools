namespace ElectroCom.RFIDTools.UI.Logic.ViewModels;

public class DesignTimeReaderManagerVM : ViewModel, IReaderManagementVM
{
  public DesignTimeReaderManagerVM()
  {
    this.Readers = new();
    this.Readers.Add(new ObservableReaderDetails(123456, "LRU1002"));
    this.Readers.Add(new ObservableReaderDetails(653123, "CPR70"));
    this.Readers.Add(new ObservableReaderDetails(453973, "LRU1002"));
    this.Readers.Add(new ObservableReaderDetails(456789, "LRU1002"));
  }
  public ObservableReaderDetailsCollection Readers { get; private set; }
}
