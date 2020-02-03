using System.Collections.Generic;

namespace AzureCognitiveSearch.Shared.Model.Azure.Cognitives
{
    public class CognitivesResponse
    {
        public string Id { get; set; }
        public string FileName { get; set; }
        public string RequestId { get; set; }
        public string Language { get; set; }
        public List<string> KeyPhrases { get; set; }
        public List<string> Entities { get; set; }
        public double? Sentiment { get; set; }
        public List<Categories> Categories { get; set; }
        public Adult Adult { get; set; }
        public Color Color { get; set; }
        public ImageType ImageType { get; set; }
        public List<Tag> Tags { get; set; }
        public Description Description { get; set; }
        public List<Face> Faces { get; set; }
        public Metadata Metadata { get; set; }
    }
}
