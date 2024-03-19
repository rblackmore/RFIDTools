namespace TagShelfLocator.UI.MVVM.ViewModels;
public class SettingsViewModelLocator : ViewModelLocatorBase<ISettingsViewModel>
{
  public SettingsViewModelLocator()
  {
    DesignTimeViewModel = new DummySettingsViewModel();
  }
}
