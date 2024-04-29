﻿namespace ElectroCom.RFIDTools.ReaderServices;

using MediatR;

public class ReaderDisconnected : INotification
{
  public ReaderDisconnected(ReaderDefinition readerDefinition, bool isSelectedReader)
  {
    ReaderDefinition = readerDefinition;
    IsSelectedReader = isSelectedReader;
  }

  public uint DeviceID => this.ReaderDefinition.DeviceID;
  public string DeviceName => this.ReaderDefinition.DeviceName;
  public bool IsSelectedReader { get; set; }

  public ReaderDefinition ReaderDefinition { get; private set; }
}
