namespace ElectroCom.RFIDTools.ReaderServices;

using MediatR;

public class SelectedReaderChanged : INotification
{
  private readonly ReaderDefinition? readerDefinition;
  public SelectedReaderChanged(ReaderDefinition? readerDefinition)
  {
    if (readerDefinition is null)
      Unselected = true;

    this.readerDefinition = readerDefinition;
  }

  public uint DeviceName => this.readerDefinition?.DeviceID ?? 0;
  public string ReaderName => this.readerDefinition?.DeviceName ?? String.Empty;
  public bool Unselected { get; private set; } = false;
}
