namespace TagShelfLocator.UI.ViewModels;

using CommunityToolkit.Mvvm.Input;

using TagShelfLocator.UI.Services;

public class ShellViewModel : ViewModel, IShellViewModel
{
  private INavigationService? navigationService;

  public ShellViewModel(INavigationService navigationService)
  {
    this.NavigationService = navigationService;
    this.NavigateToInventory = new RelayCommand(NavigateToInventoryExecute);
    this.NavigateToSettings = new RelayCommand(NavigateToSettingsExecute);
  }

  public INavigationService? NavigationService
  {
    get { return navigationService; }
    private set { SetProperty(ref this.navigationService, value); }
  }

  public IRelayCommand NavigateToInventory { get; set; }

  public IRelayCommand NavigateToSettings { get; set; }

  private void NavigateToInventoryExecute()
  {
    this.NavigationService?.NavigateTo<IInventoryViewModel>();
  }

  private void NavigateToSettingsExecute()
  {
    this.NavigationService?.NavigateTo<ISettingsViewModel>();
  }
}
