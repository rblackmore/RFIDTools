namespace TagShelfLocator.UI.MVVM.ViewModels;

using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.Input;

using TagShelfLocator.UI.MVVM.Modal;

public class DummyInventoryViewModel : ViewModel, IInventoryViewModel
{
  public ObservableCollection<TagEntry> TagList
  {
    get
    {
      return new ObservableCollection<TagEntry>(
        new[] {
          new TagEntry{
            Number = 1,
            TagType =  "EPC Class 1 Gen 2",
            SerialNumber =  "1234" },

          new TagEntry{
            Number = 2,
            TagType =  "ISO14443-A Mifare DESFire",
            SerialNumber =  "ABCD" },

          new TagEntry{
            Number = 3,
            TagType =  "EPC Class 1 Gen 2",
            SerialNumber =  "4321" },

          new TagEntry{
            Number = 4,
            TagType =  "EPC Class 1 Gen 2",
            SerialNumber =  "DEF1" },
        });
    }
  }
  public bool ClearOnStart { get; set; } = true;

  public IRelayCommand ClearTagList => throw new System.NotImplementedException();

  public IAsyncRelayCommand StartInventoryAsync => throw new System.NotImplementedException();

  public IAsyncRelayCommand StopInventoryAsync => throw new System.NotImplementedException();

  public IRelayCommand OpenSettings => throw new System.NotImplementedException();

  public bool IsReaderConnected => true;

  public bool IsReaderDisconnected => false;

  public IRelayCommand AddTagEntry => throw new System.NotImplementedException();

}
