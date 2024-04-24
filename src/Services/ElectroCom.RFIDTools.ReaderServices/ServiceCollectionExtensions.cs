namespace Microsoft.Extensions.DependencyInjection;

using ElectroCom.RFIDTools.ReaderServices.InventoryService;
using ElectroCom.RFIDTools.ReaderServices;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddReaderServices(this IServiceCollection services)
  {
    services.AddHostedService<UsbListener>();
    services.AddSingleton<IReaderManager, ReaderManagerOld>();
    services.AddSingleton<ITagReadingService, OBIDTagInventoryService>();

    return services;
  }
}
