namespace TagShelfLocator.UI;

using System;
using System.Windows;

using ElectroCom.RFIDTools.UI.Logic.Services;
using ElectroCom.RFIDTools.UI.Logic.ViewModels;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serilog;

using TagShelfLocator.UI.Helpers;
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

    Log.Logger = logger;

    builder.Logging.ClearProviders();
    builder.Logging.AddSerilog(logger);
  }

  private void ConfigureServices(HostApplicationBuilder builder)
  {
    builder.Services.AddRFIDToolsUILogic();

    builder.Services.AddHostedService<UsbListener>();
    builder.Services.AddSingleton<IReaderManager, ReaderManager>();
    builder.Services.AddSingleton<ITagInventoryService, OBIDTagInventoryService>();

    builder.Services.AddSingleton<Shell>();

    builder.Services.AddMediatR(cfg =>
    {
      cfg.RegisterServicesFromAssembly(typeof(App).Assembly);
    });
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
