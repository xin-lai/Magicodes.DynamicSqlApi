using Magicodes.DynamicSqlApi.Core;
using Magicodes.DynamicSqlApi.Core.CodeBuilder;
using Magicodes.DynamicSqlApi.Core.DynamicApis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        public static void AddDynamicSqlApi<TCodeBuilder, TCodeCompiler, TSqlExecutor, TTSqlParser>(this IServiceCollection services, string connectionString = null, string sqlMapperFileName = "sqlMapper.xml")
            where TCodeBuilder : class, ICodeBuilder
            where TCodeCompiler : class, ICodeCompiler
            where TSqlExecutor : class, ISqlExecutor
            where TTSqlParser : class, ITSqlParser
        {
            services.Configure<MvcOptions>(mvcOptions =>
            {
                AddConventions(mvcOptions);
            });


            if (!string.IsNullOrWhiteSpace(connectionString))
                services.AddTransient<IDbConnection>(p => new SqlConnection(connectionString));

            services.AddTransient<ICodeBuilder, TCodeBuilder>();
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
