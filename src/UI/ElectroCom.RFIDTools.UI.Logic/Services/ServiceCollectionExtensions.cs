namespace Microsoft.Extensions.DependencyInjection;

using ElectroCom.RFIDTools.UI.Logic.Services;
using ElectroCom.RFIDTools.UI.Logic.ViewModels;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddRFIDToolsUILogic(this IServiceCollection services)
  {
    services.AddSingleton<INavigationService, NavigationService>();

    services.AddSingleton<IInventoryViewModel, InventoryViewModel>();
    services.AddSingleton<ISettingsViewModel, SettingsViewModel>();
    services.AddSingleton<IShellViewModel, ShellViewModel>();
    services.AddSingleton<IReaderManagementVM, ReaderManagementVM>();

    services
      .AddSingleton<Func<Type, IViewModel>>(provider =>
        viewModelType =>
          (ViewModel)provider.GetRequiredService(viewModelType));

    return services;
  }
}
