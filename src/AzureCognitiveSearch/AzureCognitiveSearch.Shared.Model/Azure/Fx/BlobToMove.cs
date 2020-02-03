namespace AzureCognitiveSearch.Shared.Model.Azure.Fx
{
    public class BlobToMove : BlobInfo
    {
        public string BlobReadToken { get; set; }
        public string BlobDeleteToken { get; set; }
        public string ProcessId { get; set; }
    }
}