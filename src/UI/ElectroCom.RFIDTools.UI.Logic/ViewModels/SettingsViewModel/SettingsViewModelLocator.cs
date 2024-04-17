namespace ElectroCom.RFIDTools.UI.Logic.ViewModels;
public class SettingsViewModelLocator : ViewModelLocatorBase<ISettingsViewModel>
{
  public SettingsViewModelLocator(Func<Type, IViewModel> viewModelFactory)
    : base(viewModelFactory)
  {
    DesignTimeViewModel = new DummySettingsViewModel();
  }
}
