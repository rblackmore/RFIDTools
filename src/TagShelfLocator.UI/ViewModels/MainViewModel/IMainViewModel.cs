namespace TagShelfLocator.UI.ViewModels;

using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.Input;

using TagShelfLocator.UI.Model;

public interface IMainViewModel
{
  public bool IsReaderConnected { get; }
  public bool IsReaderDisconnected { get; }
  public ObservableCollection<ObservableTagDetails> TagList { get; }
  public IAsyncRelayCommand StartInventoryAsync { get; }
  public IAsyncRelayCommand StopInventoryAsync { get; }
}
