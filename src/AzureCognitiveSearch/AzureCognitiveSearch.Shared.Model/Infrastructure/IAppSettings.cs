using AzureCognitiveSearch.Shared.Model.Azure.Fx;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureCognitiveSearch.Shared.Model.Infrastructure
{     
    public interface IAppSettings
    {
         string SimpleField { get; set; }
         string RemoveUnidFilter { get; set; }
         string AzureRegion { get; set; }
         string VisionVersion { get; set; }
         string ApiKey { get; set; }
         string VisioApiKey { get; set; }
         string CognitiveTextApiKey { get; set; }
         string SearchName { get; set; }
         string ApiVersion { get; set; }
         string SearchUrl { get; set; }
         string BaseUrl { get; set; }
         string CustomSkillUrl { get; set; }
         string Index { get; set; }
         PolicyConfig PolicyConfig { get; set; }
         string StorageKey { get; set; }
         DocumentsFolders DocumentsFolders { get; set; }
         ResourceFiles ResourceFiles { get; set; }
         int ThumbnailHeight { get; set; }
         int ThumbnailWidth { get; set; }
         string ApvIndex { get; set; }
         int RegisterIndexRetry { get; set; }
         string SuggesterName { get; set; }
         int ItemsByPage { get; set; }         
    }
}
