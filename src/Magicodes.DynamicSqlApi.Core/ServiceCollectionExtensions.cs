using Magicodes.DynamicSqlApi.Core;
using Magicodes.DynamicSqlApi.Core.CodeBuilder;
using Magicodes.DynamicSqlApi.Core.DynamicApis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Magicodes.DynamicSqlApi.Core
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString"></param>
        /// <param name="sqlMapperFileName"></param>
        public static void AddDynamicSqlApi<TCodeBuilder, TCodeCompiler, TSqlExecutor, TTSqlParser>(this IServiceCollection services, string connectionString, string sqlMapperFileName = "sqlMapper.xml")
            where TCodeBuilder : CodeBuilderBase 
            where TCodeCompiler : class, ICodeCompiler
            where TSqlExecutor : class, ISqlExecutor
            where TTSqlParser : class, ITSqlParser
        {
            if (!string.IsNullOrWhiteSpace(connectionString))
                services.AddTransient<IDbConnection>(p => new SqlConnection(connectionString));
            else
            {
                throw new ApplicationException("请配置连接字符串！");
            }
            services.AddTransient<CodeBuilderBase, TCodeBuilder>();
            services.AddTransient<ICodeCompiler, TCodeCompiler>();
            services.AddTransient<ISqlExecutor, TSqlExecutor>();
            services.AddTransient<ITSqlParser, TTSqlParser>();
        }

        public static void UseDynamicSqlApi(this IApplicationBuilder app)
        {
            ConfigureAspNetCore(app.ApplicationServices);
        }

        public static void ConfigureAspNetCore(IServiceProvider serviceProvider)
        {
            var partManager = serviceProvider.GetService<ApplicationPartManager>();
            
            var codeBuilder = serviceProvider.GetService<CodeBuilderBase>();
            var code = codeBuilder.Build();
            if (string.IsNullOrWhiteSpace(code)) return;

            var logger = serviceProvider.GetService<ILogger>();
            if (logger!=null && logger.IsEnabled(LogLevel.Debug))
                logger.LogDebug(code);

            var codeCompiler = serviceProvider.GetService<ICodeCompiler>();
            var assembly = codeCompiler.CompileCode(code);
            partManager?.FeatureProviders?.Add(new GenericTypeControllerFeatureProvider(assembly));
        }
    }
}
