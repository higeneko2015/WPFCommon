using System.Collections.Generic;
using System.Data;

namespace WPFCommon
{
    /// <summary>
    /// データベースサービスのインターフェース定義
    /// </summary>
    public interface IDatabaseService
    {
        /// <summary>
        /// データベースに対してSELECTを発行します。
        /// </summary>
        /// <typeparam name="T">SELECTした項目用のDTO</typeparam>
        /// <param name="sql">発行対象のSQL文</param>
        /// <param name="param">SQLのパラメータ</param>
        /// <returns>
        /// SELECTされた結果をListとして返却します。
        /// SELECT結果が0件の場合でもListを返却します。
        /// SELECT結果が0件の場合は例外は発生しません。
        /// </returns>
        List<T> Select<T>(SQLStrings sql, object param = null)
            where T : class;

        /// <summary>
        /// データベースに対してSELECTを発行します。
        /// </summary>
        /// <param name="sql">発行対象のSQL文</param>
        /// <param name="param">SQLのパラメータ</param>
        /// <returns>
        /// SELECTされた結果をListとして返却します。
        /// SELECT結果が0件の場合でもListを返却します。
        /// SELECT結果が0件の場合は例外は発生しません。
        /// </returns>
        List<dynamic> Select(SQLStrings sql, object param = null);

        /// <summary>
        /// データベースに対してINSERTを発行します。
        /// </summary>
        /// <param name="trans">SQLを発行するトランザクション</param>
        /// <param name="sql">発行対象のSQL文</param>
        /// <param name="param">SQLのパラメータ</param>
        /// <returns>
        /// INSERTした行数を返却します。
        /// 自動COMMIT、自動ROLLBACKはされませんので呼び出し側で対応する必要があります。
        /// </returns>
        int Insert(IDbTransaction trans, SQLStrings sql, object param = null);

        /// <summary>
        /// データベースに対してUPDATEを発行します。
        /// </summary>
        /// <param name="trans">SQLを発行するトランザクション</param>
        /// <param name="sql">発行対象のSQL文</param>
        /// <param name="param">SQLのパラメータ</param>
        /// <returns>
        /// UPDATEした行数を返却します。
        /// UPDATE件数が0件の場合は例外は発生しません。
        /// 自動COMMIT、自動ROLLBACKはされませんので呼び出し側で対応する必要があります。
        /// </returns>
        int Update(IDbTransaction trans, SQLStrings sql, object param = null);

        /// <summary>
        /// データベースに対してDELETEを発行します。
        /// </summary>
        /// <param name="trans">SQLを発行するトランザクション</param>
        /// <param name="sql">発行対象のSQL文</param>
        /// <param name="param">SQLのパラメータ</param>
        /// <returns>
        /// DELETEした行数を返却します。
        /// DELETE件数が0件の場合は例外は発生しません。
        /// 自動COMMIT、自動ROLLBACKはされませんので呼び出し側で対応する必要があります。
        /// </returns>
        int Delete(IDbTransaction trans, SQLStrings sql, object param = null);

        /// <summary>
        /// データベース内にストアされているプロシージャを実行します。
        /// </summary>
        /// <param name="trans">トランザクション</param>
        /// <param name="procName">実行対象のプロシージャ名</param>
        /// <param name="param">プロシージャに渡す引数</param>
        /// <returns>
        /// EXECUTEの結果を返却します。
        /// 実行したプロシージャ内でCOMMITしていない限り
        /// 自動COMMIT、自動ROLLBACKはされませんので呼び出し側で対応する必要があります。
        /// </returns>
        int ExecProcedure(IDbTransaction trans, string procName, DynamicParams param = null);

        /// <summary>
        /// オブジェクト破棄時にコネクションを解放します。
        /// </summary>
        void Dispose();

        /// <summary>
        /// SELECT以外のDB操作で必要なトランザクションを生成します。
        /// </summary>
        /// <returns>生成されたトランザクション。</returns>
        IDbTransaction BeginTransaction();
    }
}
