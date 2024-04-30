namespace OBID.Scratch;

using FEDM;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public static class ServiceManager
{
  public static IServiceProvider ServiceProvider { get; private set; }

  public static async Task ConfigureAndRunHost()
  {
    var builder = Host.CreateApplicationBuilder();

    builder.Services.ConfigureServices();

    var host = builder.Build();

    ServiceProvider = host.Services;

    await host.StartAsync();
  }
}

public static class ServiceCollectionExtensions
{
  public static IServiceCollection ConfigureServices(this IServiceCollection services)
  {
    //services.AddSingleton<IReaderManager, ReaderManager>();
    var readerModule = new ReaderModule(RequestMode.UniDirectional);
    services.AddSingleton(readerModule);
    services.AddHostedService<UsbListener>();
    
    services.AddMediatR(cfg =>
    {
      cfg.RegisterServicesFromAssembly(typeof(ServiceManager).Assembly);
    });

    return services;
  }
}
