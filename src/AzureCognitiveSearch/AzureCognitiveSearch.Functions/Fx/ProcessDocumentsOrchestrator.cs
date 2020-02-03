using AzureCognitiveSearch.Functions.Fx.Activities;
using AzureCognitiveSearch.Shared.Model.Azure.Fx;
using AzureCognitiveSearch.Shared.Model.Azure.Search.IndexerStatus;
using AzureCognitiveSearch.Shared.Model.Enums;
using AzureCognitiveSearch.Shared.Model.Infrastructure;
using AzureCognitiveSearch.Shared.Utils.Extensions;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Threading.Tasks;

namespace AzureCognitiveSearch.Functions.Fx
{
    public partial class ProcessDocumentsOrchestrator
    {
        private readonly IAppSettings appSettings;

        public ProcessDocumentsOrchestrator(IAppSettings appSettings)
        {
            this.appSettings = appSettings;
        }

        [FunctionName(nameof(ProcessDocumentsOrchestrator))]
        public async Task StartOrchestrationAsync([OrchestrationTrigger] DurableOrchestrationContext context, ILogger log)
        {
            var process = context.GetInput<OrchestrationProcess>();

            try
            {
                log.LogInformationIfNotRetry(context, $"Process {process.Id} - Generating Azure Cognitive Search Infrastructure");

                var processIDNoHyphen = new Guid(process.Id).ToString("N");

                var (index, status) = await GenerateSearchInfraestructureAsync(context, appSettings, process.Id, appSettings.DocumentsFolders.ProcessContainer, processIDNoHyphen, log);

                var str = new StringBuilder();
                str.Append($"Process {process.Id} - Indexer error status: { Newtonsoft.Json.JsonConvert.SerializeObject(status.LastResult?.Errors) }. ");
                str.Append($"Indexer warning status: { Newtonsoft.Json.JsonConvert.SerializeObject(status.LastResult?.Warnings) }");

                process.Status = ProcessStatusType.OK;
            }
            catch (Exception ex)
            {
                log.LogError($"Process {process.Id} - Exception, error message: {ex.Message}");

                process.Status = ProcessStatusType.KO;
            }

            log.LogInformationIfNotRetry(context, $"Process {process.Id} - Process finishied with status {process.Status}.");
        }



