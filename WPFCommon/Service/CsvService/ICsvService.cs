using System.Collections.Generic;

namespace WPFCommon
{
    /// <summary>
    /// CSV操作サービスのインターフェース定義
    /// </summary>
    public interface ICsvService
    {
        /// <summary>
        /// 指定されたCSVファイルを読み込みます。
        /// </summary>
        /// <typeparam name="T">CSVファイルの１行のレイアウト定義クラス</typeparam>
        /// <param name="fullFileName">読み込むCSVファイル名(フルパス)</param>
        /// <returns>読み込んだデータ</returns>
        List<T> Read<T>(string fullFileName);

        /// <summary>
        /// Listの内容を指定されたファイルへ保存します。
        /// </summary>
        /// <typeparam name="T1">List内の行の型</typeparam>
        /// <typeparam name="T2">List内の行の型とCSVファイルでの順序のマッピングを指定するクラス</typeparam>
        /// <param name="fullFileName">出力先のファイル名(フルパス)</param>
        /// <param name="records">CSVに出力するデータ</param>
        void Write<T1, T2>(string fullFileName, List<T1> records)
            where T2 : CsvWriteMapper<T2>;
    }
}
