using AzureCognitiveSearch.ApplicationServices.Abstract.Clients;
using AzureCognitiveSearch.ApplicationServices.Clients;
using AzureCognitiveSearch.Shared.Model.Infrastructure;
using AzureCognitiveSearch.Shared.Utils.Extensions;
using AzureCognitiveSearch.Shared.Utils.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AzureCognitiveSearch.Functions.Extensions
{
    /// <summary>
    /// ServiceCollectionExtensions
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// AddHttpServices
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddHttpServices(this IServiceCollection services, AppSettings appSettings)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();         

            services.AddPolicies(appSettings);

            HttpClientOptions azureSearchOptions = new HttpClientOptions()
            {
                BaseAddress = new Uri($"https://{appSettings.SearchName}.search.windows.net/"),
                Timeout = TimeSpan.FromMinutes(10)
            };
            services.AddHttpClient<IAzureSearchClient, AzureSearchClient, HttpClientOptions>(azureSearchOptions, "azuresearch");

            HttpClientOptions azureVisionOptions = new HttpClientOptions()
            {
                BaseAddress = new Uri($"https://{appSettings.AzureRegion}.api.cognitive.microsoft.com/vision/{appSettings.VisionVersion}"),
                Timeout = TimeSpan.FromMinutes(10)
            };
            services.AddHttpClient<IAzureVisionClient, AzureVisionClient, HttpClientOptions>(azureVisionOptions, "azurevision");

            return services;
        }

        /// <summary>
        /// AddDependencyInjection
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        { 
            services.AddScoped<IAzureSearchClient, AzureSearchClient>();
            services.AddScoped<IAzureVisionClient, AzureVisionClient>();  

            return services;
        }
    }
}
