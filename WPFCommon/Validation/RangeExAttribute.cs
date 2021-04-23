using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using static WPFCommon.ShowMessageService;

namespace WPFCommon
{
    public enum RangeExType
    {
        Int,
        Double,
        Decimal,
        Date,
        Time,
        DateTime,
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class RangeExAttribute : ValidationAttribute
    {
        private readonly string _AddMessage = string.Empty;
        private readonly object _Maximum = null;
        private readonly object _Minimum = null;
        private readonly RangeExType _Type;

        public RangeExAttribute(int minimum, int maximum, string addMessage)
        {
            this._Minimum = minimum;
            this._Maximum = maximum;
            this._AddMessage = addMessage;
            this._Type = RangeExType.Int;
        }

        public RangeExAttribute(RangeExType type, string minimum, string maximum, string addMessage)
        {
            this._Minimum = minimum;
            this._Maximum = maximum;
            this._AddMessage = addMessage;
            this._Type = type;
        }

        public override bool IsValid(object value)
        {
            if (value.IsEmpty())
            {
                return true;
            }

            IComparable val = (IComparable)value;
            IComparable min = null;
            IComparable max = null;

            switch (this._Type)
            {
                case RangeExType.Decimal:
                    min = decimal.Parse(this._Minimum as string);
                    max = decimal.Parse(this._Maximum as string);
                    break;

                case RangeExType.Int:
                    min = (int)this._Minimum;
                    max = (int)this._Maximum;
                    break;

                case RangeExType.Date:
                    min = Date.Parse(this._Minimum as string);
                    max = Date.Parse(this._Maximum as string);
                    break;
            }

            if (min.CompareTo(val) > 0)
            {
                var svr = ApplicationEx.GetService<IMessageService>();
                var msg = svr.Get("00010", CultureInfo.CurrentCulture.Name, this._AddMessage, this._Minimum.ToString(), this._Maximum.ToString());
                this.ErrorMessage = msg.MessageString;

                var msgsvr = ApplicationEx.GetService<IShowMessageService>();
                if (Enum.TryParse(msg.MessageType, out MessageType msgType) == true)
                {
                    msgsvr.DirectShow(msg.MessageString, msgType);
                }
                else
                {
                    msgsvr.DirectShow(msg.MessageString, MessageType.Information);
                }
                //msgsvr.Show("00010", CultureInfo.CurrentCulture.Name, this._AddMessage, this._Minimum.ToString(), this._Maximum.ToString());

                return false;
            }

            if (max.CompareTo(val) < 0)
            {
                var svr = ApplicationEx.GetService<IMessageService>();
                var msg = svr.Get("00010", CultureInfo.CurrentCulture.Name, this._AddMessage, this._Minimum.ToString(), this._Maximum.ToString());
                this.ErrorMessage = msg.MessageString;

                var msgsvr = ApplicationEx.GetService<IShowMessageService>();
                if (Enum.TryParse(msg.MessageType, out MessageType msgType) == true)
                {
                    msgsvr.DirectShow(msg.MessageString, msgType);
                }
                else
                {
                    msgsvr.DirectShow(msg.MessageString, MessageType.Information);
                }
                //msgsvr.Show("00010", CultureInfo.CurrentCulture.Name, this._AddMessage, this._Minimum.ToString(), this._Maximum.ToString());

                return false;
            }

            return true;
        }
    }
}