namespace ElectroCom.Common.Controls.Converters;

using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

using System;

/// <summary>
/// Converts Scale from to Columns.
/// Second Parameter I also need, other than the scale, will be the dims of the containing scrollviewer.
/// Which I've already bound to the width of the uniform grid.
/// </summary>
public class ScaleToColumnsConverter : MarkupExtension, IMultiValueConverter
{
  private static ScaleToColumnsConverter Default = null!;

  public override object ProvideValue(IServiceProvider serviceProvider)
  {
    if (Default is null)
      Default = new ScaleToColumnsConverter();
    return Default;
  }
  /// <summary>
  /// Takes 2 values, Scale from ScalableListBox (double) and ContainerWidth from ScrollContentPresenter (double).
  ///
  /// </summary>
  /// <param name="values">
  /// 1. Scale as double.
  /// 2. ScrollContentPresenter Width as double.
  /// </param>
  /// <param name="parameter">Target Item Width.</param>
  /// <returns>Calculated Column Count. ScrollContentPresenterWidth / (TargetItemWidth * Scale).</returns>
  public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
  {
    var scale = (double)values[0] / 100;
    var containerWidth = (double)values[1]; 
    var targetWidth = (double)parameter;

    var columns = containerWidth / (targetWidth * scale);

    return (int)Math.Round(columns);
  }

  public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
  {
    throw new NotImplementedException();
  }
}
