namespace WPFCommon
{
    /// <summary>
    /// ファイル選択/保存ダイアログサービスのインターフェース
    /// </summary>
    public interface IFileDialogService
    {
        /// <summary>
        /// 「名前をつけてファイルを保存」ダイアログを表示します。
        /// </summary>
        /// <param name="filter">ファイル拡張子のフィルタ文字列</param>
        /// <param name="defaultPath">初期表示フォルダパス</param>
        /// <param name="defaultFileName">初期選択ファイル名</param>
        /// <returns>ファイルを保存した場合：保存したファイルのフルパス
        /// ファイルを保存しなかった場合：string.Empty
        /// </returns>
        string Save(string filter, string defaultPath = null, string defaultFileName = null);

        /// <summary>
        /// 「ファイルを開く」ダイアログを表示します。
        /// </summary>
        /// <param name="filter">ファイル拡張子のフィルタ文字列</param>
        /// <param name="defaultPath">初期表示フォルダパス</param>
        /// <returns>ファイルを選択した場合：選択したファイルのフルパス
        /// ファイルを選択しなかった場合：string.Empty</returns>
        string Load(string filter, string defaultPath = null);
    }
}
