namespace ElectroCom.RFIDTools.UI.Logic;

public static class ServiceLocator
{
  public static Func<IServiceProvider>? Locator { get; set; }

  public static IServiceProvider ServiceProvider => Locator!();
}
