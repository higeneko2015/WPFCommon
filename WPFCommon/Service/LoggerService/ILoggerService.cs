namespace WPFCommon
{
    /// <summary>
    /// ログ出力クラスのインターフェース
    /// </summary>
    public interface ILoggerService
    {
        /// <summary>
        /// クラス内で使用する共通的な値を設定するメソッド
        /// </summary>
        /// <param name="screenID">画面ID</param>
        /// <param name="userName">ユーザ名</param>
        /// <param name="termID">端末ID</param>
        void SetParameter(string screenID, string userName, string termID);

        /// <summary>
        /// エラーログ出力メソッド
        /// </summary>
        /// <param name="msg">出力内容</param>
        /// <param name="filePath">呼び出し元ソースファイル名[CallerFilePath]</param>
        /// <param name="memberName">呼び出し元メソッド名[CallerMemberName]</param>
        void Error(string msg, string filePath, string memberName);

        /// <summary>
        /// 情報ログ出力メソッド
        /// </summary>
        /// <param name="msg">出力内容</param>
        /// <param name="filePath">呼び出し元ソースファイル名[CallerFilePath]</param>
        /// <param name="memberName">呼び出し元メソッド名[CallerMemberName]</param>
        void Info(string msg, string filePath, string memberName);

        /// <summary>
        /// トレースログ出力メソッド
        /// </summary>
        /// <param name="msg">出力内容</param>
        /// <param name="filePath">呼び出し元ソースファイル名[CallerFilePath]</param>
        /// <param name="memberName">呼び出し元メソッド名[CallerMemberName]</param>
        void Trace(string msg, string filePath, string memberName);
    }
}
