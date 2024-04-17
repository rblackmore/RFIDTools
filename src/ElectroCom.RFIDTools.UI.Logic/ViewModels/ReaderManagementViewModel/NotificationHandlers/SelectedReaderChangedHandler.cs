namespace ElectroCom.RFIDTools.UI.Logic.ViewModels;

using System.Threading;
using System.Threading.Tasks;

using MediatR;

using TagShelfLocator.UI.Services.ReaderManagement;

public class SelectedReaderChangedHandler : INotificationHandler<SelectedReaderChanged>
{
  private readonly IReaderManagementVM vm;

  public SelectedReaderChangedHandler(IReaderManagementVM viewModel)
  {
    this.vm = viewModel;
  }

  public Task Handle(SelectedReaderChanged notification, CancellationToken cancellationToken)
  {
    this.vm.SelectedReaderChanged(notification.DeviceID);

    return Task.CompletedTask;
  }
}
