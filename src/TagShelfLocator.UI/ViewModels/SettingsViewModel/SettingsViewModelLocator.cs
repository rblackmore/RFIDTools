namespace TagShelfLocator.UI.ViewModels;

public class SettingsViewModelLocator : ViewModelLocatorBase<ISettingsViewModel>
{
  public SettingsViewModelLocator()
  {
    this.DesignTimeViewModel = new DummySettingsViewModel();
  }
}
