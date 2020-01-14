using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Magicodes.DynamicSqlApi.All;
using Magicodes.DynamicSqlApi.Core;
using Magicodes.SwaggerUI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Web2_2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddAllDynamicSqlApi(Configuration["ConnectionStrings:Default"]);

            //添加自定义API文档生成(支持文档配置)
            services.AddCustomSwaggerGen(Configuration);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseDynamicSqlApi();

            app.UseMvc(routes => routes.MapRoute(
                "default",
                "{controller=Home}/{action=Index}/{id?}"));

            //启用自定义API文档(支持文档配置)
            app.UseCustomSwaggerUI(Configuration);

        }
    }
}
