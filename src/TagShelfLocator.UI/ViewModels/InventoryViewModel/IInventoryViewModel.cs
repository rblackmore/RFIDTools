namespace TagShelfLocator.UI.ViewModels;

using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.Input;

using TagShelfLocator.UI.Core.Model;

public interface IInventoryViewModel : IViewModel
{
  public bool IsReaderConnected { get; }
  public bool IsReaderDisconnected { get; }
  public ObservableCollection<TagEntry> TagList { get; }
  public IAsyncRelayCommand StartInventoryAsync { get; }
  public IAsyncRelayCommand StopInventoryAsync { get; }
  public IRelayCommand OpenSettings { get; }

  public IRelayCommand AddTagEntry { get; }
}
