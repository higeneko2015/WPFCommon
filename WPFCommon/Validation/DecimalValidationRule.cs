﻿using System.Globalization;
using System.Windows.Controls;

namespace WPFCommon
{
    public class DecimalValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var val = value as string;

            if (val.IsEmpty())
            {
                return new ValidationResult(true, null);
            }

            var ret = decimal.TryParse(val, out _);
            if (ret)
            {
                return new ValidationResult(true, null);
            }
            else
            {
                var svr = ApplicationEx.GetService<IMessageService>();
                var msg = svr.Get("00010", cultureInfo.Name, "", decimal.MinValue.ToString(), decimal.MaxValue.ToString());

                var msgsvr = ApplicationEx.GetService<IShowMessageService>();
                msgsvr.Show("00010", cultureInfo.Name, "", decimal.MinValue.ToString(), decimal.MaxValue.ToString());

                return new ValidationResult(false, msg.MessageString);
            }
        }
    }
}