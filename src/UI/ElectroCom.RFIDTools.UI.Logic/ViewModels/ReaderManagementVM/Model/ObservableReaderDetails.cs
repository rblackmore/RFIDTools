namespace ElectroCom.RFIDTools.UI.Logic.ViewModels;

using System.Diagnostics.CodeAnalysis;

using CommunityToolkit.Mvvm.ComponentModel;

using ElectroCom.RFIDTools.ReaderServices;

/// <summary>
/// Observable Object for displaying Reader Information.
/// Likely used in list of available readers, as well as settings to create a new reader.
/// </summary>
public class ObservableReaderDetails : ObservableObject, IEquatable<ObservableReaderDetails>
{
  private ReaderDefinition readerDefinition;

  public ObservableReaderDetails(ReaderDefinition readerDefinition)
  {
    this.readerDefinition = readerDefinition;
  }

  public uint DeviceID
  {
    get => this.readerDefinition.DeviceID;
  }

  public string DeviceName
  {
    get => this.readerDefinition.DeviceName;
  }

  public bool Equals(ObservableReaderDetails? other)
  {
    if (other is null)
      return false;

    if (ReferenceEquals(this, other))
      return true;

    if (other.DeviceID == this.DeviceID)
      return true;

    return false;
  }
}
