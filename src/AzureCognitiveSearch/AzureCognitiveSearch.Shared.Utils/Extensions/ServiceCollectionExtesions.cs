using AzureCognitiveSearch.Shared.Model.Infrastructure;
using AzureCognitiveSearch.Shared.Utils.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;
using System;
using System.Linq;
using System.Net.Mime;

namespace AzureCognitiveSearch.Shared.Utils.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// AddPolicies
        /// </summary>
        /// <param name="services"></param>
        /// <param name="policyOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddPolicies(this IServiceCollection services, AppSettings appSettings)
        {
            var policyRegistry = services.AddPolicyRegistry();

            policyRegistry.Add(
                PolicyName.HttpRetry,
                HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .Or<TimeoutRejectedException>()
                    .WaitAndRetryAsync(
                        appSettings.PolicyConfig.RetryPolicy.Count,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(appSettings.PolicyConfig.RetryPolicy.Delay, retryAttempt))));

            policyRegistry.Add(
                PolicyName.HttpCircuitBreaker,
                HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .Or<TimeoutRejectedException>()
                    .CircuitBreakerAsync(
                        appSettings.PolicyConfig.CircuitBreakerPolicy.ExceptionsAllowedBeforeBreaking,
                        TimeSpan.FromSeconds(appSettings.PolicyConfig.CircuitBreakerPolicy.DurationOfBreak)));

            return services;
        }

        /// <summary>
        /// AddHttpClient
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <typeparam name="TClientOptions"></typeparam>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddHttpClient<TClient, TImplementation, TClientOptions>(
                                            this IServiceCollection services,
                                            TClientOptions options)
                                                        where TClient : class
                                                        where TImplementation : class, TClient
                                                        where TClientOptions : HttpClientOptions, new() =>
            services
                .AddHttpClient<TClient, TImplementation>()
                .ConfigureCustomHttpClient<TImplementation, TClientOptions>(options)
                .Services;

        /// <summary>
        /// AddHttpClient
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <typeparam name="TClientOptions"></typeparam>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IServiceCollection AddHttpClient<TClient, TImplementation, TClientOptions>(
                                            this IServiceCollection services,
                                            TClientOptions options, string name)
                                                        where TClient : class
                                                        where TImplementation : class, TClient
                                                        where TClientOptions : HttpClientOptions, new() =>
            services
                .AddHttpClient<TClient, TImplementation>(name)
                .ConfigureCustomHttpClient<TImplementation, TClientOptions>(options)
                .Services;

        /// <summary>
        /// AddHttpClient
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <typeparam name="TClientOptions"></typeparam>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddHttpClient<TImplementation, TClientOptions>(
                                            this IServiceCollection services,
                                            TClientOptions options)
                                                        where TImplementation : class
                                                        where TClientOptions : HttpClientOptions, new() =>
            services
                .AddHttpClient<TImplementation>()
                .ConfigureCustomHttpClient<TImplementation, TClientOptions>(options)
                .Services;

        /// <summary>
        /// AddHttpClient
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <typeparam name="TClientOptions"></typeparam>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <param name="name"></param> 
        /// <returns></returns>
        public static IServiceCollection AddHttpClient<TImplementation, TClientOptions>(
                                            this IServiceCollection services,
                                            TClientOptions options, string name)
                                                        where TImplementation : class
                                                        where TClientOptions : HttpClientOptions, new() =>
            services
                .AddHttpClient<TImplementation>(name)
                .ConfigureCustomHttpClient<TImplementation, TClientOptions>(options)
                .Services;

        /// <summary>
        /// ConfigureCustomHttpClient
        /// </summary>
        /// <typeparam name="TImplementation"></typeparam>
        /// <typeparam name="TClientOptions"></typeparam>
        /// <param name="httpClientBuilder"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        private static IHttpClientBuilder ConfigureCustomHttpClient<TImplementation, TClientOptions>(
                                                this IHttpClientBuilder httpClientBuilder,
                                                TClientOptions options)
                                                        where TImplementation : class
                                                        where TClientOptions : HttpClientOptions, new() =>
                httpClientBuilder.ConfigureHttpClient((serviceProvider, httpClientOptions) =>
                {
                    httpClientOptions.BaseAddress = options.BaseAddress;
                    httpClientOptions.Timeout = options.Timeout;
                    httpClientOptions.DefaultRequestHeaders.Add("Accept", MediaTypeNames.Application.Json);
                    AddBearerTokenToHeaders(serviceProvider, httpClientOptions);
                })
                .ConfigurePrimaryHttpMessageHandler(x => new DefaultHttpClientHandler())
                .AddPolicyHandlerFromRegistry(PolicyName.HttpRetry)
                .AddPolicyHandlerFromRegistry(PolicyName.HttpCircuitBreaker)
                .AddTransientHttpErrorPolicy(x => x.RetryAsync())
                .AddTypedClient<TImplementation>();

        /// <summary>
        /// AddBearerTokenToHeaders
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="httpClientOptions"></param>
        private static void AddBearerTokenToHeaders(IServiceProvider serviceProvider, System.Net.Http.HttpClient httpClientOptions)
        {
            const string AuthorizationHeader = "Authorization";
            const string Bearer = "bearer";

            var httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();
            var bearerToken = httpContextAccessor.HttpContext?.Request?
                                    .Headers[AuthorizationHeader]
                                    .FirstOrDefault(h => h.StartsWith($"{Bearer} ", StringComparison.InvariantCultureIgnoreCase));

            if (bearerToken != null)
            {
                httpClientOptions.DefaultRequestHeaders.Add(AuthorizationHeader, bearerToken);
            }
        }
    }
}
