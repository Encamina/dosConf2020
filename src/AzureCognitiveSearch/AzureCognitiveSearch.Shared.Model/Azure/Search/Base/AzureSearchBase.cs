using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System;
using System.ComponentModel;

namespace AzureCognitiveSearch.Shared.Model.Azure.Search
{    
    public class AzureSearchBase 
    {
        [Description("ID")]
        [System.ComponentModel.DataAnnotations.Key]
        public string Id { get; set; }

        [Description("CREATED")]
        public DateTime Created { get; set; }

        [Description("MODIFIED")]
        public DateTime Modified { get; set; }

        [Description("CREATEDUSER")]
        [IsSearchable, Analyzer(AnalyzerName.AsString.EsMicrosoft)]
        public string CreatedUser { get; set; }

        [Description("LASTMODIFIEDUSER")]
        [IsSearchable, Analyzer(AnalyzerName.AsString.EsMicrosoft)]
        public string LastModifiedUser { get; set; }
    }
}

