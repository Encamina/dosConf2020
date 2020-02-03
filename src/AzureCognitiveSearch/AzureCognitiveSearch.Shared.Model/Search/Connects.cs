using AzureCognitiveSearch.Shared.Model.Enums;

namespace AzureCognitiveSearch.Shared.Model.Search
{
    public class Connects
    {
        public Expression Exp { get; set; }
        public LogicOperatorType Connect { get; set; }
    }
}
