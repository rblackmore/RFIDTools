namespace ElectroCom.RFIDTools.UI.Logic.Helpers;
public static class ServiceLocator
{
  public static Func<IServiceProvider>? Locator { get; set; }

  public static IServiceProvider ServiceProvider => (Locator is not null)
    ? Locator()
    : throw new NullReferenceException($"{nameof(Locator)} has not been configured");
}
