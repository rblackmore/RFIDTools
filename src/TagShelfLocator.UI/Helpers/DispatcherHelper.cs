namespace TagShelfLocator.UI.Helpers;

using System;
using System.Text;
using System.Windows.Threading;

/// <summary>
/// Code referenced from MVVMLight.
/// https://github.com/lbugnion/mvvmlight
/// </summary>
public static class DispatcherHelper
{
  public static Dispatcher? UIDispatcher { get; private set; }

  public static void CheckBeginInvokeOnUI(Action action)
  {
    if (action is null)
      return;

    CheckDispatcher();

    if (UIDispatcher!.CheckAccess())
      action();
    else
      UIDispatcher.BeginInvoke(action);
  }

  private static void CheckDispatcher()
  {
    if (UIDispatcher is not null)
      return;

    var error = new StringBuilder("The Dispatcher is not initialized.");
    
    error.Append("Call DispatcherHelper.Initialize() during App Startup");

    throw new InvalidOperationException(error.ToString());
  }

  public static void Initialize()
  {
    if (UIDispatcher is not null)
      return;

    UIDispatcher = Dispatcher.CurrentDispatcher;
  }

  public static void Reset() => UIDispatcher = null!;
}
