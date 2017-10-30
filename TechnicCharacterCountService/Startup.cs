using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TechnicCharacterCountService
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = "My Word count Service",
                    Version = "v1",
                    Description = "A simple example ASP.NET Core Web API char count",
                    TermsOfService = "None"
                });
                option.IgnoreObsoleteActions();
                option.DescribeAllParametersInCamelCase();
                //option.OrderActionsBy(apiDesc => apiDesc.HttpMethod.ToString());
                option.DescribeAllEnumsAsStrings();
            });
        }

        ////public static void UseSwaggerWithOptions(this IApplicationBuilder builder, string title, string apiSuffix, string version = "v1")
        ////{
        ////    //I can access http://localhost:19081/My.SF.AppName/My.Stateless.Api/docs/v1/swagger.json
        ////    builder.UseSwagger(options => options.RouteTemplate = "docs/{documentName}/swagger.json");
        ////    builder.UseSwaggerUI(c =>
        ////    {
        ////        c.RoutePrefix = $"MyCalculatorApplication/TechnicCharacterCountService/docs";
        ////        c.SwaggerEndpoint($"/docs/{version}/swagger.json", $"{title} {version}");
        ////    });
        ////}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
            app.UseSwagger( options => {
                options.RouteTemplate = "api/swagger/{documentName}/swagger.json";
                // options.RouteTemplate = "docs/{documentName}/swagger.json";
                //options.PreSerializeFilters.Add((swaggerDoc, httpReq) => swaggerDoc.BasePath = "/MyCalculatorApplication/TechnicCharacterCountService");
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/api/swagger/v1/swagger.json", "Some API");
                c.RoutePrefix = "api/swagger";
                //c.SwaggerEndpoint($"/docs/v1/swagger.json", $"TechnicCharacterCountService v1");
                //c.RoutePrefix = $"MyCalculatorApplication/TechnicCharacterCountService/docs";
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint.  
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            ////app.UseSwagger();
            ////app.UseSwaggerUI(c =>
            ////{
            ////    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
         //////.EnableSwagger(c =>
         ////// {
         //////     var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
         //////     var commentsFileName = Assembly.GetExecutingAssembly().GetName().Name + ".XML";
         //////     var commentsFile = Path.Combine(baseDirectory, commentsFileName);

         //////     c.SingleApiVersion("v1", "A title for your API");
         //////     c.IncludeXmlComments(commentsFile);
         //////     c.IncludeXmlComments(GetXmlCommentsPathForModels());
         ////// });
            //app.UseSwagger(c =>
            //{
            //    //c.PreSerializeFilters.Add((swagger, httpReq) => swagger.Host = httpReq.Host.Value);
            //    c.PreSerializeFilters.Add((swaggerDoc, httpReq) => swaggerDoc.BasePath = "/MyCalculatorApplication/TechnicCharacterCountService");
            //});
            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/MyCalculatorApplication/TechnicCharacterCountService/swagger/v1/swagger.json", "My API V1");
            //});

            //app.UseSwagger()
            // .UseSwaggerUI(c =>
            // {
            //     c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            //});
        }
    }
}
