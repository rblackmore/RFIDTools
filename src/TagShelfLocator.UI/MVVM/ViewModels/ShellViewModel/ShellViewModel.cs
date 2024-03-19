namespace TagShelfLocator.UI.MVVM.ViewModels;

using CommunityToolkit.Mvvm.Input;

using TagShelfLocator.UI.Services;

public class ShellViewModel : ViewModel, IShellViewModel
{
  private INavigationService? navigationService;

  public ShellViewModel(INavigationService navigationService)
  {
    NavigationService = navigationService;
    NavigateToInventory = new RelayCommand(NavigateToInventoryExecute);
    NavigateToSettings = new RelayCommand(NavigateToSettingsExecute);
  }

  public INavigationService? NavigationService
  {
    get { return navigationService; }
    private set { SetProperty(ref navigationService, value); }
  }

  public IRelayCommand NavigateToInventory { get; set; }

  public IRelayCommand NavigateToSettings { get; set; }

  private void NavigateToInventoryExecute()
  {
    NavigationService?.NavigateTo<IInventoryViewModel>();
  }

  private void NavigateToSettingsExecute()
  {
    NavigationService?.NavigateTo<ISettingsViewModel>();
  }
}
