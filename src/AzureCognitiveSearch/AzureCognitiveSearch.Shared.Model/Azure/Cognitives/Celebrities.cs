namespace AzureCognitiveSearch.Shared.Model.Azure.Cognitives
{
    public class Celebrities
    {
        public string Name { get; set; }
        public double? Confidence { get; set; }
        public FaceRectangle FaceRectangle { get; set; }
    }
}
