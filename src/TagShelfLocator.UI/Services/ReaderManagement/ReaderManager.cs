namespace TagShelfLocator.UI.Services.ReaderManagement;

using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

using MediatR;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class ReaderManager : IReaderManager
{
  private readonly Dictionary<uint, ReaderDescription> readers;

  private readonly ILogger<ReaderManager> logger;
  private readonly IMediator mediator;
  private readonly IHostApplicationLifetime appLifetime;

  private uint selectedReaderId;

  public ReaderManager(
    ILogger<ReaderManager> logger,
    IMediator mediator,
    IHostApplicationLifetime appLifetime)
  {
    this.readers = new Dictionary<uint, ReaderDescription>();

    this.logger = logger;
    this.mediator = mediator;
    this.appLifetime = appLifetime;

    this.appLifetime.ApplicationStopping
      .Register(HandleGracefulShutdown);
  }

  public ReaderDescription SelectedReader =>
    readers.TryGetValue(selectedReaderId, out ReaderDescription? rd)
    ? rd
    : null!;

  public ReaderDescription this[uint deviceID] => this.readers[deviceID];

  public uint[] GetDeviceIDs() => this.readers.Keys.ToArray();

  public IReadOnlyList<ReaderDescription> GetReaderDescriptions()
    => this.readers.Values.ToList().AsReadOnly();

  public void SetSelectedReader(uint readerId)
  {
    if (this.selectedReaderId == readerId)
      return;

    if (!this.readers.ContainsKey(readerId))
      return;

    this.selectedReaderId = readerId;

    this.mediator.Publish(new SelectedReaderChanged(readerId));
  }

  public bool TryGetReaderByDeviceID(uint deviceID, out ReaderDescription reader)
  {
    return this.readers.TryGetValue(deviceID, out reader!);
  }

  public void AddReaderDescription(uint deviceID, ReaderDescription description)
  {
    this.readers[deviceID] = description;

    var notification = new ReaderAdded(deviceID);

    this.mediator.Publish(notification);

    if (this.readers.Count == 1)
      SetSelectedReader(this.readers.First().Key);
  }

  public void RemoveReaderDescription(uint deviceID)
  {
    if (!this.readers.TryGetValue(deviceID, out ReaderDescription? rd))
      return;

    rd.Disconnect();

    this.readers.Remove(deviceID);

    var notification = new ReaderRemoved(deviceID);

    this.mediator.Publish(notification);

    if (this.readers.Count == 1)
      SetSelectedReader(this.readers.First().Key);
  }

  private void HandleGracefulShutdown()
  {
    foreach (var r in this.readers.Values.ToList())
      r.Disconnect();
  }
}
