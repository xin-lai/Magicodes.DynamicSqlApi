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
        public static void AddDynamicSqlApi(this IServiceCollection services, string connectionString = null, string sqlMapperFileName = "sqlMapper.xml")
        {
            services.AddDynamicSqlApi<DefaultCodeBuilder, CsScriptCodeCompiler, DapperSqlExecutor, SqlServerTSqlParser>(connectionString, sqlMapperFileName);
        }

        public static void UseDynamicSqlApi(this IApplicationBuilder app)
        {
            app.UseDynamicSqlApi();
        }
    }
}
