﻿namespace Microsoft.Extensions.DependencyInjection;

using ElectroCom.RFIDTools.ReaderServices;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddReaderServices(this IServiceCollection services)
  {
    services.AddHostedService<UsbListener>();
    services.AddSingleton<IReaderManager, ReaderManager>();
    services.AddTransient<ITagReaderFactory, TagReaderFactory>();

    return services;
  }
}
