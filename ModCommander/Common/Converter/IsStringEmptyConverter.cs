namespace ModCommander.Common.Converter
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    internal sealed class IsStringEmptyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return String.IsNullOrEmpty(value as string);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
