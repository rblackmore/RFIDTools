namespace ElectroCom.RFIDTools.ReaderServices.ReaderManagement;
using System;
using System.Collections.Generic;

using MediatR;

using Microsoft.Extensions.Logging;

public class ReaderManager : IReaderManager
{
  private List<ReaderDefinition> readerDefinitions;

  private readonly ILogger<ReaderManager> logger;
  private readonly IMediator mediator;

  public ReaderManager(IMediator mediator, ILogger<ReaderManager> logger)
  {
    this.readerDefinitions = new List<ReaderDefinition>();
    this.mediator = mediator;
    this.logger = logger;
  }

  public ReaderDefinition? SelectedReader { get; private set; }

  public IReadOnlyCollection<ReaderDefinition> GetReaderDefinitions()
  {
    return readerDefinitions.AsReadOnly();
  }

  public void RegisterReader(ReaderDefinition rd)
  {
    if (this.readerDefinitions.Contains(rd))
      return;

    this.readerDefinitions.Add(rd);

    this.mediator.Send(new ReaderRegistered(rd));

    OnCollectionChanged();
  }

  public void UnregisterReader(ReaderDefinition rdToRemove)
  {
    if (!this.readerDefinitions.Contains(rdToRemove))
      return;

    if (this.readerDefinitions.Remove(rdToRemove))
    {
      this.mediator.Send(new ReaderUnregistered(rdToRemove));
      OnCollectionChanged();
    }
  }

  public void SelectReader(ReaderDefinition rd)
  {
    if (!this.readerDefinitions.Contains(rd))
      return;

    var idx = this.readerDefinitions.IndexOf(rd);

    SelectReaderByIndex(idx);
  }

  public void SelectReaderByIndex(int idx)
  {
    if (idx < 0)
    {
      this.SelectedReader = null;
      this.mediator.Send(new SelectedReaderChanged(SelectedReader));
    }

    if (idx >= this.readerDefinitions.Count)
      return;

    this.SelectedReader = this.readerDefinitions[idx];

    this.mediator.Send(new SelectedReaderChanged(SelectedReader));
  }

  // Call this whenever the collection may change.
  // This Adjusts the currently selected reader if the currently one is null or removed.
  private void OnCollectionChanged()
  {
    if (!this.readerDefinitions.Any())
      SelectReaderByIndex(-1);

    var notContains =
      this.SelectedReader is not null &&
      !this.readerDefinitions.Contains(this.SelectedReader);

    if (notContains || this.readerDefinitions.Count == 1)
      SelectReaderByIndex(0);
  }
}
