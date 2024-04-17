namespace ElectroCom.RFIDTools.ReaderServices.InventoryService;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using FEDM;

using MediatR;

using Microsoft.Extensions.Logging;

using ElectroCom.RFIDTools.ReaderServices.InventoryService.Events;
using ElectroCom.RFIDTools.ReaderServices.InventoryService.Messages;
using ElectroCom.RFIDTools.ReaderServices.ReaderManagement;
using ElectroCom.RFIDTools.ReaderServices.Model;

public class OBIDTagInventoryService :
  IDisposable,
  ITagInventoryService
{
  private readonly ILogger<OBIDTagInventoryService> logger;
  private readonly IReaderManager readerManager;
  private readonly IMediator mediator;
  private ReaderDescription readerDescription;
  private Task RunningTask = Task.CompletedTask;

  private CancellationTokenSource cancellationTokenSource = new();

  public OBIDTagInventoryService(
    ILogger<OBIDTagInventoryService> logger,
    IMediator mediator,
    IReaderManager readerManager)
  {
    this.logger = logger;
    this.readerManager = readerManager;
    this.readerDescription = readerManager.SelectedReader;
    this.mediator = mediator;
  }

  private ReaderModule Reader => this.readerDescription.ReaderModule;

  public bool IsRunning => !IsNotRunning;

  public bool IsNotRunning => RunningTask is null || RunningTask.IsCompleted;

  public Task StartAsync(string message = "", CancellationToken cancellationToken = default)
  {
    if (IsRunning)
      return Task.CompletedTask;

    cancellationTokenSource = new CancellationTokenSource();

    RunningTask = Task.Run(async () =>
    {
      await RunAsync(cancellationTokenSource.Token);
    })
    .ContinueWith(HandleRunningTaskCompletion);

    this.mediator.Publish(new InventoryStartedMessage(message));

    return Task.CompletedTask;
  }

  /// <summary>
  /// Handles the completion of the Running Task, and any exceptions that may have occurred.
  /// This is required as Running Task is infinite, and never awaited until StopAsync is called.
  /// If an exception happens, it won't be caught until it's awaited.
  /// This Method will handle the exception, and also gracefully stop the Running Task.
  /// </summary>
  /// <param name="tsk">The Completed Task.</param>
  /// <returns>An Awaitable Task.</returns>
  private async Task HandleRunningTaskCompletion(Task tsk)
  {
    if (!tsk.IsFaulted)
      return;

    await StopAsync("Exception in Running Task");

    if (tsk.Exception is null)
      return;

    foreach (var ex in tsk.Exception.Flatten().InnerExceptions)
    {
      logger.LogError("Exception in Running Task {exType} {exMessage}", ex.GetType(), ex.Message);
    }
  }

  public async Task StopAsync(string message = "", CancellationToken cancellationToken = default)
  {
    if (IsNotRunning)
      return;

    cancellationTokenSource?.Cancel();
    await RunningTask;

    this.Reader.rf().off();

    await this.mediator.Publish(new InventoryStoppedMessage(message));
  }

  private async Task RunAsync(CancellationToken cancellationToken = default)
  {
    Reader.hm().setUsageMode(Hm.UsageMode.UseQueue);

    var invParams = new InventoryParam();

    // I should use a configuration for the antenna to use.
    if (this.Reader.readerType() is ReaderType.MRU102)
      invParams.setAntennas(0x08);

    while (!cancellationToken.IsCancellationRequested)
    {
      var state = Reader.hm().inventory(true, invParams);

      // TODO: I should handle a few other error codes depending on what may go wrong.
      // eg. Code 0x01 means no tags, this is fine to continue
      // Code 0x84 Means RF-Warning, the reader has noise issues, perhaps I should stop the loop,
      // or display the error and continue anyway.
      if (state != ErrorCode.Ok)
        continue;

      var tagList = new List<TagEntry>();

      while (Reader.hm().queueItemCount() > 0)
      {
        var tagItem = Reader.hm().popItem();

        if (tagItem is null)
          continue;

        var entry = new TagEntry(tagItem);

        tagList.Add(entry);
      }

      await this.mediator.Publish(new InventoryTagItemsDetectedMessage(tagList));
    }
  }

  public void Dispose()
  {
    //this.messenger.UnregisterAll(this);
  }
}
