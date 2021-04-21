using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace WPFCommon
{
    /// <summary>
    /// ログ出力クラス
    /// </summary>
    public class LoggerService : ILoggerService, IDisposable
    {
        private enum LogKbn
        {
            ERROR,
            INFO,
            TRACE,
        }

        private static IDatabaseService _database = null;
        private static readonly SQLStrings _SQL = null;
        private string _userName = "SYSTEM";
        private string _screenID = "SYSTEM";
        private string _termID = "SSYTEM";

        /// <summary>
        /// クラスへの初回アクセス時に１度だけ実行される静的コンストラクタ
        /// </summary>
        static LoggerService()
        {
            _SQL = new SQLStrings();
            _SQL.AppendLine("INSERT INTO");
            _SQL.AppendLine("    T_LOG");
            _SQL.AppendLine("(");
            _SQL.AppendLine("    SEQ,");
            _SQL.AppendLine("    LOG_KBN,");
            _SQL.AppendLine("    CLASS_NM,");
            _SQL.AppendLine("    METHOD_NM,");
            _SQL.AppendLine("    MESSAGE,");
            _SQL.AppendLine("    INS_DATE,");
            _SQL.AppendLine("    INS_TIME,");
            _SQL.AppendLine("    INS_PROGRAM_ID,");
            _SQL.AppendLine("    INS_USER_NM,");
            _SQL.AppendLine("    INS_TERM_NM,");
            _SQL.AppendLine("    UPD_DATE,");
            _SQL.AppendLine("    UPD_TIME,");
            _SQL.AppendLine("    UPD_PROGRAM_ID,");
            _SQL.AppendLine("    UPD_USER_NM,");
            _SQL.AppendLine("    UPD_TERM_NM");
            _SQL.AppendLine(")");
            _SQL.AppendLine("VALUES");
            _SQL.AppendLine("(");
            _SQL.AppendLine("    T_LOG_SEQ.NEXTVAL,");
            _SQL.AppendLine("    :LOG_KBN,");
            _SQL.AppendLine("    :CLASS_NM,");
            _SQL.AppendLine("    :METHOD_NM,");
            _SQL.AppendLine("    :MESSAGE,");
            _SQL.AppendLine("    TO_CHAR(LOCALTIMESTAMP, 'YYYYMMDD'),");
            _SQL.AppendLine("    TO_CHAR(LOCALTIMESTAMP, 'HH24MISS'),");
            _SQL.AppendLine("    :PROGRAM_ID,");
            _SQL.AppendLine("    :USER_NM,");
            _SQL.AppendLine("    :TERM_NM,");
            _SQL.AppendLine("    TO_CHAR(LOCALTIMESTAMP, 'YYYYMMDD'),");
            _SQL.AppendLine("    TO_CHAR(LOCALTIMESTAMP, 'HH24MISS'),");
            _SQL.AppendLine("    :PROGRAM_ID,");
            _SQL.AppendLine("    :USER_NM,");
            _SQL.AppendLine("    :TERM_NM");
            _SQL.AppendLine(")");
        }

        /// <summary>
        /// クラスのインスタンス作成時に実行されるコンストラクタ
        /// </summary>
        /// <param name="db">データベース操作クラス</param>
        public LoggerService(IDatabaseService db)
        {
            _database = db;
        }

        /// <summary>
        /// クラス内で使用する共通的な値を設定するメソッド
        /// </summary>
        /// <param name="screenID">画面ID</param>
        /// <param name="userName">ユーザ名</param>
        /// <param name="termID">端末ID</param>
        public void SetParameter(string screenID, string userName, string termID)
        {
            this._userName = userName;
            this._screenID = screenID;
            this._termID = termID;
        }

        /// <summary>
        /// インスタンスが破棄されるときに実行されるメソッド
        /// </summary>
        public void Dispose()
        {
            if (_database.IsEmpty() == false)
            {
                _database.Dispose();
            }
        }

        /// <summary>
        /// エラーログ出力メソッド
        /// </summary>
        /// <param name="msg">出力内容</param>
        /// <param name="filePath">呼び出し元ソースファイル名[CallerFilePath]</param>
        /// <param name="memberName">呼び出し元メソッド名[CallerMemberName]</param>
        public void Error(string msg, [CallerFilePath] string filePath = "", [CallerMemberName] string memberName = "")
        {
            this.InsertLog(LogKbn.ERROR, msg, filePath, memberName);
        }

        /// <summary>
        /// 情報ログ出力メソッド
        /// </summary>
        /// <param name="msg">出力内容</param>
        /// <param name="filePath">呼び出し元ソースファイル名[CallerFilePath]</param>
        /// <param name="memberName">呼び出し元メソッド名[CallerMemberName]</param>
        public void Info(string msg, [CallerFilePath] string filePath = "", [CallerMemberName] string memberName = "")
        {
            this.InsertLog(LogKbn.INFO, msg, filePath, memberName);
        }

        /// <summary>
        /// トレースログ出力メソッド
        /// </summary>
        /// <param name="msg">出力内容</param>
        /// <param name="filePath">呼び出し元ソースファイル名[CallerFilePath]</param>
        /// <param name="memberName">呼び出し元メソッド名[CallerMemberName]</param>
        public void Trace(string msg, [CallerFilePath] string filePath = "", [CallerMemberName] string memberName = "")
        {
            this.InsertLog(LogKbn.TRACE, msg, filePath, memberName);
        }

        /// <summary>
        /// ログを出力するメソッド
        /// </summary>
        /// <param name="kbn">ログ区分</param>
        /// <param name="msg">ログに出力する内容</param>
        /// <param name="filePath">呼び出し元ソースファイル名</param>
        /// <param name="memberName">呼び出し元メソッド名</param>
        private void InsertLog(LogKbn kbn, string msg, string filePath, string memberName)
        {
            var className = Path.GetFileNameWithoutExtension(filePath);

            var tran = _database.BeginTransaction();
            var param = new
            {
                LOG_KBN = kbn,
                CLASS_NM = className,
                METHOD_NM = memberName,
                MESSAGE = msg,
                PROGRAM_ID = this._screenID,
                USER_NM = this._userName,
                TERM_NM = this._termID
            };

            _database.Insert(tran, _SQL, param);
            tran.Commit();
            tran.Dispose();
        }
    }
}
