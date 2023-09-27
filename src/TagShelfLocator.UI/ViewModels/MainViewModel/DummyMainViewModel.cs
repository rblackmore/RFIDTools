namespace TagShelfLocator.UI.ViewModels;

using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using TagShelfLocator.UI.Services;

public class DummyMainViewModel : ObservableObject, IMainViewModel
{
  public ObservableCollection<TagReadViewModel> TagList => throw new System.NotImplementedException();

  public IAsyncRelayCommand StartInventoryAsync => throw new System.NotImplementedException();

  public IAsyncRelayCommand StopInventoryAsync => throw new System.NotImplementedException();

  public bool IsReaderConnected => true;

  public bool IsReaderDisconnected => false;
}
