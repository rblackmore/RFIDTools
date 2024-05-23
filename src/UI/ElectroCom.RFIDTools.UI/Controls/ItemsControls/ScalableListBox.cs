namespace ElectroCom.RFIDTools.UI.Controls;

using System.Windows;
using System.Windows.Controls;

using ElectroCom.RFIDTools.UI.Controls.Selectors;

public enum Layout
{
  Tile,
  List
}

public class ScalableListBox : ListBox
{

  #region Scale Properties

  #region Scale
  public double Scale
  {
    get { return (double)GetValue(ScaleProperty); }
    set { SetValue(ScaleProperty, value); }
  }

  public static readonly DependencyProperty ScaleProperty =
      DependencyProperty.Register(nameof(Scale), typeof(double), typeof(ScalableListBox), new PropertyMetadata(100d, OnScaleChanged, CoerceScaleBounds));

  private static void OnScaleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    // Nothing to do here, move along.
  }

  private static object CoerceScaleBounds(DependencyObject d, object baseValue)
  {
    var slb = (ScalableListBox)d;
    var newValue = (double)baseValue;

    newValue = (newValue < slb.MinScale) ? slb.MinScale : newValue;
    newValue = (newValue > slb.MaxScale) ? slb.MaxScale : newValue;

    return newValue;
  }
  #endregion

  #region MinScale
  public double MinScale
  {
    get { return (double)GetValue(MinScaleProperty); }
    set { SetValue(MinScaleProperty, value); }
  }

  public static readonly DependencyProperty MinScaleProperty =
      DependencyProperty.Register(nameof(MinScale), typeof(double), typeof(ScalableListBox), new PropertyMetadata(80d));
  #endregion

  #region MaxScale
  public double MaxScale
  {
    get { return (double)GetValue(MaxScaleProperty); }
    set { SetValue(MaxScaleProperty, value); }
  }

  public static readonly DependencyProperty MaxScaleProperty =
      DependencyProperty.Register(nameof(MaxScale), typeof(double), typeof(ScalableListBox), new PropertyMetadata(250d));
  #endregion

  #endregion

  #region Layout Properties

  #region Layout
  public Layout Layout
  {
    get { return (Layout)GetValue(LayoutProperty); }
    set { SetValue(LayoutProperty, value); }
  }

  public static readonly DependencyProperty LayoutProperty =
      DependencyProperty.Register(nameof(Layout), typeof(Layout), typeof(ScalableListBox), new PropertyMetadata(Layout.List, OnLayoutChanged));

  private static void OnLayoutChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    var slb = (ScalableListBox)d;

    if (slb.ItemsPanelTemplateSelector is not null)
    {
      slb.ItemsPanel = slb.ItemsPanelTemplateSelector.SelectTemplate(e.NewValue, slb);
    }

    if (slb.ItemTemplateSelector is not null)
    {
      slb.ItemTemplate = slb.ItemTemplateSelector.SelectTemplate(e.NewValue, slb);
    }

    if (slb.ItemContainerStyleSelector is not null)
    {
      slb.ItemContainerStyle = slb.ItemContainerStyleSelector.SelectStyle(e.NewValue, slb);
    }
  }
  #endregion

  #region ItemsPanelTemplateSelector
  public ItemsPanelTemplateSelector ItemsPanelTemplateSelector
  {
    get { return (ItemsPanelTemplateSelector)GetValue(ItemsPanelTemplateSelectorProperty); }
    set { SetValue(ItemsPanelTemplateSelectorProperty, value); }
  }

  // Using a DependencyProperty as the backing store for ItemsPanelTemplateSelector.  This enables animation, styling, binding, etc...
  public static readonly DependencyProperty ItemsPanelTemplateSelectorProperty =
      DependencyProperty.Register(nameof(ItemsPanelTemplateSelector), typeof(ItemsPanelTemplateSelector), typeof(ScalableListBox));
  #endregion

  #endregion
}
