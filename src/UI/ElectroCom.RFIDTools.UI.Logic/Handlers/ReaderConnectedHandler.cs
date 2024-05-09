namespace ElectroCom.RFIDTools.UI.Logic.Handlers;

using System.Threading;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.Messaging;

using ElectroCom.RFIDTools.ReaderServices;

using MediatR;

public class ReaderConnectedHandler : INotificationHandler<ReaderConnected>
{
  private IMessenger messenger;

  public ReaderConnectedHandler(IMessenger messenger)
  {
    this.messenger = messenger;
  }

  public Task Handle(ReaderConnected notification, CancellationToken cancellationToken)
  {
    this.messenger.Send(notification);
    return Task.CompletedTask;
  }
}
