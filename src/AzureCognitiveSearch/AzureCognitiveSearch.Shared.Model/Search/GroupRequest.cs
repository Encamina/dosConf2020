using System.Collections.Generic;

namespace AzureCognitiveSearch.Shared.Model.Search
{
    public class GroupRequest
    {
        public IEnumerable<ConnectRequest> Group { get; set; }               
    }
}