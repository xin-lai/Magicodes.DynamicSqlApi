using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Magicodes.DynamicSqlApi.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Magicodes.DynamicSqlApi.All;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Web3_1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddAllDynamicSqlApi(Configuration["ConnectionStrings:Default"]);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Magicodes.DynamicSqlApi APIÎÄµµ", Version = "v1" });
                // Set the comments path for the Swagger JSON and UI.
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);

                //c.DocumentFilter<TestFilter>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDynamicSqlApi();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            foreach (var item in Configuration.GetSection("SqlApis").AsEnumerable())
            {
                Console.WriteLine(item.Key + ":" + item.Value);
            }
        }
    }

    //public class TestFilter : IDocumentFilter
    //{
    //    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    //    {
    //        foreach (var item in swaggerDoc.Paths)
    //        {
    //            Console.WriteLine(item.Key + "----" + item.Value);
    //        }
    //        if (swaggerDoc.Paths.Any(p => p.Key.EndsWith("GetAbpAuditLogById")))
    //        {
    //            var path = swaggerDoc.Paths.First(p => p.Key.EndsWith("GetAbpAuditLogById"));
    //            path.Value.Description = "²âÊÔ";
    //            path.Value.Summary = "²âÊÔ";
    //            if (path.Value.Parameters.Any())
    //                path.Value.Parameters.First().Description = "²ÎÊý²âÊÔ";
    //        }
    //        //var path = swaggerDoc.Paths.Where(x => x.Key.Contains("Values")).First().Value;
    //        //path.Post.Parameters.FirstOrDefault().Extensions.Add("x-stuff", "123456");
    //    }
    //}
}
