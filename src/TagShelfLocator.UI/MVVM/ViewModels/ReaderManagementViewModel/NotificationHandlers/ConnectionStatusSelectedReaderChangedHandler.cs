namespace TagShelfLocator.UI.MVVM.ViewModels;

using System.Threading;
using System.Threading.Tasks;

using MediatR;

using TagShelfLocator.UI.Services.ReaderManagement;

public class ConnectionStatusSelectedReaderChangedHandler : INotificationHandler<SelectedReaderChanged>
{
  private readonly IReaderConnectionStatusViewModel vm;

  public ConnectionStatusSelectedReaderChangedHandler(
    IReaderConnectionStatusViewModel viewModel)
  {
    this.vm = viewModel;
  }

  public Task Handle(SelectedReaderChanged notification, CancellationToken cancellationToken)
  {
    this.vm.SelectedReaderChanged(notification.DeviceID);
    return Task.CompletedTask;
  }
}
