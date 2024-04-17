namespace ElectroCom.RFIDTools.ReaderServices.InventoryService.Messages;

using System.Collections.Generic;

using ElectroCom.RFIDTools.ReaderServices.Model;

using MediatR;

public class InventoryTagItemsDetectedMessage : INotification
{
  public List<TagEntry> Tags { get; }

  public InventoryTagItemsDetectedMessage(List<TagEntry> tags)
  {
    this.Tags = tags;
  }
}

