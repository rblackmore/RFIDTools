namespace TagShelfLocator.UI;

using System;
using System.Configuration;
using System.Windows;

using CommunityToolkit.Mvvm.Messaging;

using FEDM;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serilog;

using TagShelfLocator.UI.Helpers;
using TagShelfLocator.UI.Services;
using TagShelfLocator.UI.ViewModels;

using DateTime = System.DateTime;

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
    var timestamp = DateTime.Now.ToString("dd-HHmmss");

    string logFile = $"protocollog{timestamp}.log";
    var appLoggingParams = AppLoggingParam.createFileLogger(logFile);

    var reader = new ReaderModule(RequestMode.UniDirectional);

    if (builder.Environment.IsDevelopment())
    {
      Serilog.Log.Logger.Information("Protocol Log File: {logfile}", logFile);
      reader.log().startLogging(appLoggingParams);
    }

    builder.Services.AddSingleton(reader);
    builder.Services.AddSingleton<Shell>();
    builder.Services.AddHostedService<ReaderConnectionListener>();
    builder.Services.AddSingleton<IMessenger>(WeakReferenceMessenger.Default);
    builder.Services.AddSingleton<TagReaderService>();
    builder.Services.AddTransient<IMainViewModel, MainViewModel>();
  }

  private async void Application_Startup(object sender, StartupEventArgs e)
  {
    DispatcherHelper.Initialize();
    var logger = this.host.Services.GetRequiredService<ILogger<App>>();

    var shell = this.host.Services.GetRequiredService<Shell>();

    shell.Show();

    await this.host.StartAsync();
  }

  private async void Application_Exit(object sender, ExitEventArgs e)
  {
    await this.host.StopAsync();
    this.host.Dispose();
  }
}
