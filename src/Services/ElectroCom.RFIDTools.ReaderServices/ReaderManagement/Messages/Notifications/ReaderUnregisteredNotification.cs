﻿namespace ElectroCom.RFIDTools.ReaderServices;

using MediatR;

public class ReaderUnregistered : INotification
{
  private readonly ReaderDefinition readerDefinition;

  public ReaderUnregistered(ReaderDefinition readerDefinition)
  {
    ArgumentNullException.ThrowIfNull(readerDefinition);
    this.readerDefinition = readerDefinition;
  }

  public uint DeviceID => this.readerDefinition.DeviceID;
  public string DeviceName => this.readerDefinition.DeviceName;
  public bool IsConnected => this.readerDefinition.IsConnected;
}
