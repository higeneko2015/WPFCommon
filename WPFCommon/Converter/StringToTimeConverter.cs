using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace WPFCommon
{
    [ValueConversion(typeof(Time), typeof(string))]
    public class StringToTimeConverter : MarkupExtension, IValueConverter
    {
        // データから画面へ(Time→string)
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var target = value as Time;

            if (target.IsEmpty())
            {
                return null;
            }
            else
            {
                return target.ToString(parameter as string);
            }
        }

        // 画面の値をデータソースへ(string→Time)
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var target = value as string;

            if (target.IsEmpty())
            {
                return null;
            }

            var now = Time.Now;
            int hour;
            int minute;
            int second;

            switch (target.Length)
            {
                case 1:
                    // "."の場合はシステム時刻だと判断する
                    // 上記以外の場合はシステム時刻の入力秒とする
                    if (target == ".")
                    {
                        return now;
                    }
                    return new Time(int.Parse(target), 0, 0);

                case 2:
                    // システム年月の入力日だと判断する
                    return new Time(int.Parse(target), 0, 0);

                case 4:
                    // システム年の入力年月だと判断する
                    hour = int.Parse(target.AsSpan(0, 2));
                    minute = int.Parse(target.AsSpan(2, 2));

                    return new Time(hour, minute, 0);

                case 6:
                    // yyyyMMddの入力だと判断する
                    hour = int.Parse(target.AsSpan(0, 2));
                    minute = int.Parse(target.AsSpan(2, 2));
                    second = int.Parse(target.AsSpan(4, 2));

                    return new Time(hour, minute, second);
            }
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
