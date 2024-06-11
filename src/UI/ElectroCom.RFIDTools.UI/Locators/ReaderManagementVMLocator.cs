namespace ElectroCom.RFIDTools.UI.Locator;

using ElectroCom.RFIDTools.UI.Locator;
using ElectroCom.RFIDTools.UI.Logic.ViewModels;

public class ReaderManagementVMLocator : ViewModelLocatorBase<IReaderManagementVM>
{
  public ReaderManagementVMLocator()
  {
    this.DesignTimeViewModel = new DesignTimeReaderManagerVM();
  }
}
