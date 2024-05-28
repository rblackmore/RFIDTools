namespace ElectroCom.RFIDTools.UI;

using System;
using System.Windows;
using System.Windows.Threading;

using ElectroCom.RFIDTools.UI.Logic;
using ElectroCom.RFIDTools.UI.Logic.ViewModels;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serilog;

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

    builder.Services.AddSingleton<Shell>();
  }

  protected override async void OnStartup(StartupEventArgs e)
  {
    var uiDispatcher = Dispatcher.CurrentDispatcher;

    DispatcherHelper.Initialize(action =>
    {
      if (uiDispatcher.CheckAccess())
        action();
      else
        uiDispatcher.BeginInvoke(action);
    });

    ServiceLocator.Locator = () =>
    {
      return App.Current.Services;
    };

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