        private async Task<(string index, IndexerStatus status)> GenerateSearchInfraestructureAsync(DurableOrchestrationContext context, IAppSettings appSettings,
                                                                      string processId, string dataSourceContainer, string processIDNoHyphen, ILogger log)
        {
            var skillSet = "skillset" + processIDNoHyphen;
            var dataSource = "demodata";
            var index = "index" + processIDNoHyphen;
            var indexer = "indexer" + processIDNoHyphen;

            var jsonContainer = appSettings.DocumentsFolders.Resources;

            log.LogInformationIfNotRetry(context, $"Process {processId} - Obtaining SkillSet JSON");
            var skillParmas = new DownloadBlobTextParameters
            {
                Container = jsonContainer,
                File = appSettings.ResourceFiles.SkillSetJsonName
            };
            var skillSetJson = await context.CallActivityAsync<string>(nameof(BlobTextDownload.DownloadBlobText), skillParmas);
            log.LogInformationIfNotRetry(context, $"Process {processId} - Finished obtaining SkillSet JSON");

            log.LogInformationIfNotRetry(context, $"Process {processId} - Obtaining DataSource JSON");
            var dataSourceBlob = new DownloadBlobTextParameters
            {
                Container = jsonContainer,
                File = appSettings.ResourceFiles.DataSourceJsonName
            };
            var dataSourceJson = await context.CallActivityAsync<string>(nameof(BlobTextDownload.DownloadBlobText), dataSourceBlob);
            log.LogInformationIfNotRetry(context, $"Process {processId} - Finished obtaining DataSource JSON");

            log.LogInformationIfNotRetry(context, $"Process {processId} - Obtaining Index JSON");
            var indexBlob = new DownloadBlobTextParameters
            {
                Container = jsonContainer,
                File = appSettings.ResourceFiles.IndexJsonName
            };
            var indexJson = await context.CallActivityAsync<string>(nameof(BlobTextDownload.DownloadBlobText), indexBlob);
            log.LogInformationIfNotRetry(context, $"Process {processId} - Finished obtaining Index JSON");

            log.LogInformationIfNotRetry(context, $"Process {processId} - Obtaining Indexer JSON");
            var indexerBlob = new DownloadBlobTextParameters
            {
                Container = jsonContainer,
                File = appSettings.ResourceFiles.IndexerJsonName
            };
            var indexerJson = await context.CallActivityAsync<string>(nameof(BlobTextDownload.DownloadBlobText), indexerBlob);
            log.LogInformationIfNotRetry(context, $"Process {processId} - Finished obtaining Indexer JSON");


            log.LogInformationIfNotRetry(context, $"Process {processId} - Creating SkillSet {skillSet}");
            var isSkillSetCreated = await context.CallActivityAsync<bool>(nameof(AzureSearchManagement.FtnCreateSkillSet),
                new BaseSearchCreateParameters
                {
                    Name = skillSet,
                    Json = skillSetJson
                });
            log.LogInformationIfNotRetry(context, $"Process {processId} - SkillSet {skillSet} { GetStatusText(isSkillSetCreated)}");

            //log.LogInformationIfNotRetry(context, $"Process {processId} - Creating DataSource {dataSource}");
            //var isDataSourceCreated = await context.CallActivityAsync<bool>(nameof(AzureSearchManagement.FtnCreateDataSource),
            //    new CreateDataSourceParameters
            //    {
            //        Name = dataSource,
            //        Container = dataSourceContainer,
            //        Json = dataSourceJson
            //    });
            //log.LogInformationIfNotRetry(context, $"Process {processId} - DataSource {dataSource} creation {GetStatusText(isDataSourceCreated)}");

            log.LogInformationIfNotRetry(context, $"Process {processId} - Creating Index {index}");
            var isIndexCreated = await context.CallActivityAsync<bool>(nameof(AzureSearchManagement.FtnCreateIndex),
                new BaseSearchCreateParameters
                {
                    Name = index,
                    Json = indexJson
                });
            log.LogInformationIfNotRetry(context, $"Process {processId} - Index {index} creation {GetStatusText(isIndexCreated)}");

            log.LogInformationIfNotRetry(context, $"Process {processId} - Creating Indexer {indexer}");
            var isIndexerCreated = await context.CallActivityAsync<bool>(nameof(AzureSearchManagement.FtnCreateIndexer),
                new CreateIndexerParameters
                {
                    Name = indexer,
                    Json = indexerJson,
                    SkillSetName = skillSet,
                    DataSourceName = dataSource,
                    IndexName = index
                });
            log.LogInformationIfNotRetry(context, $"Process {processId} - Indexer {indexer} creation {GetStatusText(isIndexerCreated)}");

            log.LogInformationIfNotRetry(context, $"Process {processId} - Checking Indexer {indexer} status...");
            System.Threading.Thread.Sleep(5000);

            IndexerStatus indexerStatus;
            do
            {
                System.Threading.Thread.Sleep(1000);
                indexerStatus = await context.CallActivityAsync<IndexerStatus>(nameof(AzureSearchManagement.FtnGetIndexerStatus), indexer);
                log.LogInformationIfNotRetry(context, $"Process {processId} - Checking Indexer {indexer} status. STATUS: {indexerStatus.LastResult?.Status}");
            } while (indexerStatus.LastResult?.Status == Status.InProgress);

            log.LogInformationIfNotRetry(context, $"Process {processId} - Finished checking Indexer {indexer} status. STATUS: {indexerStatus.LastResult?.Status}");

            return (index, indexerStatus);
        }

        private static string GetStatusText(bool processInfo)
        {
            return (processInfo ? "SUCCEDED" : "FAILED");
        }
    }
}