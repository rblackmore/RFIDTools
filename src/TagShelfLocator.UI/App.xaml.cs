namespace TagShelfLocator.UI;

using System.Configuration;
using System.Windows;

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

    builder.Logging.ClearProviders();
    builder.Logging.AddSerilog(logger);
  }

  private void ConfigureServices(HostApplicationBuilder builder)
  {
    builder.Services.AddSingleton<MainWindow>();
  }

  private async void Application_Startup(object sender, StartupEventArgs e)
  {
    await this.host.StartAsync();

    var mainWindow = this.host.Services.GetRequiredService<MainWindow>();

    mainWindow.Show();
  }

  private async void Application_Exit(object sender, ExitEventArgs e)
  {
    await this.host.StopAsync();
    this.host.Dispose();
  }
}
