namespace TagShelfLocator.UI.ViewModels;

using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.Input;

using TagShelfLocator.UI.Services;

public interface IMainViewModel
{
  public bool IsReaderConnected { get; }
  public bool IsReaderDisconnected { get; }
  public ObservableCollection<TagReadViewModel> TagList { get; }
  public IAsyncRelayCommand StartInventoryAsync { get; }
  public IAsyncRelayCommand StopInventoryAsync { get; }
}
