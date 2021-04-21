using CsvHelper.Configuration.Attributes;

namespace WPFCommon
{
    /// <summary>
    /// CSVの列とクラスのプロパティをマッピングするための属性
    /// </summary>
    public class CsvIndexAttribute : IndexAttribute
    {
        /// <summary>
        /// インスタンス作成時に実行されるコンストラクタ
        /// </summary>
        /// <param name="idx1">インデックス値１</param>
        public CsvIndexAttribute(int idx1)
            : base(idx1, -1)
        {
        }
    }
}
