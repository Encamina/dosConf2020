using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace AzureCognitiveSearch.Shared.Model.Azure.Search
{
    [SerializePropertyNamesAsCamelCase]
    public class AzureSearch : AzureSearchBase
    {
        [IsSearchable, IsFilterable]
        public string DocumentClass { get; set; }

        [IsSearchable]
        public string DocumentClassId { get; set; }

        [IsSearchable, IsFilterable]
        public string DocumentType { get; set; }

        [IsSortable, IsSearchable, IsFilterable]
        public string FileName { get; set; }

        [IsFilterable]
        public bool IsDeleted { get; set; }

        [IsFilterable]
        public bool HasLegalHold { get; set; }

        [IsSearchable, IsFilterable]
        public string ContentType { get; set; }

        [IsSortable]
        public long Size { get; set; }

        [IsSearchable]
        public string[] KeyPhrases { get; set; }

        [IsSearchable]
        public string[] Entities { get; set; }

        [IsSortable, IsFilterable]
        public double? Sentiment { get; set; }

        [IsSearchable, IsSortable]
        public string Language { get; set; }

        [IsSearchable, IsSortable]
        public string Adult { get; set; }

        [IsSearchable, IsSortable]
        public string ImageType { get; set; }

        [IsSearchable]
        public string Tags { get; set; }

        [IsSearchable]
        public string Faces { get; set; }

        [IsSearchable]
        public string Categories { get; set; }

        [IsSearchable]
        public string Color { get; set; }

        [IsSearchable]
        public string Description { get; set; }

        [IsFilterable]
        public string RequestId { get; set; }

        [IsSearchable]
        public string Metadata { get; set; }
    }
}
