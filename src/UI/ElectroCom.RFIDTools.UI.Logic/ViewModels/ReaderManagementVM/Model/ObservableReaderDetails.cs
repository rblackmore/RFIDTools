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
  private uint deviceId;
  private string deviceName = String.Empty;

  public ObservableReaderDetails(uint deviceId, string deviceName)
  {
    this.deviceId = deviceId;
    this.deviceName = deviceName;
  }

  public ObservableReaderDetails(ReaderDefinition readerDefinition)
  {
    this.DeviceID = readerDefinition.DeviceID;
    this.DeviceName = readerDefinition.DeviceName;
  }

  public uint DeviceID
  {
    get => this.deviceId;
    set => SetProperty(ref this.deviceId, value);
  }

  public string DeviceName
  {
    get => this.deviceName;
    set => SetProperty(ref this.deviceName, value);
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
