namespace TagShelfLocator.UI.ViewModels;

public class InventoryViewModelLocator : ViewModelLocatorBase<IInventoryViewModel>
{
  public InventoryViewModelLocator()
  {
    this.DesignTimeViewModel = new DummyInventoryViewModel();
  }
}
