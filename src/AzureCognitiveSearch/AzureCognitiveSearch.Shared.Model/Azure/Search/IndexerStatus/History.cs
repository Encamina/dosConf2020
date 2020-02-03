using System;
using System.Collections.Generic;

namespace AzureCognitiveSearch.Shared.Model.Azure.Search.IndexerStatus
{
    public class History
    {
        public string Status { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int ItemsProcessed { get; set; }
        public int ItemsFailed { get; set; }
        public string InitialTrackingState { get; set; }
        public string FinalTrackingState { get; set; }
        public ICollection<Error> Errors { get; set; }
        public ICollection<Warning> Warnings { get; set; }
    }
}
