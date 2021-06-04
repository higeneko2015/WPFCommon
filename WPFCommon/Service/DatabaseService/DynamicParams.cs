using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using static Dapper.SqlMapper;

namespace WPFCommon
{
    /// <summary>
    /// ストアドプロシージャの引数を定義するクラス
    /// </summary>
    public class DynamicParams : IDynamicParameters
    {
        /// <summary>
        /// 引数の一覧をため込んでおくための変数です。
        /// </summary>
        private readonly Dictionary<string, OracleParams> _params = new Dictionary<string, OracleParams>();

        /// <summary>
        /// AddInParam等で受け取った引数の情報を格納しておくクラス
        /// </summary>
        private class OracleParams
        {
            /// <summary>
            /// パラメータ名
            /// </summary>
            public string Name { get; set; } = null;

            /// <summary>
            /// パラメータに渡す値
            /// </summary>
            public object Value { get; set; } = null;

            /// <summary>
            /// パラメータがINなのかOUTなのかを指定する列挙型値
            /// </summary>
            public ParameterDirection? Direction { get; set; } = null;

            /// <summary>
            /// 引数の型
            /// </summary>
            public DbType? DbType { get; set; } = null;

            /// <summary>
            /// 引数が配列かどうかを判定する列挙型値
            /// </summary>
            public OracleCollectionType? CollectionType { get; set; } = null;

            /// <summary>
            /// 引数が配列だった場合の１要素の桁数
            /// </summary>
            public int? Size { get; set; } = null;

            /// <summary>
            /// 引数が配列だった場合の配列の要素数。
            /// </summary>
            public int? ArrayCount { get; set; } = null;

            /// <summary>
            /// OracleCommandに設定したパラメータの情報。
            /// プロシージャ実行によってOUTパラメータにセット
            /// された戻り値を取得するために使用します。
            /// </summary>
            public IDbDataParameter AttachedParam { get; set; } = null;
        }

        /// <summary>
        /// プロシージャに渡す引数を追加します
        /// </summary>
        /// <param name="name">パラメータ名</param>
        /// <param name="value">パラメータ値</param>
        /// <param name="dbType">パラメータの型</param>
        public void AddInParam(string name, object value, DbType dbType)
        {
            this._params[name] =
                new OracleParams()
                {
                    Name = name,
                    Value = value,
                    Direction = ParameterDirection.Input,
                    DbType = dbType,
                };
        }

        /// <summary>
        /// プロシージャに渡す引数を追加します
        /// </summary>
        /// <typeparam name="T">パラメータの型</typeparam>
        /// <param name="name">パラメータ名</param>
        /// <param name="value">パラメータ値</param>
        /// <param name="dbType">パラメータの型</param>
        public void AddInArrayParam<T>(string name, List<T> value, DbType dbType)
        {
            this._params[name] =
                new OracleParams()
                {
                    Name = name,
                    Value = value.ToArray(),
                    Direction = ParameterDirection.Input,
                    DbType = dbType,
                    Size = value.Count,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray,
                    ArrayCount = value.Count,
                };
        }

        /// <summary>
        /// プロシージャから受け取る引数を追加します
        /// </summary>
        /// <param name="name">パラメータ名</param>
        /// <param name="dbType">パラメータの型</param>
        public void AddOutParam(string name, DbType dbType)
        {
            this._params[name] =
                new OracleParams()
                {
                    Name = name,
                    Direction = ParameterDirection.Output,
                    DbType = dbType,
                };
        }

        /// <summary>
        /// プロシージャから受け取る引数を追加します
        /// </summary>
        /// <param name="name">パラメータ名</param>
        /// <param name="dbType">パラメータの型</param>
        /// <param name="paramSize">戻り値(配列)用バッファの１要素の桁数</param>
        /// <param name="paramCount">戻り値(配列)用バッファの要素数</param>
        public void AddOutArrayParam(string name, DbType dbType, int paramSize, int paramCount)
        {
            this._params[name] =
                new OracleParams()
                {
                    Name = name,
                    Direction = ParameterDirection.Output,
                    DbType = dbType,
                    Size = paramSize,
                    CollectionType = OracleCollectionType.PLSQLAssociativeArray,
                    ArrayCount = paramCount,
                };
        }

        /// <summary>
        /// DapperのExecue実行時に自動的に呼ばれるメソッドです。
        /// AddInParamなどで追加したパラメータ情報を
        /// OracleCommandのパラメータに変換してセットします。
        /// </summary>
        /// <param name="command">Oracleコマンド</param>
        /// <param name="identity">用途不明。未使用</param>
        public void AddParameters(IDbCommand command, Identity identity)
        {
            foreach (var item in this._params.Values)
            {
                var param = ((OracleCommand)command).CreateParameter();
                param.ParameterName = item.Name;
                param.Value = item.Value;
                param.Direction = item.Direction.Value;
                param.DbType = item.DbType.Value;

                if (item.Size.HasValue)
                {
                    param.Size = item.Size.Value;
                }

                if (item.CollectionType.HasValue)
                {
                    param.CollectionType = item.CollectionType.Value;
                }

                if (item.ArrayCount.HasValue)
                {
                    param.ArrayBindSize = Enumerable.Repeat(item.Size.Value, item.ArrayCount.Value).ToArray();
                }

                command.Parameters.Add(param);
                item.AttachedParam = param;
            }
        }

        /// <summary>
        /// プロシージャのOUTパラメータの値を取得します。
        /// </summary>
        /// <typeparam name="T">OUTパラメータの型</typeparam>
        /// <param name="name">取得対象のOUTパラメータ名</param>
        /// <returns>取得したパラメータ値</returns>
        public List<T> GetOutParamValues<T>(string name)
        {
            var val = this._params[name].AttachedParam.Value;
            if (!(val is T[] ret))
            {
                return new List<T>();
            }

            return ret.ToList();
        }
    }
}
