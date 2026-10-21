using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using System.Configuration;
using System.Globalization;
using System.Net;
using System.Reflection;
using TSWebApplication.API.Controllers;
using TSWebApplication.API.MiddleWares;
using TSWebApplication.Application;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddDataAnnotationsLocalization(options =>
{
    options.DataAnnotationLocalizerProvider = (type, factory) =>
    {
        AssemblyName? assemblyName = new AssemblyName(typeof(ApplicationResource).GetTypeInfo().Assembly.FullName);
        return factory.Create("ApplicationResource", assemblyName.Name);
    };
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationServices(); 
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddCors();
builder.Services.AddBrowserDetection();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddSingleton<LocalizationServices>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
                    new CultureInfo("ja-JP"),
                    new CultureInfo("en"),
                     new CultureInfo("hi"),

                };
    options.DefaultRequestCulture = new RequestCulture("ja-JP");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
    
    //services.AddMvc().AddRazorOptions(options => options.AllowRecompilingViewsOnFileChange = true);
});
builder.Services.Configure<FormOptions>(options =>
{
    options.ValueCountLimit = 200000; // added for FormData rows total rows * columns
});
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
app.UseSwagger();
app.UseSwaggerUI();
//app.UseHttpsRedirection();
var requestlocalizationOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(requestlocalizationOptions.Value);
app.UseAuthorization();
app.UseRouting();
app.UseMiddleware<ErrorHandlerMiddleware>();
UIConstants.UIConstantsConfigure(app.Services.GetService<LocalizationServices>(), app.Services.GetService<IHttpContextAccessor>());
UIHelper.UIHeplerConfigure(app.Services.GetService<LocalizationServices>(), app.Services.GetService<IHttpContextAccessor>());
app.MapControllers();
app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                );
app.Run();
