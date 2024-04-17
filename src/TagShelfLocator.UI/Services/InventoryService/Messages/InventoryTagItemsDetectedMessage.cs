namespace TagShelfLocator.UI.Services.InventoryService.Messages;

using System.Collections.Generic;

using MediatR;

using TagShelfLocator.UI.MVVM.Modal;

public class InventoryTagItemsDetectedMessage : INotification
{
  public List<TagEntry> Tags { get; }

  public InventoryTagItemsDetectedMessage(List<TagEntry> tags)
  {
    this.Tags = tags;
  }
}

