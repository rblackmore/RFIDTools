namespace ElectroCom.RFIDTools.UI.Logic.ViewModels;
public class SettingsViewModelLocator : ViewModelLocatorBase<ISettingsViewModel>
{
  public SettingsViewModelLocator()
  {
    DesignTimeViewModel = new DesignSettingsVM();
  }
}
