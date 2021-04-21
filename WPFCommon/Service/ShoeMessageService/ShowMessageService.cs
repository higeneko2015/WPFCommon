using System;
using System.Linq;
using System.Windows;

namespace WPFCommon
{
    public class ShowMessageService : IShowMessageService
    {
        private readonly IMessageService _Messaging = null;

        public ShowMessageService(IMessageService message)
        {
            this._Messaging = message;
        }

        /// <summary>
        /// メッセージ種別
        /// </summary>
        public enum MessageType
        {
            /// <summary>
            /// 情報
            /// </summary>
            Information = 1,

            /// <summary>
            /// 警告
            /// </summary>
            Warning = 2,

            /// <summary>
            /// エラー
            /// </summary>
            Error = 3,

            /// <summary>
            /// 問い合わせ
            /// </summary>
            Question = 4,
        }

        public MessageBoxResult DirectShow(string msg)
        {
            return DirectShow(msg, MessageType.Information, MessageBoxButton.OK);
        }

        public MessageBoxResult DirectShow(string msg, MessageType msgType)
        {
            return DirectShow(msg, msgType, MessageBoxButton.OK);
        }

        public MessageBoxResult DirectShow(string msg, MessageBoxButton btnType)
        {
            return DirectShow(msg, MessageType.Information, btnType);
        }

        /// <summary>
        /// 指定された文字をメッセージボックスに表示します。
        /// </summary>
        /// <param name="msg">メッセージ文字</param>
        /// <returns>メッセージボックスからの戻り値</returns>
        public MessageBoxResult DirectShow(string msg, MessageType msgType, MessageBoxButton btnType)
        {
            // MessageTypeからアイコンイメージを決定する
            var imageType = GetMessageBoxType(msgType);

            return ShowMessageBox(msg, string.Empty, imageType, btnType);
        }

        /// <summary>
        /// 指定されたメッセージIDの内容でメッセージボックスを表示します。
        /// </summary>
        /// <param name="msgID">メッセージID</param>
        /// <param name="addStrings">メッセージ追加文字</param>
        /// <returns>メッセージボックスからの戻り値</returns>
        public MessageBoxResult Show(string msgID, string langCD, params string[] addStrings)
        {
            var msg = this._Messaging.Get(msgID, langCD, addStrings);

            var msgType = (MessageType)Enum.ToObject(typeof(MessageType), int.Parse(msg.MessageType));

            var msgImageType = GetMessageBoxType(msgType);

            MessageBoxButton msgButtonType;

            switch (msgType)
            {
                case MessageType.Error:
                    msgButtonType = MessageBoxButton.OK;
                    break;

                case MessageType.Information:
                    msgButtonType = MessageBoxButton.OK;
                    break;

                case MessageType.Question:
                    msgButtonType = MessageBoxButton.YesNo;
                    break;

                case MessageType.Warning:
                    msgButtonType = MessageBoxButton.OK;
                    break;

                default:
                    msgButtonType = MessageBoxButton.OK;
                    break;
            }

            return ShowMessageBox(msg.MessageString, msgID, msgImageType, msgButtonType);
        }

        private static MessageBoxImage GetMessageBoxType(MessageType type)
        {
            switch (type)
            {
                case MessageType.Error:
                    return MessageBoxImage.Error;

                case MessageType.Information:
                    return MessageBoxImage.Information;

                case MessageType.Question:
                    return MessageBoxImage.Question;

                case MessageType.Warning:
                    return MessageBoxImage.Warning;

                default:
                    return MessageBoxImage.Information;
            }
        }

        private static MessageBoxResult ShowMessageBox(string msg, string msgID, MessageBoxImage msgImageType, MessageBoxButton msgButtonType)
        {
            // Commandから呼び出されたとき、Commandの実体はUIスレッドとは
            // 別のスレッドで実行されている場合がある。
            // UIスレッド以外からWindowの表示等を行うことはできないため
            // UIスレッドのDispatcherを経由してWindowを表示する。
            return Application.Current.Dispatcher.Invoke(() =>
            {
                var owner = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);

                var msgWindow = new MessageWindow(owner, msg, msgID, msgImageType, msgButtonType);

                if (msgWindow.Owner.IsEmpty())
                {
                    msgWindow.Owner = Application.Current.MainWindow;
                    msgWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                }

                msgWindow.ShowDialog();
                return msgWindow.Result;
            });
        }
    }
}