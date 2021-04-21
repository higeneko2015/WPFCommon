using CsvHelper.Configuration;

namespace WPFCommon
{
    /// <summary>
    /// CSVとクラスのマッピングを定義するクラス
    /// </summary>
    /// <typeparam name="T">マッピング定義クラス</typeparam>
    public class CsvWriteMapper<T> : ClassMap<T>
    {
    }
}
