namespace ElectroCom.RFIDTools.UI.Logic.ViewModels;

using CommunityToolkit.Mvvm.Input;

using ElectroCom.RFIDTools.UI.Logic.Services;

public interface IShellViewModel : IViewModel
{
  INavigationService? NavigationService { get; }
  IRelayCommand NavigateToInventory { get; }
  IRelayCommand NavigateToSettings { get; }
}
