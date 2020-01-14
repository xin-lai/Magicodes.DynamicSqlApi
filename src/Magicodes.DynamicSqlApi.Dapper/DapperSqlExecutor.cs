using Dapper;
using Magicodes.DynamicSqlApi.Core;
using System;
using System.Collections.Generic;
using System.Data;
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


        public IEnumerable<T> Query<T>(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null) => DbConnection.Query<T>(sql, param, transaction, buffered, commandTimeout, commandType);


        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null) => await DbConnection.QueryAsync<T>(sql, param, transaction, commandTimeout, commandType);
    }
}
