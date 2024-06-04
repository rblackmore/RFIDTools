namespace Microsoft.Extensions.DependencyInjection;

using System.Reflection;

using CommunityToolkit.Mvvm.Messaging;

using ElectroCom.RFIDTools.ReaderServices;
using ElectroCom.RFIDTools.UI.Logic;
using ElectroCom.RFIDTools.UI.Logic.ViewModels;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddRFIDToolsUILogic(this IServiceCollection services)
  {
    services.AddSingleton<IMessenger>(StrongReferenceMessenger.Default);
    services.AddSingleton<INavigationService, NavigationService>();

    services.AddSingleton<IInventoryViewModel, InventoryViewModel>();
    services.AddSingleton<ISettingsViewModel, SettingsViewModel>();
    services.AddSingleton<IShellViewModel, ShellViewModel>();
    services.AddSingleton<ISelectedReaderStatusVM, SelectedReaderStatusVM>();
    services.AddSingleton<IReaderManagementVM, ReaderManagementVM>();

    services.AddReaderServices();

    services.AddMediatR(cfg =>
    {
      var assemblies = new List<Assembly>
      {
        typeof(ShellViewModel).Assembly,
        typeof(ReaderDefinition).Assembly,
      };

      cfg.RegisterServicesFromAssemblies(assemblies.ToArray());
    });

    services
      .AddSingleton<Func<Type, IViewModel>>(provider =>
        viewModelType =>
          (ViewModel)provider.GetRequiredService(viewModelType));

    return services;
  }
}
