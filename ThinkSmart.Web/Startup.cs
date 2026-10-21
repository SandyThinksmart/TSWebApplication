using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using PdfSharpCore.Fonts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using TSWebApplication.Application;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure;
using TSWebApplication.Web.MiddleWares;
using TSWebApplication.Web.Models;

namespace TSWebApplication.Web
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
           services.AddSingleton<IFontResolver>(sp =>
            {
                var env = sp.GetRequiredService<IWebHostEnvironment>();
                return new FontResolver(env);
            });
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddApplicationServices();
            services.AddInfrastructureServices(Configuration);
            services.AddBrowserDetection();
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddSingleton<LocalizationServices>();
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.AddScoped<IViewRenderService, ViewRenderService>();
            services.AddHttpContextAccessor();
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });
            services.AddHttpContextAccessor();
            services.AddScoped<CsvSqlString>();
            services.AddMvc()
                .AddNewtonsoftJson()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                    {
                        var assemblyName = new AssemblyName(typeof(ApplicationResource).GetTypeInfo().Assembly.FullName);
                        return factory.Create("ApplicationResource", assemblyName.Name);
                    };
                });
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("ja-JP"),
                    new CultureInfo("en"),
    
                };
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.DefaultRequestCulture = new RequestCulture("ja-JP");
                options.RequestCultureProviders = new List<IRequestCultureProvider>
        {
            new QueryStringRequestCultureProvider(),
            new CookieRequestCultureProvider()
        };
                //services.AddMvc().AddRazorOptions(options => options.AllowRecompilingViewsOnFileChange = true);
            });
            services.AddHttpContextAccessor();
            services.AddDistributedMemoryCache();
          
            services.AddHttpContextAccessor();
           
        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
           // app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();
           
            var requestlocalizationOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(requestlocalizationOptions.Value);
            app.UseAuthorization();
            app.UseMiddleware<ErrorHandlerMiddleware>();
            UIConstants.UIConstantsConfigure(app.ApplicationServices.GetService<LocalizationServices>(),app.ApplicationServices.GetService<IHttpContextAccessor>());
            UIHelper.UIHeplerConfigure(app.ApplicationServices.GetService<LocalizationServices>(), app.ApplicationServices.GetService<IHttpContextAccessor>());
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=MenuMaster}/{action=Index}/{id?}");
            });
        }
    }
}
