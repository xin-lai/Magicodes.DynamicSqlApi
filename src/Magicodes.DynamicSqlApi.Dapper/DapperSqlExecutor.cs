using Dapper;
using Magicodes.DynamicSqlApi.Core;
using Magicodes.DynamicSqlApi.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Magicodes.DynamicSqlApi.Dapper
{
    /// <summary>
    /// Dapper实现
    /// </summary>
    public class DapperSqlExecutor : ISqlExecutor
    {
        public DapperSqlExecutor(IDbConnection dbConnection)
        {
            DbConnection = dbConnection;
        }

        public IDbConnection DbConnection { get; }

        public async Task<int> ExecuteAsync(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, string connectionString = null)
        {
            if (!connectionString.IsNullOrWhiteSpace())
            {
                return await new SqlConnection(connectionString).ExecuteAsync(sql, param, transaction, commandTimeout, commandType);
            }
            return await DbConnection.ExecuteAsync(sql, param, transaction, commandTimeout, commandType);
        }

        public IEnumerable<T> Query<T>(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null, string connectionString = null)
        {
            if (!connectionString.IsNullOrWhiteSpace())
            {
                return new SqlConnection(connectionString).Query<T>(sql, param, transaction, buffered, commandTimeout, commandType);
            }
            return DbConnection.Query<T>(sql, param, transaction, buffered, commandTimeout, commandType);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, string connectionString = null)
        {
            if (!connectionString.IsNullOrWhiteSpace())
            {
                return await new SqlConnection(connectionString).QueryAsync<T>(sql, param, transaction, commandTimeout, commandType);
            }
            return await DbConnection.QueryAsync<T>(sql, param, transaction, commandTimeout, commandType);
        }


    }
}
