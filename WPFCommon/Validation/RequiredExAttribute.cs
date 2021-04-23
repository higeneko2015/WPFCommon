using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using static WPFCommon.ShowMessageService;

namespace WPFCommon
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredExAttribute : ValidationAttribute
    {
        private readonly string _AddMessage = string.Empty;

        public RequiredExAttribute(string addMessage)
        {
            this._AddMessage = addMessage;
        }

        public override bool IsValid(object value)
        {
            if (value.IsEmpty())
            {
                var svr = ApplicationEx.GetService<IMessageService>();
                var msg = svr.Get("00008", CultureInfo.CurrentCulture.Name, this._AddMessage);
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
                //msgsvr.Show("00008", CultureInfo.CurrentCulture.Name, this._AddMessage);

                return false;
            }
            else
            {
                return true;
            }
        }
    }
}