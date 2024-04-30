namespace ElectroCom.RFIDTools.UI.Logic.Handlers;

using System.Threading;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.Messaging;

using ElectroCom.RFIDTools.ReaderServices;

using MediatR;

internal class ReaderRegisteredHandler : INotificationHandler<ReaderRegistered>
{
  private readonly IMessenger messenger;

  public ReaderRegisteredHandler(IMessenger messenger)
  {
    this.messenger = messenger;
  }

  public Task Handle(ReaderRegistered notification, CancellationToken cancellationToken)
  {
    this.messenger.Send(notification);
    return Task.CompletedTask;
  }
}
