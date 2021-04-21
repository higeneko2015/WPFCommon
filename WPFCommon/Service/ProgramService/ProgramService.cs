using System.Diagnostics;

namespace WPFCommon
{
    /// <summary>
    /// プログラム情報サービスクラス
    /// </summary>
    public class ProgramService : IProgramService
    {
        /// <summary>
        /// クラスのインスタンス作成時に実行されるコンストラクタ
        /// </summary>
        public ProgramService()
        {
            var sql = new SQLStrings();
            sql.AppendLine("SELECT");
            sql.AppendLine("    PROGRAM_ID,");
            sql.AppendLine("    PROGRAM_NM,");
            sql.AppendLine("    ASSEMBLY_NM");
            sql.AppendLine("FROM");
            sql.AppendLine("    M_PROGRAM");
            sql.AppendLine("WHERE");
            sql.AppendLine("    ASSEMBLY_NM = :ASSEMBLY_NM");

            var exeName = Process.GetCurrentProcess().MainModule?.ModuleName;
            var param = new { ASSEMBLY_NM = exeName };

            // TODO
            //var ret = Database.Select(sql, param);
            //if (ret.Count == 0)
            //{
            //    return;
            //}

            //var first = ret.FirstOrDefault();
            //this.ProgramID = first.PROGRAM_ID;
            //this.ProgramName = first.PROGRAM_NM;
            //this.AssemblyName = first.ASSEMBLY_NM;
        }

        /// <inheritdoc/>
        public string AssemblyName { get; }

        /// <inheritdoc/>
        public string ProgramID { get; }

        /// <inheritdoc/>
        public string ProgramName { get; }
    }
}
