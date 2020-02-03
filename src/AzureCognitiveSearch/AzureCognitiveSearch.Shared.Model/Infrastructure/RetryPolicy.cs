namespace AzureCognitiveSearch.Shared.Model.Infrastructure
{
    public class RetryPolicy
    {
        public int Count { get; set; }

        public int Delay { get; set; }
    }
}