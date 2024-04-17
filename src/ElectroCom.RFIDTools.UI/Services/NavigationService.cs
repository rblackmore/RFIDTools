namespace TagShelfLocator.UI.Services;

using System;

using CommunityToolkit.Mvvm.ComponentModel;

using TagShelfLocator.UI.MVVM.ViewModels;

public interface INavigationService
{
  IViewModel CurrentViewModel { get; }

  void NavigateTo<T>() where T : IViewModel;
}

public class NavigationService : ObservableObject, INavigationService
{
  private IViewModel currentViewModel;
  private Func<Type, IViewModel> viewModelFactory;

  public NavigationService(Func<Type, IViewModel> viewModelFactory)
  {
    this.viewModelFactory = viewModelFactory;
  }

  public IViewModel CurrentViewModel
  {
    get { return this.currentViewModel; }
    set
    {
      SetProperty(ref currentViewModel, value);
    }
  }

  public void NavigateTo<TViewModel>() where TViewModel : IViewModel
  {
    this.CurrentViewModel = this.viewModelFactory.Invoke(typeof(TViewModel));
  }
}
