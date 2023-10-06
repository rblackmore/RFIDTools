namespace TagShelfLocator.UI.ViewModels;

using System.ComponentModel;
using System.Windows;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.Extensions.DependencyInjection;

public abstract class ViewModelLocatorBase<TViewModel>
  : ObservableObject where TViewModel : IViewModel
{
  private static DependencyObject dummy = new();

  private static bool? isInDesignMode;
  private TViewModel? runtimeViewModel;
  private TViewModel? designTimeViewModel;

  public static bool IsInDesignMode
  {
    get
    {
      if (!isInDesignMode.HasValue)
        isInDesignMode = DesignerProperties.GetIsInDesignMode(dummy);

      return isInDesignMode.Value;
    }
  }

  protected TViewModel RuntimeViewModel
  {
    get
    {
      if (this.runtimeViewModel is null)
        this.RuntimeViewModel = App.Current.Services.GetRequiredService<TViewModel>();

      return this.runtimeViewModel!;
    }
    set => SetProperty(ref this.runtimeViewModel, value);
  }

  public TViewModel DesignTimeViewModel
  {
    get => this.designTimeViewModel!;
    set => SetProperty(ref this.designTimeViewModel, value);
  }

  public TViewModel ViewModel =>
    IsInDesignMode ? this.DesignTimeViewModel : this.RuntimeViewModel;
}
