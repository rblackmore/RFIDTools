namespace TagShelfLocator.UI.Services.ReaderManagement.Messages;

using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// Message to let other services know the reader is disconnecting.
/// This allows them to gracefully stop communication before the reader is disposed
/// Within unmanaged code.
/// </summary>
public class ReaderDisconnecting
{
  private List<Task> busyTasks = new();

  public ReaderDisconnecting(uint DeviceID)
  {
    this.DeviceID = DeviceID;
  }

  public uint DeviceID { get; }

  public IReadOnlyList<Task>? RunningTasks { get; set; }

  public void AddTask(Task busyTask)
  {
    busyTasks.Add(busyTask);
  }
}
