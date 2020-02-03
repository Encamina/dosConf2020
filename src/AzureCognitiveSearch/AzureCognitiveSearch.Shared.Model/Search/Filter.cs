using System.Collections.Generic;

namespace AzureCognitiveSearch.Shared.Model.Search
{
    public class Filter
    {
        public Filter()
        {
            Conditions = new List<string>();
        }

        public string Field { get; set; }
        public string Display { get; set; }
        public List<string> Conditions { get; set; }
    }
}
