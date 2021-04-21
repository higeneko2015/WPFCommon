using System;
using System.Data;
using System.Runtime.CompilerServices;

namespace WPFCommon
{
    /// <summary>
    /// ログ出力クラス
    /// </summary>
    public static class Logger
    {
        private static readonly ILoggerService _Logger = null;

        /// <summary>
        /// ログ出力機能が有効になっているかの判定値を取得または設定します。
        /// </summary>
        public static ConnectionState State { get; private set; }

        /// <summary>
        /// クラスへの初回アクセス時に１度だけ実行される静的コンストラクタ
        /// </summary>
        static Logger()
        {
            try
            {
                // TODO
                //_Logger = ServiceManager.Get<ILoggerService>();
                State = ConnectionState.Open;
            }
            catch (Exception)
            {
                State = ConnectionState.Closed;
            }
        }

        /// <inheritdoc/>
        public static void Error(string msg, [CallerFilePath] string filePath = "", [CallerMemberName] string memberName = "")
        {
            _Logger?.Error(msg, filePath, memberName);
        }

        /// <inheritdoc/>
        public static void Info(string msg, [CallerFilePath] string filePath = "", [CallerMemberName] string memberName = "")
        {
            _Logger?.Info(msg, filePath, memberName);
        }

        /// <inheritdoc/>
        public static void SetParameter(string screenID, string userName, string termID)
        {
            _Logger?.SetParameter(screenID, userName, termID);
        }

        /// <inheritdoc/>
        public static void Trace(string msg, [CallerFilePath] string filePath = "", [CallerMemberName] string memberName = "")
        {
            _Logger?.Trace(msg, filePath, memberName);
        }
    }
}