namespace TagShelfLocator.UI.MVVM.ViewModels;
public class InventoryViewModelLocator : ViewModelLocatorBase<IInventoryViewModel>
{
  public InventoryViewModelLocator()
  {
    DesignTimeViewModel = new DummyInventoryViewModel();
  }
}
