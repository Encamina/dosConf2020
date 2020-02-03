namespace AzureCognitiveSearch.Shared.Model.Azure.Search.IndexerStatus
{
    public class Error
    {
        public string Key { get; set; }
        public string ErrorMessage { get; set; }
        public int StatusCode { get; set; }
    }
}
