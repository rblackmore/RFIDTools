namespace TagShelfLocator.UI.ViewModels;

using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.Input;

using TagShelfLocator.UI.Core.Model;

public interface IInventoryViewModel : IViewModel
{
  public bool IsReaderConnected { get; }
  public bool IsReaderDisconnected { get; }
  public ObservableCollection<EPCTagEntry> TagList { get; }
  public IAsyncRelayCommand StartInventoryAsync { get; }
  public IAsyncRelayCommand StopInventoryAsync { get; }
  public IRelayCommand OpenSettings { get; }
}
