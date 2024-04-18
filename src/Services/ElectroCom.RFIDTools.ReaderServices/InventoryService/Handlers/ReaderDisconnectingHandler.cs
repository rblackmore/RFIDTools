namespace ElectroCom.RFIDTools.ReaderServices.InventoryService.Handlers;

using System.Threading;
using System.Threading.Tasks;

using ElectroCom.RFIDTools.ReaderServices.ReaderManagement;

using MediatR;

internal class ReaderDisconnectingHandler : INotificationHandler<ReaderDisconnecting>
{
  private ITagReadingService tagReadingService;

  public ReaderDisconnectingHandler(ITagReadingService tagReadingService)
  {
    this.tagReadingService = tagReadingService;
  }

  public async Task Handle(ReaderDisconnecting notification, CancellationToken cancellationToken)
  {
    await tagReadingService.StopAsync(cancellationToken);
  }
}
