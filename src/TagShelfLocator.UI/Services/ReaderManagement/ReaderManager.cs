namespace TagShelfLocator.UI.Services.ReaderManagement;

using System.Collections.Generic;
using System.Linq;

using MediatR;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using TagShelfLocator.UI.Services.ReaderManagement.Model;

public class ReaderManager : IReaderManager
{
  private Dictionary<uint, ReaderDescription> readers = new();

  private readonly ILogger<ReaderManager> logger;
  private readonly IMediator mediator;
  private readonly IHostApplicationLifetime appLifetime;

  private uint selectedReaderId;

  public ReaderManager(
    ILogger<ReaderManager> logger,
    IMediator mediator,
    IHostApplicationLifetime appLifetime)
  {
    this.logger = logger;
    this.mediator = mediator;
    this.appLifetime = appLifetime;

    this.appLifetime.ApplicationStopping.Register(HandleGracefulShutdown);
  }

  private void HandleGracefulShutdown()
  {
    foreach (var r in this.readers.Values.ToList())
      r.Disconnect();
  }

  public void AddReaderDescription(uint deviceID, ReaderDescription description)
  {
    this.logger.LogInformation("Adding New Reader: {deviceID}:{deviceName}", deviceID, description.ReaderName);
    this.readers[deviceID] = description;

    if (this.readers.Count == 1)
      SetSelectedReader(this.readers.First().Key);
  }

  public void RemoveReaderDescription(uint deviceID)
  {
    if (!this.readers.TryGetValue(deviceID, out ReaderDescription? rd))
      return;

    this.logger.LogInformation("Removing Reader: {deviceID}:{deviceName}", deviceID, rd.ReaderName);

    rd.Disconnect();

    this.readers.Remove(deviceID);

    if (this.readers.Count == 1)
      SetSelectedReader(this.readers.First().Key);
  }

  public ReaderDescription this[uint deviceID] => this.readers[deviceID];

  public IReadOnlyList<ReaderDescription> GetReaderDescriptions()
  {
    return this.readers.Values.ToList().AsReadOnly();
  }

  public ReaderDescription SelectedReader =>
    (TryGetReaderByDeviceID(this.selectedReaderId, out ReaderDescription? rd))
      ? rd
      : null!;

  public bool SetSelectedReader(uint readerId)
  {
    if (this.selectedReaderId == readerId)
      return true;

    if (!this.readers.ContainsKey(readerId))
      return false;

    this.selectedReaderId = readerId;

    this.mediator.Publish(new SelectedReaderChanged(readerId));

    return true;
  }

  public uint[] GetReaderIDs()
  {
    return this.readers.Keys.ToArray();
  }

  public bool TryGetReaderByDeviceID(uint deviceID, out ReaderDescription reader)
  {
    return this.readers.TryGetValue(deviceID, out reader!);
  }
}
