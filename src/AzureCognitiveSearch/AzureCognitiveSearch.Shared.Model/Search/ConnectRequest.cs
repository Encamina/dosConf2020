using AzureCognitiveSearch.Shared.Model.Enums;
using System.Collections.Generic;

namespace AzureCognitiveSearch.Shared.Model.Search
{
    public class ConnectRequest
    {
        public IEnumerable<Connects> Condition{ get; set; }
        public LogicOperatorType Connect { get; set; }
    }
}