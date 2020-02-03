using System.Collections.Generic;

namespace AzureCognitiveSearch.Shared.Model.Azure.Cognitives
{
    public class Description
    {
        public List<string> Tags { get; set; }
        public List<Caption> Captions { get; set; }
    }
}
