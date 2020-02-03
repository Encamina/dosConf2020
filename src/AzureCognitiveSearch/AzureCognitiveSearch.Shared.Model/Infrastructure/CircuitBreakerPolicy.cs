namespace AzureCognitiveSearch.Shared.Model.Infrastructure
{
    public class CircuitBreakerPolicy
    {
        public int DurationOfBreak { get; set; }

        public int ExceptionsAllowedBeforeBreaking { get; set; }
    }
}