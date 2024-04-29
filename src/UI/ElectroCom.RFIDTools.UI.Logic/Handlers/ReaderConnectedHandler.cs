namespace ElectroCom.RFIDTools.UI.Logic.Handlers;

using System.Threading;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.Messaging;

using ElectroCom.RFIDTools.ReaderServices;

using MediatR;

public class ReaderDisconnectedHandler : INotificationHandler<ReaderDisconnected>
{
  private IMessenger messenger;

  public ReaderDisconnectedHandler(IMessenger messenger)
  {
    this.messenger = messenger;
  }

  public Task Handle(ReaderDisconnected notification, CancellationToken cancellationToken)
  {
    this.messenger.Send(notification);
    return Task.CompletedTask;
  }
}
