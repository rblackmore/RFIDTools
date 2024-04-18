namespace ElectroCom.RFIDTools.UI.Logic.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;

using ElectroCom.RFIDTools.UI.Logic.Helpers;

using Microsoft.Extensions.DependencyInjection;

public abstract class ViewModelLocatorBase<TViewModel>
  : ObservableObject where TViewModel : IViewModel
{
  private TViewModel? runtimeViewModel;
  private TViewModel? designTimeViewModel;

  private Func<Type, IViewModel> viewModelFactory;

  protected ViewModelLocatorBase()
  {
    this.viewModelFactory = ServiceLocator.ServiceProvider.GetRequiredService<Func<Type, IViewModel>>();
  }

  protected TViewModel RuntimeViewModel
  {
    get
    {
      if (runtimeViewModel is null)
        this.runtimeViewModel = (TViewModel)viewModelFactory(typeof(TViewModel));

      return runtimeViewModel!;
    }
    //set => SetProperty(ref runtimeViewModel, value);
  }

  public TViewModel DesignTimeViewModel
  {
    get => designTimeViewModel!;
    protected set => SetProperty(ref designTimeViewModel, value);
  }

  public TViewModel ViewModel =>
    this.viewModelFactory is null ? DesignTimeViewModel : RuntimeViewModel;
}
