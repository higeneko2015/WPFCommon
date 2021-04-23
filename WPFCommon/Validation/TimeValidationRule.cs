using System;
using System.Globalization;
using System.Windows.Controls;
using static WPFCommon.ShowMessageService;

namespace WPFCommon
{
    public class TimeValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var val = value as string;

            if (val.IsEmpty())
            {
                return new ValidationResult(true, null);
            }

            var now = Time.Now;
            int hour;
            var minute = now.Minute;
            var second = now.Second;
            var hasError = false;

            switch (val.Length)
            {
                case 1:
                    // "."の場合はシステム時刻だと判断する
                    // 上記以外の場合はシステム時刻の時とする
                    break;

                case 2:
                    // システム時刻の時だと判断する
                    if (Time.TryParse($"{val.ToString()}:{minute.ToString()}:{second.ToString()}", out _) == false)
                    {
                        hasError = true;
                    }
                    break;

                case 4:
                    // システム時刻の入力時分だと判断する
                    hour = int.Parse(val.Substring(0, 2));
                    minute = int.Parse(val.Substring(2, 2));

                    if (Time.TryParse($"{hour.ToString()}:{minute.ToString()}:{second.ToString()}", out _) == false)
                    {
                        hasError = true;
                    }
                    break;

                case 6:
                    // HHmmssの入力だと判断する
                    hour = int.Parse(val.Substring(0, 2));
                    minute = int.Parse(val.Substring(2, 2));
                    second = int.Parse(val.Substring(4, 2));

                    if (Time.TryParse($"{hour.ToString()}:{minute.ToString()}:{second.ToString()}", out _) == false)
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
                var msg = svr.Get("00012", cultureInfo.Name);

                var msgsvr = ApplicationEx.GetService<IShowMessageService>();
                if (Enum.TryParse(msg.MessageType, out MessageType msgType) == true)
                {
                    msgsvr.DirectShow(msg.MessageString, msgType);
                }
                else
                {
                    msgsvr.DirectShow(msg.MessageString, MessageType.Information);
                }
                //msgsvr.Show("00012", cultureInfo.Name);

                return new ValidationResult(false, msg.MessageString);
            }
            return new ValidationResult(true, null);
        }
    }
}
