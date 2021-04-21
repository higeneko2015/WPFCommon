using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace WPFCommon
{
    [ValueConversion(typeof(int), typeof(string))]
    public class StringToIntegerConverter : MarkupExtension, IValueConverter
    {
        // データから画面へ(int→string)
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var target = value as int?;

            if (target.HasValue == false)
            {
                return "0";
            }
            else
            {
                return target.Value.ToString(parameter as string);
            }
        }

        // 画面の値をデータソースへ(string→int)
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var target = value as string;

            if (target.IsEmpty())
            {
                return 0;
            }
            else
            {
                return int.Parse(target);
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}