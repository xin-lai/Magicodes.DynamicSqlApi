using Magicodes.DynamicSqlApi.Core;
using Magicodes.DynamicSqlApi.Core.Models;
using Magicodes.DynamicSqlApi.SqlServer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Magicodes.DynamicSqlApi.Sqlserver
{
    /// <summary>
    /// 解析SQL Server语句
    /// </summary>
    public class SqlServerTSqlParser : ITSqlParser
    {
        public SqlServerTSqlParser(ISqlExecutor sqlExecutor)
        {
            SqlExecutor = sqlExecutor;
        }

        public ISqlExecutor SqlExecutor { get; }

        /// <summary>
        /// https://docs.microsoft.com/zh-cn/sql/relational-databases/system-stored-procedures/sp-describe-undeclared-parameters-transact-sql?view=sql-server-ver15
        /// </summary>
        /// <param name="sqlText"></param>
        /// <returns></returns>
        public IEnumerable<TSqlParameterInfo> GetParameters(string sqlText) => SqlExecutor.Query<GetSqlServerParametersOutput>("sp_describe_undeclared_parameters", new
        {
            tsql = sqlText
        }, commandType: CommandType.StoredProcedure).Select(p => new TSqlParameterInfo()
        {
            CsTypeName = p.suggested_system_type_name.GetCsTypeByDbType(),
            Name = p.name
        });


        /// <summary>
        /// https://docs.microsoft.com/zh-cn/sql/relational-databases/system-stored-procedures/sp-describe-first-result-set-transact-sql?view=sql-server-ver15
        /// </summary>
        /// <param name="sqlText"></param>
        /// <returns></returns>
        public IEnumerable<TSqlOutputFieldInfo> GetOutputFieldList(string sqlText)
         => SqlExecutor.Query<GetSqlServerOutputListDto>("sp_describe_first_result_set", new
         {
             tsql = sqlText,
             @params = string.Empty,
             browse_information_mode = 1
         }, commandType: CommandType.StoredProcedure)
            //不输出隐藏列
            .Where(p => !p.is_hidden)
            .Select(p => new TSqlOutputFieldInfo()
            {
                CsTypeName = p.system_type_name.GetCsTypeByDbType(),
                Name = p.name,
                AllowNullable = p.is_nullable,
                SqlTypeName = p.system_type_name
            });
    }
}
