using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Microsoft.Extensions.HealthChecks;
using TechnicBirthdayAgeService.Infrastructure;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace TechnicBirthdayAgeService
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
            ////services.AddHealthChecks(checks =>
            ////{
            ////    var minutes = 1;
            ////    if (int.TryParse(Configuration["HealthCheck:Timeout"], out var minutesParsed))
            ////    {
            ////        minutes = minutesParsed;
            ////    }
            ////    checks.AddSqlCheck("CatalogDb", Configuration["ConnectionString"], TimeSpan.FromMinutes(minutes));

            ////    var accountName = Configuration.GetValue<string>("AzureStorageAccountName");
            ////    var accountKey = Configuration.GetValue<string>("AzureStorageAccountKey");
            ////    if (!string.IsNullOrEmpty(accountName) && !string.IsNullOrEmpty(accountKey))
            ////    {
            ////        checks.AddAzureBlobStorageCheck(accountName, accountKey);
            ////    }
            ////});

          
            // Add framework services.
            services.AddMvc();


            services.AddDbContext<CatalogContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionString"],
                                     sqlServerOptionsAction: sqlOptions =>
                                     {
                                         sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                                         //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                                         sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                                     });

                // Changing default behavior when client evaluation occurs to throw. 
                // Default in EF Core would be to log a warning when client evaluation is performed.
                //options.ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));
                //Check Client vs. Server evaluation: https://docs.microsoft.com/en-us/ef/core/querying/client-eval
            });

           
            services.Configure<CatalogSettings>(Configuration);

            //Loads the configuration section into out Authentication Settings Object
            services.Configure<ApplicationSettings>(Configuration.GetSection("AuthenticationSettings"));

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = "My Birthday Service",
                    Version = "v1",
                    Description = "A simple example ASP.NET Core Web API",
                    TermsOfService = "None"
                });

                
                option.IgnoreObsoleteActions();
                option.DescribeAllParametersInCamelCase();
                option.OrderActionsBy(apiDesc => apiDesc.HttpMethod.ToString());
                option.DescribeAllEnumsAsStrings();
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
            //var connection = Configuration.GetConnectionString("SQLServerConnection").ToString();
            //services.AddDbContext<DbContext>(options => options.UseSqlServer(connection));
            //services.Configure<Settings>(options =>
            //{
            //    options.ConnectionString = Configuration.GetSection("MongoConnection:ConnectionString").Value;
            //    options.Database = Configuration.GetSection("MongoConnection:Database").Value;
            //});
            ////services.AddSingleton<IUnitOfWork>(option => new NorthwindUnitOfWork(Configuration.GetConnectionString("Northwind")));
            ////services.AddTransient<IValidator<Customer>, CustomerValidator>();

            ////services.AddResponseCompression();
            ////services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);

            ////var tokenProvider = new RsaJwtTokenProvider("issuer", "audience", "token_cibertec_2017");
            ////services.AddSingleton<ITokenProvider>(tokenProvider);

            ////services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            ////    .AddJwtBearer(options =>
            ////    {
            ////        options.RequireHttpsMetadata = false;
            ////        options.TokenValidationParameters = tokenProvider.GetValidationParameters();
            ////    });


            ////services.AddAuthorization(auth =>
            ////{
            ////    auth.DefaultPolicy = new AuthorizationPolicyBuilder()
            ////        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
            ////        .RequireAuthenticatedUser()
            ////        .Build();
            ////});

            // injection for autorfac
            //var container = new ContainerBuilder();
            //container.Populate(services);
            //return new AutofacServiceProvider(container.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseCors(
               options => options.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()
           );

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            // Add Serilog to the logging pipeline
            //loggerFactory.AddSerilog();
            //app.UseJwtBearerAuthentication(new JwtBearerOptions
            //{
            //    Authority = Configuration["AuthenticationSettings:AadInstance"] + Configuration["AuthenticationSettings:TenantId"],
            //    Audience = Configuration["AuthenticationSettings:Audience"]
            //});

            app.UseMvc();
            // Enable middleware to serve generated Swagger as a JSON endpoint.

            app.UseSwagger()
             .UseSwaggerUI(c =>
             {
                 c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
             });

            ////app.UseSwagger(c =>
            ////{
            ////    c.PreSerializeFilters.Add((swaggerDoc, httpReq) => swaggerDoc.BasePath = "/MyCalculatorApplication/TechnicBirthdayAgeService");
            ////});
            ////// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            ////app.UseSwaggerUI(c =>
            ////{
            ////    c.SwaggerEndpoint("/MyCalculatorApplication/TechnicBirthdayAgeService/swagger/v1/swagger.json", "My Birthday Service");
            ////});
        }
    }
}
