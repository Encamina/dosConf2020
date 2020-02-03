namespace AzureCognitiveSearch.Shared.Model.Infrastructure
{
    public class PolicyConfig
    {
        public RetryPolicy RetryPolicy { get; set; }
        public CircuitBreakerPolicy CircuitBreakerPolicy { get; set; }
    }
}