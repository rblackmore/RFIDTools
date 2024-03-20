﻿namespace TagShelfLocator.UI.MVVM.ViewModels;

using CommunityToolkit.Mvvm.Input;

using TagShelfLocator.UI.Services;

public interface IShellViewModel : IViewModel
{
  INavigationService? NavigationService { get; }
  IRelayCommand NavigateToInventory { get; }
  IRelayCommand NavigateToSettings { get; }
}