using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace WPFCommon
{
    [ValueConversion(typeof(Date), typeof(string))]
    public class StringToDateConverter : MarkupExtension, IValueConverter
    {
        // データから画面へ(Date→string)
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var target = value as Date;

            if (target.IsEmpty())
            {
                return null;
            }
            else
            {
                return target.ToString(parameter as string);
            }
        }

        // 画面の値をデータソースへ(string→Date)
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var target = value as string;

            if (target.IsEmpty())
            {
                return null;
            }

            var now = Date.Now;
            var year = now.Year;
            var month = now.Month;
            int day;

            switch (target.Length)
            {
                case 1:
                    // "."の場合はシステム日付だと判断する
                    // 上記以外の場合はシステム年月の入力日とする
                    if (target == ".")
                    {
                        return now;
                    }
                    return new Date(year, month, int.Parse(target));

                case 2:
                    // システム年月の入力日だと判断する
                    return new Date(year, month, int.Parse(target));

                case 4:
                    // システム年の入力月日だと判断する
                    month = int.Parse(target.Substring(0, 2));
                    day = int.Parse(target.Substring(2, 2));

                    return new Date(year, month, day);

                case 8:
                    // yyyyMMddの入力だと判断する
                    year = int.Parse(target.Substring(0, 4));
                    month = int.Parse(target.Substring(4, 2));
                    day = int.Parse(target.Substring(6, 2));

                    return new Date(year, month, day);
            }
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
