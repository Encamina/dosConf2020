using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AzureCognitiveSearch.Shared.Utils.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<T> SendRequestAsync<T>(this HttpClient client, string requestUri, object body, Dictionary<string, string> headers,
                                                     Dictionary<string, object> parameters, HttpMethod httpMethod)
        {
            HttpResponseMessage response = await InternalSendRequest(client, requestUri, body, headers, parameters, httpMethod);

            if (response.IsSuccessStatusCode)
            {
                var jsonResult = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(jsonResult);
            }

            return await VerifyStatusCode<T>(response);
        }        

        public static async Task<byte[]> SendRequestToByteArrayAsync(this HttpClient client, string requestUri, object body, Dictionary<string, string> headers,
                                                            Dictionary<string, object> parameters, HttpMethod httpMethod)
        {
            HttpResponseMessage response = await InternalSendRequest(client, requestUri, body, headers, parameters, httpMethod);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsByteArrayAsync();
            }

            return null;
        }

        public static async Task<T> SendGetAsync<T>(this HttpClient client, string requestUri, Dictionary<string, string> headers, 
                                            Dictionary<string, object> parameters)
        {
            HttpResponseMessage response = await InternalSendRequest(client, requestUri, null, headers, parameters, HttpMethod.Get);

            if (response.IsSuccessStatusCode)
            {
                var jsonResult = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(jsonResult);
            }

            return await VerifyStatusCode<T>(response);
        }

        private static async Task<HttpResponseMessage> InternalSendRequest(HttpClient client, string requestUri,
                                                                   object body, Dictionary<string, string> headers,
                                                                   Dictionary<string, object> parameters, HttpMethod httpMethod)
        {
            var urlWithParameters = GetUrlWithParameters(requestUri, parameters);
            var json = body != null ? JsonConvert.SerializeObject(body) : "{}";
            var content = new StringContent(json);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            using (var httpRequestMessage = new HttpRequestMessage(httpMethod, urlWithParameters)
            {
                Content = content
            })
            {
                if (headers != null)
                {
                    foreach (var item in headers)
                    {
                        httpRequestMessage.Headers.Add(item.Key, item.Value);
                    }
                }
                return await client.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead);
            }
        }

        private static string GetUrlWithParameters(string requestUri, Dictionary<string, object> parameters)
        {
            var parameterArray = new List<string>();
            if (parameters != null)
            {
                requestUri += "?";
                foreach (var parameter in parameters)
                {
                    if (parameter.Value != null)
                    {
                        parameterArray.Add($"{parameter.Key}={parameter.Value}");
                    }
                }
                requestUri += string.Join("&", parameterArray);
            }
            return requestUri;
        }

        private static async Task<T> VerifyStatusCode<T>(HttpResponseMessage result)
        {
            if (result.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return default;
            }
            else if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new HttpRequestException(await result.Content.ReadAsStringAsync());
            }
            else
            {
                throw new AggregateException($"Api request error: {await result.Content.ReadAsStringAsync()}");
            }
        }
    }
}
