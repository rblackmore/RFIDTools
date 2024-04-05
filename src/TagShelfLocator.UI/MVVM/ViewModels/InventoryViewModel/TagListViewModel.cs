namespace TagShelfLocator.UI.MVVM.ViewModels;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Threading;

using CommunityToolkit.Mvvm.Messaging;

using TagShelfLocator.UI.Helpers;
using TagShelfLocator.UI.MVVM.Modal;
using TagShelfLocator.UI.Services.InventoryService.Messages;

public class TagListViewModel : ViewModel,
  IDisposable,
    IRecipient<InventoryTagItemsDetectedMessage>
{
  private readonly IMessenger messenger;

  public TagListViewModel(IMessenger messenger)
  {
    this.messenger = messenger;

    this.messenger.RegisterAll(this);

    this.TagList = new();
  }

  public ObservableCollection<TagEntry> TagList { get; set; }


  public void Receive(InventoryTagItemsDetectedMessage message)
  {
    DispatcherHelper.CheckBeginInvokeOnUI(() =>
    {
      foreach (var item in message.Tags)
      {
        var existingItem = this.TagList.FirstOrDefault(te => te.SerialNumber ==  item.SerialNumber);
        if (existingItem is null)
          this.TagList.Add(item);
        else
          existingItem.AddRead();
      }
    });
  }

  public void Dispose()
  {
    this.messenger.UnregisterAll(this);
  }
}
