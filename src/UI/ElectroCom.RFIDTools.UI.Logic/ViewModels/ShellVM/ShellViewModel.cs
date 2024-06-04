namespace ElectroCom.RFIDTools.UI.Logic.ViewModels;

using System;

using CommunityToolkit.Mvvm.Input;

public class ShellViewModel : ViewModel, IShellViewModel
{
  private INavigationService? navigationService;

  public ShellViewModel(INavigationService navigationService)
  {
    NavigationService = navigationService;
    NavigateToInventory = new RelayCommand(NavigateToInventoryExecute);
    NavigateToSettings = new RelayCommand(NavigateToSettingsExecute);
    NavigateToReaderManagement = new RelayCommand(NavigateToReaderManagementExecute);
  }



  public INavigationService? NavigationService
  {
    get { return navigationService; }
    private set { SetProperty(ref navigationService, value); }
  }

  public IRelayCommand NavigateToInventory { get; private set; }

  public IRelayCommand NavigateToSettings { get; private set; }

  public IRelayCommand NavigateToReaderManagement { get; private set; }

  private void NavigateToInventoryExecute()
  {
    NavigationService?.NavigateTo<IInventoryViewModel>();
  }

  private void NavigateToSettingsExecute()
  {
    NavigationService?.NavigateTo<ISettingsViewModel>();
  }

  private void NavigateToReaderManagementExecute()
  {
    this.NavigationService?.NavigateTo<IReaderManagementVM>();
  }
}
