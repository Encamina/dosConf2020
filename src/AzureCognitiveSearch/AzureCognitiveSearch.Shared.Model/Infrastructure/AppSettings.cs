using AzureCognitiveSearch.Shared.Model.Azure.Fx;

namespace AzureCognitiveSearch.Shared.Model.Infrastructure
{
    public class AppSettings : IAppSettings
    {
        public string SimpleField { get; set; }
        public string RemoveUnidFilter { get; set; }
        public string AzureRegion { get; set; }
        public string VisionVersion { get; set; }
        public string ApiKey { get; set; }
        public string VisioApiKey { get; set; }
        public string CognitiveTextApiKey { get; set; }
        public string SearchName { get; set; }
        public string ApiVersion { get; set; }
        public string SearchUrl { get; set; }
        public string BaseUrl { get; set; }
        public string CustomSkillUrl { get; set; }
        public string Index { get; set; }
        public PolicyConfig PolicyConfig { get; set; }
        public string StorageKey { get; set; }         
        public DocumentsFolders DocumentsFolders { get; set; }
        public ResourceFiles ResourceFiles { get; set; }
        public int ThumbnailHeight { get; set; }
        public int ThumbnailWidth { get; set; }
        public string ApvIndex { get; set; }
        public int RegisterIndexRetry { get; set; }
        public string SuggesterName { get; set; }
        public int ItemsByPage { get; set; }
    }
}