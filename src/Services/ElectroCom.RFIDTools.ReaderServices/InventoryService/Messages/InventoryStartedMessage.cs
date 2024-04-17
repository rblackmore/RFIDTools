namespace ElectroCom.RFIDTools.ReaderServices.InventoryService.Events;

using MediatR;

public class InventoryStartedMessage : INotification
{
  // TODO: Might be worth adding a <see="Type"> property
  // to detail what object started the Loop.

  public InventoryStartedMessage(string information)
  {
    Information = information;
  }

  public string Information { get; init; }

}
