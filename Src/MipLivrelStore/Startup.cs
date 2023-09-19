using Azure.Storage.Blobs;
using DDD.Infra.CrossCutting.IoC;
using DDD.Infra.Data.Context;
using DDD.Services.Api.StartupExtensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using MipLivrelStore.Configurations;
using MipLivrelStore.StartupExtensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MipLivrelStore
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;

            var builder = new ConfigurationBuilder().AddConfiguration(configuration)
           .SetBasePath(env.ContentRootPath)
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
           .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _env;
        private IApplicationBuilder app;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Miplivrel", Version = "v1" });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First()); //This line
            });

            services.AddControllersWithViews();

            // ----- Database -----
            services.AddCustomizedDatabase(Configuration, _env);
            // ----- Auth -----
            services.AddCustomizedAuth(Configuration);

            // ----- Http -----
            //services.AddCustomizedHttp(Configuration);

            // ----- AutoMapper -----
            services.AddAutoMapperSetup();

            // Adding MediatR for Domain Events and Notifications
            services.AddMediatR(typeof(Startup));

            services.AddCustomizedHash(Configuration);

            services.AddMemoryCache();

            // .NET Native DI Abstraction
            RegisterServices(services);
            //services.AddRazorPages();

            //add our azure connection string
            services.AddScoped(_ => { return new BlobServiceClient(Configuration.GetConnectionString("AzureBlobStorage")); });

            // services.Addlocalization
            services.AddLocalization(options => options.ResourcesPath = "Ressource");
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.SetDefaultCulture("ar-SA");
                options.AddSupportedUICultures("en-US", "fr-FR", "ar-SA");
                options.FallBackToParentUICultures = true;
                //-------
                // options.RequestCultureProviders.Remove((IRequestCultureProvider)typeof(AcceptLanguageHeaderRequestCultureProvider));
            });

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
            });

            services.AddMvc().AddViewLocalization().AddDataAnnotationsLocalization();

            // Register the Google Analytics configuration
            services.Configure<GoogleAnalyticsOptions>(options => Configuration.GetSection("GoogleAnalytics").Bind(options));

            // Register the TagHelperComponent
            services.AddTransient<ITagHelperComponent, GoogleAnalyticsTagHelperComponent>();

        }

        private static void RegisterServices(IServiceCollection services)
        {
            // Adding dependencies from another layers (isolated from Presentation)
            NativeInjectorBootStrapper.RegisterServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            this.app = app;
            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();                
            }

            bool testEnvironment = string.Equals(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"), "test", StringComparison.InvariantCultureIgnoreCase);

            if (env.IsDevelopment() || testEnvironment)
            {
                app.UseSwagger();
                app.UseSwaggerUI(config =>
                {
                    config.SwaggerEndpoint("/swagger/v1/swagger.json", "Test v1");
                    //config.SwaggerEndpoint("/swagger/v2/swagger.json", "Test v2");
                });
            }

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBMAY9C3t2VVhhQlFaclhJXGFWfVJpTGpQdk5xdV9DaVZUTWY/P1ZhSXxRdkFiXn5ecn1RQmBbV0Y=");

            // ----- Error Handling -----
            app.UseCustomizedErrorHandling(_env);

            //Localization
            app.UseHttpsRedirection();

            var supportedCultures = new[]
           {
                new CultureInfo("en-US"),
                new CultureInfo("fr-FR"),
                new CultureInfo("ar")
            };
            var requestLocalizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,
                // UI strings that we have localized.
                SupportedUICultures = supportedCultures
            };

            app.UseResponseCompression();
            app.UseRequestLocalization(requestLocalizationOptions);
            app.UseStaticFiles();

            app.UseRouting();
            // ----- CORS -----
            app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                                    name: "default",
                                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                                    name: "Api",
                                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllers();
            });


            UpdateDatabase();
        }

        private void UpdateDatabase()
        {
            //logger.LogInformation("Triggered UpdateDatabase");
            try
            {
                using var serviceScope = app.ApplicationServices
                    .GetRequiredService<IServiceScopeFactory>()
                    .CreateScope();
                using var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                context.Database.Migrate();
                //logger.LogInformation("Update Database done");
            }
            catch (Exception e)
            {
                //  logger.LogError("Error when excute migrate database", e);
            }
        }
    }
}
