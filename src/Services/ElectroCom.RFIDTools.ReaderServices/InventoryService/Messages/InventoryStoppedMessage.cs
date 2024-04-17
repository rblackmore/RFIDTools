namespace ElectroCom.RFIDTools.ReaderServices.InventoryService.Events;

using MediatR;

public class InventoryStoppedMessage : INotification
{
  // TODO: Might be worth adding a <see="Type"> property
  // to detail what object stopped the Loop.

  public InventoryStoppedMessage(string information)
  {
    Information = information;
  }

  public string Information { get; init; }
}
