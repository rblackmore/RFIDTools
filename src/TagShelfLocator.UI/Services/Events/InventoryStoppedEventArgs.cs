namespace TagShelfLocator.UI.Services.Events;
using System;

public delegate void InventoryStoppedHandler(object sender, InventoryStoppedEventArgs e);

public class InventoryStoppedEventArgs : EventArgs
{
  public InventoryStoppedEventArgs(string reasonforstopping)
  {
    this.Reasonforstopping = reasonforstopping;
  }

  public string Reasonforstopping { get; }
}
