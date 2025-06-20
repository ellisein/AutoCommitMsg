﻿using System.Globalization;
using System.Windows.Data;

namespace AutoCommitMsg.Converters;

public class EnumToBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value != null && value.Equals(parameter);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (bool)value ? parameter : System.Windows.Data.Binding.DoNothing;
    }
}
