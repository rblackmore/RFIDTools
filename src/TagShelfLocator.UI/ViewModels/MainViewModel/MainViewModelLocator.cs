namespace TagShelfLocator.UI.ViewModels;

public class MainViewModelLocator : ViewModelLocatorBase<IMainViewModel>
{
  public MainViewModelLocator()
  {
    this.DesignTimeViewModel = new DummyMainViewModel();
  }
}
