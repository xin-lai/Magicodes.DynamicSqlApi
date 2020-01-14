using Magicodes.DynamicSqlApi.Core;
using Magicodes.DynamicSqlApi.Core.CodeBuilder;
using Magicodes.DynamicSqlApi.Core.DynamicApis;
using Magicodes.DynamicSqlApi.CsScript;
using Magicodes.DynamicSqlApi.Dapper;
using Magicodes.DynamicSqlApi.Sqlserver;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Magicodes.DynamicSqlApi.All
{
    public static class Extensions
    {
        public static void AddAllDynamicSqlApi(this IServiceCollection services, string connectionString, string sqlMapperFileName = "sqlMapper.xml")
        {
            services.AddDynamicSqlApi<DefaultCodeBuilder, CsScriptCodeCompiler, DapperSqlExecutor, SqlServerTSqlParser>(connectionString, sqlMapperFileName);
        }
    }
}
