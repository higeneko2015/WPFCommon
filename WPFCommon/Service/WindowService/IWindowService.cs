namespace WPFCommon
{
    /// <summary>
    /// Window表示クラスのインターフェース
    /// </summary>
    public interface IWindowService
    {
        /// <summary>
        /// 表示対象のWindowの情報を追加します。
        /// </summary>
        /// <param name="windowName">追加するWindow名</param>
        void Add(string windowName);

        /// <summary>
        /// 表示対象のWindowの情報を追加します。
        /// </summary>
        /// <param name="windowName">追加するWindow名</param>
        /// <param name="assmblyName">Windowが定義されているアセンブリ名</param>
        void Add(string windowName, string assmblyName);

        /// <summary>
        /// 指定されたWindowを表示します。
        /// </summary>
        /// <param name="windowName">表示するWindowの名前</param>
        /// <param name="param">Windowに渡す起動引数</param>
        void Show(string windowName, object[] param = null);

        /// <summary>
        /// 指定されたWindowをPopup表示します。
        /// </summary>
        /// <param name="windowName">表示するWindowの名前</param>
        /// <param name="param">Windowに渡す起動引数</param>
        /// <returns>Windowからの戻り値</returns>
        bool? ShowDialog(string windowName, object[] param = null);
    }
}
