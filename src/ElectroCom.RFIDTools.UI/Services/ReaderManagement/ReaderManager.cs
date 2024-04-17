namespace TagShelfLocator.UI.Services.ReaderManagement;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

  public void AddReaderDescription(uint deviceID, ReaderDescription rd)
  {
    this.readers[deviceID] = rd;

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
  public async Task<bool> ConnectReader(uint deviceID)
  {
    if (!this.readers.TryGetValue(deviceID, out ReaderDescription? rd))
      return false;

    if (!rd.Connect())
      return false;

    var notification = new ReaderConnected(deviceID, rd.ReaderName);
    await this.mediator.Publish(notification);

    return true;
  }

  public async Task<bool> DisconnectReader(uint deviceID)
  {
    if (!this.readers.TryGetValue(deviceID, out ReaderDescription? rd))
      return false;

    var disconnectingNotification = new ReaderDisconnecting(deviceID);
    await this.mediator.Publish(disconnectingNotification);

    if (!rd.Disconnect())
      return false;

    var disconnectedNotificaiton = new ReaderDisconnected(deviceID);
    await this.mediator.Publish(disconnectedNotificaiton);

    return true;
  }

  private void HandleGracefulShutdown()
  {
    foreach (var r in this.readers.Values.ToList())
      r.Disconnect();
  }
}
