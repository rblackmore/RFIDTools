namespace ElectroCom.RFIDTools.UI.Logic.ViewModels;
public class InventoryViewModelLocator : ViewModelLocatorBase<IInventoryViewModel>
{
  public InventoryViewModelLocator()
  {
    DesignTimeViewModel = new DummyInventoryViewModel();
  }
}
