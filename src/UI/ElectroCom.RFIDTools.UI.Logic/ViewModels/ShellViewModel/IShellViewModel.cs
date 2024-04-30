namespace ElectroCom.RFIDTools.UI.Logic.ViewModels;

using CommunityToolkit.Mvvm.Input;

public interface IShellViewModel : IViewModel
{
  INavigationService? NavigationService { get; }
  IRelayCommand NavigateToInventory { get; }
  IRelayCommand NavigateToSettings { get; }
}
