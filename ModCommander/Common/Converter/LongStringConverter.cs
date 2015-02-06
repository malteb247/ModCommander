namespace ModCommander.Common.Converter
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Data;

    [ValueConversion(typeof(String), typeof(String))]
    public class LongStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string date = (string)value;

            if (date.Length >= 80)
            {
                date = date.Substring(date.Length - 80, date.Length);

                return "..." + date;
            }
            else
            {
                return date;
            }            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //string strValue = value as string;
            //DateTime resultDateTime;
            //if (DateTime.TryParse(strValue, out resultDateTime))
            //{
            //    return resultDateTime;
            //}
            //return DependencyProperty.UnsetValue;

            throw new NotImplementedException();
        }
    }
}
