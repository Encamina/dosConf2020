using System.Collections.Generic;

namespace AzureCognitiveSearch.Shared.Model.Azure.Fx
{
    public class ExpFolderResult
    {
        public List<string> CaseFiles {get; set;}
        public int TotalJpgs { get; set; }
        public int TotalPdfs { get; set; }
    }
}
