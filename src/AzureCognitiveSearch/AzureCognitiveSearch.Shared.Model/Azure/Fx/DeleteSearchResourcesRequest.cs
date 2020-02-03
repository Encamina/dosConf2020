namespace AzureCognitiveSearch.Shared.Model.Azure.Fx
{
    public class DeleteSearchResourcesRequest
    {
        public string SkillSet { get; set; }
        public string DataSource { get; set; }
        public string Index { get; set; }
        public string Indexer { get; set; }
    }
}
