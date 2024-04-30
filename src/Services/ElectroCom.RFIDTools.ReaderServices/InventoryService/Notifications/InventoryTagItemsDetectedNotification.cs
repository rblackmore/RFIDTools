namespace ElectroCom.RFIDTools.ReaderServices.InventoryService;

using System.Collections.Generic;

using ElectroCom.RFIDTools.ReaderServices.Model;

using MediatR;

public class InventoryTagItemsDetectedNotification : INotification
{
  public IReadOnlyList<TagEntry> Tags { get; }

  public InventoryTagItemsDetectedNotification(List<TagEntry> tags)
  {
    this.Tags = tags.AsReadOnly();
  }
}

