using System.Text;

namespace WPFCommon
{
    /// <summary>
    /// SQL文を構成するときに使用する文字列クラス。
    /// 内部的にStringBuilderで管理をしています。
    /// </summary>
    public class SQLStrings
    {
        /// <summary>
        /// SQL文字列を管理するバッファ
        /// </summary>
        private readonly StringBuilder _buffer = new();

        /// <summary>
        /// 引数で渡された文字列を連結します。
        /// </summary>
        /// <param name="param">連結対象する文字列</param>
        /// <returns>連結結果のSQLStringsクラスのインスタンス</returns>
        public SQLStrings AppendLine(string param)
        {
            this._buffer.AppendLine(param);
            return this;
        }

        /// <summary>
        /// 文字列をクリアします。
        /// </summary>
        public void Clear()
        {
            this._buffer.Clear();
        }

        /// <summary>
        /// 連結された文字列をstring型に変換して返却します。
        /// </summary>
        /// <returns>string型に変換された文字列</returns>
        public override string ToString()
        {
            return this._buffer.ToString();
        }
    }
}
