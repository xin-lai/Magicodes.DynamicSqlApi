using Magicodes.DynamicSqlApi.Core;
using Magicodes.DynamicSqlApi.Core.CodeBuilder;
using Magicodes.DynamicSqlApi.Core.DynamicApis;
using Magicodes.DynamicSqlApi.CsScript;
using Magicodes.DynamicSqlApi.Dapper;
using Magicodes.DynamicSqlApi.Sqlserver;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Magicodes.DynamicSqlApi.All
{
    public static class Extensions
    {
        public static void AddDynamicSqlApi(this IServiceCollection services, string connectionString = null, string sqlMapperFileName = "sqlMapper.xml")
        {
            services.Configure<MvcOptions>(mvcOptions =>
            {
                AddConventions(mvcOptions);
            });

            
            if (!string.IsNullOrWhiteSpace(connectionString))
                services.AddTransient<IDbConnection>(p => new SqlConnection(connectionString));

            services.AddTransient<ICodeBuilder, DefaultCodeBuilder>();
            services.AddTransient<ICodeCompiler, CsScriptCodeCompiler>();
            services.AddTransient<ISqlExecutor, DapperSqlExecutor>();
            services.AddTransient<ITSqlParser, SqlServerTSqlParser>();
        }

        public static void UseDynamicSqlApi(this IApplicationBuilder app)
        {
            ConfigureAspNetCore(app.ApplicationServices);
        }

        private static void ConfigureAspNetCore(IServiceProvider serviceProvider)
        {
            //Add feature providers
            var partManager = serviceProvider.GetService<ApplicationPartManager>();
            
            var codeBuilder = serviceProvider.GetService<ICodeBuilder>();
            var code = codeBuilder.Build();

            var codeCompiler = serviceProvider.GetService<ICodeCompiler>();
            var assembly = codeCompiler.CompileCode(code);
            partManager?.FeatureProviders?.Add(new GenericTypeControllerFeatureProvider(assembly));
        }

        private static void AddConventions(MvcOptions options)
        {
            options.Conventions.Add(new GenericControllerRouteConvention());
        }
    }
}
