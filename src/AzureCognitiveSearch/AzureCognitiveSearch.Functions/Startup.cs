using AzureCognitiveSearch.Functions.Common;
using AzureCognitiveSearch.Functions.Extensions;
using AzureCognitiveSearch.Shared.Model.Infrastructure;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(AzureCognitiveSearch.Functions.Startup))]
namespace AzureCognitiveSearch.Functions
{
    public class Startup : FunctionsStartup
    { 

        public override void Configure(IFunctionsHostBuilder builder)
        {

           string basePath = IsDevelopmentEnvironment() ?
                            Environment.GetEnvironmentVariable("AzureWebJobsScriptRoot") :
                            $"{Environment.GetEnvironmentVariable("HOME")}\\site\\wwwroot";

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)  // common settings go here.
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)  // environment specific settings go here
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)  // secrets go here. This file is excluded from source control.
                .AddEnvironmentVariables()
                .Build();

            builder.Services.AddSingleton<IConfiguration>(configuration);             

            var appSettingsSection = configuration.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<AppSettings>();

            builder.Services.AddSingleton<IAppSettings>(appSettings);

            builder.Services.AddOptions<AppSettings>(); 
            builder.Services.AddHttpServices(appSettings);
            builder.Services.AddLogging();

            ConnectionPool.Container = builder.Services.BuildServiceProvider();
            ConnectionPool.AppSettings = appSettings;
            ConnectionPool.Configuration = configuration;
        }
       
             
        public static bool IsDevelopmentEnvironment()
        {
            return "Development".Equals(Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT"), StringComparison.OrdinalIgnoreCase);
        }
    }
}