using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace WPFCommon
{
    [ValueConversion(typeof(WindowState), typeof(string))]
    public class TitleButtonCaptionConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var param = parameter as string;
            var valueType = (WindowState)value;

            switch (param)
            {
                case "1":
                    return "0";

                case "2":
                    if (valueType == WindowState.Maximized)
                    {
                        return "2";
                    }
                    else
                    {
                        return "1";
                    }
                case "3":
                    return "r";

                default:
                    return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return targetType;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}