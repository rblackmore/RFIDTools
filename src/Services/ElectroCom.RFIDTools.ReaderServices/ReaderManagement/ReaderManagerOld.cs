namespace ElectroCom.RFIDTools.ReaderServices;

using System.Collections.Generic;

using MediatR;

using Microsoft.Extensions.Logging;

public class ReaderManagerOld : IReaderManager
{
  private List<ReaderDefinition> readerDefinitions;

  private int selectedIdx;
  private readonly ILogger<ReaderManagerOld> logger;
  private readonly IMediator mediator;

  public ReaderManagerOld(ILogger<ReaderManagerOld> logger, IMediator mediator)
  {
    this.readerDefinitions = new List<ReaderDefinition>();
    this.logger = logger;
    this.mediator = mediator;
  }

  public int SelectedIdx
  {
    get
    {
      this.selectedIdx = EnsureSelectedIndexIsWithinBounds(this.selectedIdx);
      return selectedIdx;
    }
    set
    {
      this.selectedIdx = EnsureSelectedIndexIsWithinBounds(value);
    }
  }

  public ReaderDefinition? SelectedReader
  {
    get
    {
      if (this.readerDefinitions.Any())
        return this.readerDefinitions[SelectedIdx];

      return null;
    }
  }

  public IReadOnlyCollection<ReaderDefinition> GetReaderDefinitions()
  {
    return this.readerDefinitions.AsReadOnly();
  }

  public void RegisterReader(ReaderDefinition readerDefinition)
  {
    ArgumentNullException.ThrowIfNull(readerDefinition, nameof(readerDefinition));

    if (!readerDefinition.IsValid())
      return;

    this.readerDefinitions.Add(readerDefinition);
    
    if (this.readerDefinitions.Count == 1)
      SelectReader(readerDefinition);

    this.mediator.Publish(new ReaderRegistered(readerDefinition));
  }

  public bool UnregisterSelectedReader()
  {
    var rdToRemove = this.SelectedReader;

    return UnregisterReader(rdToRemove);
  }

  public bool UnregisterReader(uint deviceId)
  {
    var rdToRemove =
      this.readerDefinitions.FirstOrDefault(rd => rd.DeviceID == deviceId);

    return UnregisterReader(rdToRemove);
  }

  public bool UnregisterReader(ReaderDefinition rdToRemove)
  {
    if (rdToRemove is null)
      return false;

    var isRemoved = this.readerDefinitions.Remove(rdToRemove);

    if (!isRemoved)
      return false;

    this.selectedIdx =
      EnsureSelectedIndexIsWithinBounds(this.selectedIdx);

    this.mediator.Publish(new ReaderUnregistered(rdToRemove));

    return isRemoved;
  }

  public bool SelectReader(int idx)
  {
    int newIdx = EnsureSelectedIndexIsWithinBounds(idx);

    if (this.selectedIdx == newIdx)
      return false;

    this.mediator.Publish(new SelectedReaderChanged(SelectedReader));

    return true;
  }

  public bool SelectReader(uint deviceId)
  {
    var rd =
      this.readerDefinitions.FirstOrDefault(r => r.DeviceID == deviceId);

    return SelectReader(rd);
  }

  public bool SelectReader(ReaderDefinition? rd)
  {
    if (rd is null)
      return false;

    var idx = this.readerDefinitions.IndexOf(rd);

    return SelectReader(idx);
  }

  private int EnsureSelectedIndexIsWithinBounds(int idx)
  {
    if (this.readerDefinitions.Count <= 0)
      return -1;

    if (this.readerDefinitions.Count == 1)
      return 0;

    if (selectedIdx >= this.readerDefinitions.Count)
      return this.readerDefinitions.Count - 1;

    return idx;
  }
}
