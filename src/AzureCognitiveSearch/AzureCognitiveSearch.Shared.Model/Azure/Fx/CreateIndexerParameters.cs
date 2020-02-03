namespace AzureCognitiveSearch.Shared.Model.Azure.Fx
{
    public class CreateIndexerParameters : BaseSearchCreateParameters
    {
        public string DataSourceName { get; set; }
        public string IndexName { get; set; }
        public string SkillSetName { get; set; }
    }
}
