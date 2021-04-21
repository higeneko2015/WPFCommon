using System.Windows;
using static WPFCommon.ShowMessageService;

namespace WPFCommon
{
    public interface IShowMessageService
    {
        MessageBoxResult DirectShow(string msg);

        MessageBoxResult DirectShow(string msg, MessageType msgType);

        MessageBoxResult DirectShow(string msg, MessageBoxButton btnType);

        MessageBoxResult DirectShow(string msg, MessageType msgType, MessageBoxButton btnType);

        MessageBoxResult Show(string msgID, string langCD, params string[] addStrings);
    }
}