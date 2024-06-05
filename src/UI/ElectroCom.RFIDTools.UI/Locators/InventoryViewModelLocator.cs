namespace ElectroCom.RFIDTools.UI.Locator;

using ElectroCom.RFIDTools.UI.Logic.ViewModels;

public class InventoryViewModelLocator : ViewModelLocatorBase<IInventoryViewModel>
{
  public InventoryViewModelLocator()
  {
    DesignTimeViewModel = new DesignInventoryVM();
  }
}
