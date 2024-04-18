namespace ElectroCom.RFIDTools.UI.Logic.Helpers;

using System;
using System.Text;

/// <summary>
/// Allows marshalling an Action to the UI thread, if not already on the UI Thread.
/// Code referenced from MVVMLight.
/// https://github.com/lbugnion/mvvmlight
/// </summary>
public static class DispatcherHelper
{
  public static void CheckBeginInvokeOnUI(Action action)
  {
    if (action is null)
      return;

    CheckDispatcher();

    InvokeToUIThread!(action);
  }

  private static void CheckDispatcher()
  {
    if (InvokeToUIThread is not null)
      return;

    var error = new StringBuilder("The Dispatcher is not initialized.");

    error.Append("Call DispatcherHelper.Initialize() during App Startup");

    throw new InvalidOperationException(error.ToString());
  }

  private static Action<Action>? InvokeToUIThread;

  public static void Initialize(Action<Action> invoke)
  {
    InvokeToUIThread = invoke;
  }

  public static void Reset() => InvokeToUIThread = null!;
}
