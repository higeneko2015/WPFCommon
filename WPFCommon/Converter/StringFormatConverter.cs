using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace WPFCommon
{
    /// <summary>
    /// a
    /// </summary>
    public class StringFormatConverter : MarkupExtension, IMultiValueConverter
    {
        /// <summary>
        /// a
        /// </summary>
        /// <param name="values">a</param>
        /// <param name="targetType">a</param>
        /// <param name="parameter">a</param>
        /// <param name="culture">a</param>
        /// <returns>a</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var target = values[0] as string;
            var format = values[1] as string;
            if (format.IsEmpty())
            {
                return target;
            }

            return string.Format(format, target);
        }

        /// <summary>
        /// a
        /// </summary>
        /// <param name="value">a</param>
        /// <param name="targetTypes">a</param>
        /// <param name="parameter">a</param>
        /// <param name="culture">a</param>
        /// <returns>a</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return targetTypes;
        }

        /// <summary>
        /// a
        /// </summary>
        /// <param name="serviceProvider">a</param>
        /// <returns>a</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
