namespace TagShelfLocator.UI;

using System;
using System.Windows;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serilog;

using TagShelfLocator.UI.Helpers;
using TagShelfLocator.UI.MVVM.ViewModels;
using TagShelfLocator.UI.Services;
using TagShelfLocator.UI.Services.InventoryService;
using TagShelfLocator.UI.Services.ReaderManagement;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
  private IHost host;

  public IServiceProvider Services => this.host.Services;

  public new static App Current => (App)Application.Current;

  public App()
  {
    var builder = Host.CreateApplicationBuilder();

    this.ConfigureLogging(builder);
    this.ConfigureAppConfiguration(builder);
    this.ConfigureServices(builder);

    this.host = builder.Build();
  }

  private void ConfigureAppConfiguration(HostApplicationBuilder builder)
  {

  }

  private void ConfigureLogging(HostApplicationBuilder builder)
  {
    var logger = new LoggerConfiguration()
      .WriteTo.Console()
      .CreateLogger();

    Serilog.Log.Logger = logger;

    builder.Logging.ClearProviders();
    builder.Logging.AddSerilog(logger);
  }

  private void ConfigureServices(HostApplicationBuilder builder)
  {

    builder.Services.AddHostedService<UsbListener>();
    builder.Services.AddSingleton<IReaderManager, ReaderManager>();
    builder.Services.AddSingleton<Shell>();
    //builder.Services.AddSingleton<IMessenger>(new StrongReferenceMessenger());
    
    builder.Services.AddMediatR(cfg =>
    {
      cfg.RegisterServicesFromAssembly(typeof(App).Assembly);
    });

    builder.Services.AddSingleton<ITagInventoryService, OBIDTagInventoryService>();
    builder.Services.AddSingleton<INavigationService, NavigationService>();

    builder.Services.AddSingleton<IInventoryViewModel, InventoryViewModel>();
    builder.Services.AddSingleton<ISettingsViewModel, SettingsViewModel>();

    builder.Services.AddSingleton<IShellViewModel, ShellViewModel>();

    builder.Services
      .AddSingleton<IReaderConnectionStatusViewModel, ReaderConnectionStatusViewModel>();

    // Register IViewModel Factory.
    // This factory takes in a Type, Gets this service from the standard provider.
    // Type must be of type 'IViewModel'
    builder.Services
      .AddSingleton<Func<Type, IViewModel>>(provider =>
        viewModelType =>
          (ViewModel)provider.GetRequiredService(viewModelType));
  }

  protected override async void OnStartup(StartupEventArgs e)
  {
    DispatcherHelper.Initialize();
    var navigation = this.host.Services.GetRequiredService<INavigationService>();

    navigation.NavigateTo<IInventoryViewModel>();

    var shell = this.host.Services.GetRequiredService<Shell>();

    shell.Show();

    await this.host.StartAsync();
    base.OnStartup(e);
  }
  protected override async void OnExit(ExitEventArgs e)
  {
    await this.host.StopAsync();
    this.host.Dispose();
    base.OnExit(e);
  }
}
