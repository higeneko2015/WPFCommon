namespace WPFCommon
{
    /// <summary>
    /// プログラムサービスのインターフェース
    /// </summary>
    public interface IProgramService
    {
        /// <summary>
        /// プログラムIDを取得します。
        /// </summary>
        string ProgramID { get; }

        /// <summary>
        /// プログラム名を取得します。
        /// </summary>
        string ProgramName { get; }

        /// <summary>
        /// アセンブリ名を取得します。
        /// </summary>
        string AssemblyName { get; }
    }
}
