namespace AzureCognitiveSearch.Shared.Model.Azure.Search.IndexerStatus
{
    public class Limits
    {
        public string MaxRunTime { get; set; }
        public int MaxDocumentExtractionSize { get; set; }
        public int MaxDocumentContentCharactersToExtract { get; set; }
    }
}
