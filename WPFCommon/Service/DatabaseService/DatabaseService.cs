using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Oracle.ManagedDataAccess.Client;

namespace WPFCommon
{
    /// <summary>
    /// データベース操作サービスクラス
    /// </summary>
    public class DatabaseService : IDatabaseService, IDisposable
    {
        private readonly Task _ConnectionTask = null;

        /// <summary>
        /// DBコネクション
        /// </summary>
        private OracleConnection _conn = null;

        /// <summary>
        /// クラスのインスタンス作成時に実行されるコンストラクタ
        /// </summary>
        /// <param name="connString">データベース接続文字列</param>
        public DatabaseService(string connString)
        {
            _ConnectionTask = Task.Run(() =>
            {
                this._conn = new OracleConnection(connString);
                this._conn.Open();
            });
        }

        public DatabaseService(IConfigService config)
        {
            _ConnectionTask = Task.Run(() =>
            {
                this._conn = new OracleConnection(config.ConnectionStrings);
                this._conn.Open();
            });
        }

        /// <inheritdoc/>
        public IDbTransaction BeginTransaction()
        {
            return this._conn.BeginTransaction();
        }

        /// <inheritdoc/>
        public int Delete(IDbTransaction trans, SQLStrings sql, object param = null)
        {
            try
            {
                return this._conn.Execute(sql.ToString(), param, trans);
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (this._conn.IsEmpty())
            {
                return;
            }

            if (this._conn.State == ConnectionState.Open)
            {
                this._conn.Close();
            }

            this._conn.Dispose();
            this._conn = null;
        }

        /// <inheritdoc/>
        public int ExecProcedure(IDbTransaction trans, string procName, DynamicParams param = null)
        {
            try
            {
                return this._conn.Execute(procName, param, trans, commandType: CommandType.StoredProcedure);
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
        }

        /// <inheritdoc/>
        public int Insert(IDbTransaction trans, SQLStrings sql, object param = null)
        {
            try
            {
                return this._conn.Execute(sql.ToString(), param, trans);
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
        }

        /// <inheritdoc/>
        public List<T> Select<T>(SQLStrings sql, object param = null)
            where T : class
        {
            this._ConnectionTask.Wait();

            var retValue = this._conn.Query<T>(sql.ToString(), param);
            if (retValue.IsEmpty() == false)
            {
                return retValue.ToList();
            }
            else
            {
                return new List<T>();
            }
        }

        /// <inheritdoc/>
        public List<dynamic> Select(SQLStrings sql, object param = null)
        {
            this._ConnectionTask.Wait();

            var retValue = this._conn.Query(sql.ToString(), param);
            if (retValue.IsEmpty() == false)
            {
                return retValue.ToList();
            }
            else
            {
                return new List<dynamic>();
            }
        }

        /// <inheritdoc/>
        public int Update(IDbTransaction trans, SQLStrings sql, object param = null)
        {
            try
            {
                return this._conn.Execute(sql.ToString(), param, trans);
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
        }
    }
}