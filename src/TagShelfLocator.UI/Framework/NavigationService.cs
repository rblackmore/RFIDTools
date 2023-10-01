namespace TagShelfLocator.UI.Framework;

using System;
using System.Collections.Generic;
using System.Windows.Controls;

public class NavigationService
{
  private Dictionary<Type, Type> viewMapping = new();

  private readonly Frame frame;

  public NavigationService(Frame frame)
  {
    this.frame = frame;
  }

  public void Navigate<T>(object args)
  {
    this.frame.Navigate(typeof(T), args);
  }

}
