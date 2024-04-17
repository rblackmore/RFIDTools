namespace ElectroCom.RFIDTools.UI.Logic.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;

public abstract class ViewModelLocatorBase<TViewModel>
  : ObservableObject where TViewModel : IViewModel
{
  private TViewModel? runtimeViewModel;
  private TViewModel? designTimeViewModel;

  private Func<Type, IViewModel> viewModelFactory;

  protected ViewModelLocatorBase(Func<Type, IViewModel> viewModelFactory)
  {
    this.viewModelFactory = viewModelFactory;
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
