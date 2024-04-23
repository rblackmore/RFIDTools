namespace ElectroCom.RFIDTools.UI.Logic.Handlers;

using System.Threading;
using System.Threading.Tasks;

using CommunityToolkit.Mvvm.Messaging;

using ElectroCom.RFIDTools.ReaderServices;

using MediatR;

internal class SelectedReaderChangedHandler : INotificationHandler<SelectedReaderChanged>
{
  private readonly IMessenger messenger;

  public SelectedReaderChangedHandler(IMessenger messenger)
  {
    this.messenger = messenger;
  }

  public Task Handle(SelectedReaderChanged notification, CancellationToken cancellationToken)
  {
    this.messenger.Send(notification);
    return Task.CompletedTask;
  }
}
