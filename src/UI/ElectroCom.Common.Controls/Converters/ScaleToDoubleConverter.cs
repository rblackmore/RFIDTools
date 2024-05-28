namespace ElectroCom.Common.Controls.Converters;

using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System;

public class ScaleToDoubleConverter : MarkupExtension, IValueConverter
{
  private static ScaleToDoubleConverter Default = null!;

  public override object ProvideValue(IServiceProvider serviceProvider)
  {
    if (Default is null)
      Default = new ScaleToDoubleConverter();
    return Default;
  }

  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    return (double)value / 100d * (double)parameter;
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    throw new NotImplementedException();
  }
}
