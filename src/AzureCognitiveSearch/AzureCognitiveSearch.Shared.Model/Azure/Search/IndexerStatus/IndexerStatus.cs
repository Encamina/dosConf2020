using System.Collections.Generic;

namespace AzureCognitiveSearch.Shared.Model.Azure.Search.IndexerStatus
{
    public class IndexerStatus
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public History LastResult { get; set; }
        public ICollection<History> ExecutionHistory { get; set; }
        public Limits Limits { get; set; }
    }
}
