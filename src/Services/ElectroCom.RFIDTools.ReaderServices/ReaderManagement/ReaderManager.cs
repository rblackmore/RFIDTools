namespace ElectroCom.RFIDTools.ReaderServices;
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
    this.SelectedReader = ReaderFactory.CreateReader(CommsInterface.None);
    this.mediator = mediator;
    this.logger = logger;
  }

  public ReaderDefinition SelectedReader { get; private set; }

  public IReadOnlyCollection<ReaderDefinition> GetReaderDefinitions()
  {
    return readerDefinitions.AsReadOnly();
  }

  public void RegisterReader(ReaderDefinition rd)
  {
    if (rd is NullReaderDefinition)
      return;

    if (this.readerDefinitions.Contains(rd))
      return;

    this.readerDefinitions.Add(rd);

    rd.ReaderConnected += ReaderConnected_Handler;
    rd.ReaderDisconnected += ReaderDisconnected_Handler;

    this.mediator.Publish(new ReaderRegistered(rd));

    OnCollectionChanged();
  }

  private void ReaderConnected_Handler(object sender, ReaderConnectedEventArgs e)
  {
    this.mediator.Publish(new ReaderConnected(e.ReaderDefinition));
  }

  private void ReaderDisconnected_Handler(object sender, ReaderDisconnectedEventArgs e)
  {
    this.mediator.Publish(new ReaderDisconnected(e.ReaderDefinition));
  }

  public void UnregisterReader(ReaderDefinition rdToRemove)
  {
    if (!this.readerDefinitions.Contains(rdToRemove))
      return;

    if (this.readerDefinitions.Remove(rdToRemove))
    {
      rdToRemove.Disconnect();
      rdToRemove.ReaderConnected -= ReaderConnected_Handler;
      rdToRemove.ReaderDisconnected += ReaderDisconnected_Handler;

      this.mediator.Publish(new ReaderUnregistered(rdToRemove));
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
      this.SelectedReader = ReaderFactory.CreateReader(CommsInterface.None);
      this.mediator.Publish(new SelectedReaderChanged(SelectedReader));
      return;
    }

    if (idx >= this.readerDefinitions.Count)
      return;

    this.SelectedReader = this.readerDefinitions[idx];

    this.mediator.Publish(new SelectedReaderChanged(SelectedReader));
  }

  // Call this whenever the collection may change.
  // This Adjusts the currently selected reader
  // if the current one is null or removed.
  private void OnCollectionChanged()
  {
    if (!this.readerDefinitions.Any())
      SelectReaderByIndex(-1);

    if (this.readerDefinitions.Any() && this.SelectedReader is NullReaderDefinition)
      SelectReaderByIndex(0);

    if (!this.readerDefinitions.Contains(this.SelectedReader))
      SelectReaderByIndex(0);

    if (this.SelectedReader is null)
      this.SelectedReader = ReaderFactory.CreateReader(CommsInterface.None);
  }
}
