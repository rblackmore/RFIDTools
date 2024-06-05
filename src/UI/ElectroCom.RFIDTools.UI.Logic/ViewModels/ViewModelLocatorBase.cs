namespace ElectroCom.RFIDTools.UI.Logic.ViewModels;

using System.ComponentModel;
using System.Windows;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.Extensions.DependencyInjection;

public abstract class ViewModelLocatorBase<TViewModel>
  : ObservableObject where TViewModel : IViewModel
{
  private static bool? isInDesignMode;
  private TViewModel? runtimeViewModel;
  private TViewModel? designTimeViewModel;

  private Func<Type, IViewModel>? viewModelFactory;

  protected ViewModelLocatorBase()
  {
    this.viewModelFactory = ServiceLocator.ServiceProvider?.GetRequiredService<Func<Type, IViewModel>>();

  }

  protected TViewModel RuntimeViewModel
  {
    get
    {
      if (runtimeViewModel is null)
      {
        this.viewModelFactory = ServiceLocator.ServiceProvider.GetRequiredService<Func<Type, IViewModel>>();
        this.runtimeViewModel = (TViewModel)viewModelFactory(typeof(TViewModel));
      }

      return runtimeViewModel!;
    }
  }

  public TViewModel DesignTimeViewModel
  {
    get => designTimeViewModel!;
    protected set => SetProperty(ref designTimeViewModel, value);
  }

  public TViewModel ViewModel =>
    IsInDesignMode ? DesignTimeViewModel : RuntimeViewModel;

  private static bool IsInDesignMode
  {
    get
    {
      if (!isInDesignMode.HasValue)
      {
        isInDesignMode = DesignerProperties.GetIsInDesignMode(new DependencyObject());
      }
      return isInDesignMode.Value;
    }
  }
}
