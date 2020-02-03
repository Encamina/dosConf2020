namespace AzureCognitiveSearch.Shared.Utils.Http
{
    public class RetryPolicyOptions
    {
        public int Count { get; set; } = 3;

        public int Delay { get; set; } = 2;
    }
}
