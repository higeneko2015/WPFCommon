using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace WPFCommon
{
    [ValueConversion(typeof(decimal?), typeof(string))]
    public class StringToNullableDecimalConverter : MarkupExtension, IValueConverter
    {
        // データから画面へ(decimal?→string)
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var target = value as decimal?;

            if (target.HasValue == false)
            {
                return null;
            }
            else
            {
                return target.Value.ToString(parameter as string);
            }
        }

        // 画面の値をデータソースへ(string→decimal?)
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var target = value as string;

            if (target.IsEmpty())
            {
                return null;
            }
            else
            {
                return decimal.Parse(target);
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
