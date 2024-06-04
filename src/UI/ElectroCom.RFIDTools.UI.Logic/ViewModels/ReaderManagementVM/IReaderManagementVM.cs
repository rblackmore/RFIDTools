namespace ElectroCom.RFIDTools.UI.Logic.ViewModels;

public interface IReaderManagementVM : IViewModel
{
  public ObservableReaderDetailsCollection Readers { get; }
}
