using System.Collections.Generic;

namespace AzureCognitiveSearch.Shared.Model.Search
{
    public class SearchResultResponse
    {
        public IEnumerable<object> Result { get; set; }
        public int Page { get; set; }
        public int Total { get; set; }
    }
}
