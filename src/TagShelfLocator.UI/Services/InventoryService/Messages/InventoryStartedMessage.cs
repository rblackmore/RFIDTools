namespace TagShelfLocator.UI.Services.InventoryService.Events;
public class InventoryStartedMessage
{
  // TODO: Might be worth adding a <see="Type"> property
  // to detail what object started the Loop.
  
  public InventoryStartedMessage(string information)
  {
    Information = information;
  }

  public string Information { get; init; }

}
