using System.Globalization;
using System.Windows.Controls;

namespace WPFCommon
{
    public class DateValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var val = value as string;

            if (val.IsEmpty())
            {
                return new ValidationResult(true, null);
            }

            var now = Date.Now;
            var year = now.Year;
            var month = now.Month;
            var hasError = false;

            int day;
            switch (val.Length)
            {
                case 1:
                    // "."の場合はシステム日付だと判断する
                    // 上記以外の場合はシステム年月の入力日とする
                    if (val == "0")
                    {
                        hasError = true;
                    }
                    break;

                case 2:
                    // システム年月の入力日だと判断する
                    if (Date.TryParse($"{year}/{month}/{val}", out _) == false)
                    {
                        hasError = true;
                    }
                    break;

                case 4:
                    // システム年の入力年月だと判断する
                    month = int.Parse(val.Substring(0, 2));
                    day = int.Parse(val.Substring(2, 2));

                    if (Date.TryParse($"{year}/{month}/{day}", out _) == false)
                    {
                        hasError = true;
                    }
                    break;

                case 8:
                    // yyyyMMddの入力だと判断する
                    year = int.Parse(val.Substring(0, 4));
                    month = int.Parse(val.Substring(4, 2));
                    day = int.Parse(val.Substring(6, 2));

                    if (Date.TryParse($"{year}/{month}/{day}", out _) == false)
                    {
                        hasError = true;
                    }
                    break;

                default:
                    hasError = true;
                    break;
            }
            if (hasError)
            {
                var svr = ApplicationEx.GetService<IMessageService>();
                var msg = svr.Get("00011", cultureInfo.Name);

                var msgsvr = ApplicationEx.GetService<IShowMessageService>();
                msgsvr.Show("00011", cultureInfo.Name);

                return new ValidationResult(false, msg.MessageString);
            }
            return new ValidationResult(true, null);
        }
    }
}
