using System;
using System.Globalization;
using System.Windows.Controls;
using static WPFCommon.ShowMessageService;

namespace WPFCommon
{
    public class IntegerValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var val = value as string;

            if (val.IsEmpty())
            {
                return new ValidationResult(true, null);
            }

            var ret = int.TryParse(val, out _);
            if (ret)
            {
                return new ValidationResult(true, null);
            }
            else
            {
                var svr = ApplicationEx.GetService<IMessageService>();
                var msg = svr.Get("00010", cultureInfo.Name, "", int.MinValue.ToString(), int.MaxValue.ToString());

                var msgsvr = ApplicationEx.GetService<IShowMessageService>();

                if (Enum.TryParse(msg.MessageType, out MessageType msgType) == true)
                {
                    msgsvr.DirectShow(msg.MessageString, msgType);
                }
                else
                {
                    msgsvr.DirectShow(msg.MessageString, MessageType.Information);
                }

                return new ValidationResult(false, msg.MessageString);
            }
        }
    }
}