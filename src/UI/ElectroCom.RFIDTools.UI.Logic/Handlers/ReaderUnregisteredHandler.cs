namespace ElectroCom.RFIDTools.UI.Logic.Handlers;

using System.Threading;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.Messaging;

using ElectroCom.RFIDTools.ReaderServices;

using MediatR;

internal class ReaderUnregisteredHandler : INotificationHandler<ReaderUnregistered>
{
  private readonly IMessenger messenger;

  public ReaderUnregisteredHandler(IMessenger messenger)
  {
    this.messenger = messenger;
  }

  public Task Handle(ReaderUnregistered notification, CancellationToken cancellationToken)
  {
    this.messenger.Send(notification);
    return Task.CompletedTask;
  }
}
