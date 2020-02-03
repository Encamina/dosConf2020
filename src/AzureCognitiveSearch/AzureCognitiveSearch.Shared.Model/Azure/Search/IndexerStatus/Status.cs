namespace AzureCognitiveSearch.Shared.Model.Azure.Search.IndexerStatus
{
    public static class Status
    {
        public static readonly string Success = "success";
        public static readonly string InProgress = "inProgress";
        public static readonly string TransientFailure = "transientFailure";
        public static readonly string PersistentFailure = "persistentFailure";
        public static readonly string Reset = "reset";
    }
}
