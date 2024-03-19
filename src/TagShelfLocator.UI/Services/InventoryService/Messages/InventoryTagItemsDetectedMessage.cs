namespace TagShelfLocator.UI.Services.InventoryService.Messages;

using System.Collections.Generic;

using TagShelfLocator.UI.MVVM.Modal;

public class InventoryTagItemsDetectedMessage
{
  public List<TagEntry> Tags { get; }

  public InventoryTagItemsDetectedMessage(List<TagEntry> tags)
  {
    this.Tags = tags;
  }
}

