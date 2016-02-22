using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AzureApplicationDemo.Features.Upload;
using AzureApplicationDemo.Services;
using MediatR;
using Microsoft.Azure.Batch;
using Microsoft.Azure.Batch.Auth;
using Microsoft.Azure.Batch.Common;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureApplicationDemo.Features.ImageProcessing
{
    public class BatchProcessorHandler : INotificationHandler<BatchUploadComplete>, INotificationHandler<FileUploaded>
    {
        public BatchProcessorHandler()
        {

        }

        public void Handle(FileUploaded notification)
        {
            var storageAccount = CloudStorageAccount.Parse(
                    ConfigurationService.ConfigurationValue(ConfigurationService.AzureStorageConnectionString));
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference("batches");
            table.CreateIfNotExists();
            var insertOperation = TableOperation.Insert(
                    new BatchTableEntity(notification.BatchId, notification.FileName));
            table.Execute(insertOperation);

        }

        public void Handle(BatchUploadComplete notification)
        {
            var creds = new BatchSharedKeyCredentials(ConfigurationService.ConfigurationValue(ConfigurationService.BatchUrl),
                ConfigurationService.ConfigurationValue(ConfigurationService.BatchAccountName),
                ConfigurationService.ConfigurationValue(ConfigurationService.BatchAccountKey));
            var client = BatchClient.Open(creds);


            var job = client.JobOperations.CreateJob();
            job.Id = notification.BatchId.ToString();
            job.PoolInformation = new PoolInformation {
                PoolId = ConfigurationService.ConfigurationValue(ConfigurationService.BatchPoolId) };
            job.Commit();

            var submissionJob = client.JobOperations.GetJob(notification.BatchId.ToString());
            foreach (var file in GetFilesInBatch(notification.BatchId))
            {
                var task = new CloudTask(file, string.Format("BatchTask.exe \"{0}\" \"{1}\" \"{2}\" \"{3}\"", file, ConfigurationService.ConfigurationValue(ConfigurationService.VisionAPIKey), ConfigurationService.ConfigurationValue(ConfigurationService.AzureStorageBaseUrl), ConfigurationService.ConfigurationValue(ConfigurationService.AzureStorageConnectionString)));
                var programFile = new ResourceFile(ConfigurationService.ConfigurationValue(ConfigurationService.AzureStorageBaseUrl) + "/batchcommand/" + "BatchTask.exe", "BatchTask.exe");
                var newtonsoft = new ResourceFile(ConfigurationService.ConfigurationValue(ConfigurationService.AzureStorageBaseUrl) + "/batchcommand/" + "Newtonsoft.Json.dll", "Newtonsoft.Json.dll");
                var vision = new ResourceFile(ConfigurationService.ConfigurationValue(ConfigurationService.AzureStorageBaseUrl) + "/batchcommand/" + "Microsoft.ProjectOxford.Vision.dll", "Microsoft.ProjectOxford.Vision.dll");
                var storageDll = new ResourceFile(ConfigurationService.ConfigurationValue(ConfigurationService.AzureStorageBaseUrl) + "/batchcommand/" + "Microsoft.WindowsAzure.Storage.dll", "Microsoft.WindowsAzure.Storage.dll");

                var dataFile = new ResourceFile(ConfigurationService.ConfigurationValue(ConfigurationService.AzureStorageBaseUrl) + "/uploadedimages/" + file, file);

                var taskFiles = new List<ResourceFile> { programFile, newtonsoft, vision, dataFile, storageDll };
                task.ResourceFiles = taskFiles;
                submissionJob.AddTask(task);
                submissionJob.Commit();
                submissionJob.Refresh();
            }
            client.Utilities.CreateTaskStateMonitor().WaitAll(submissionJob.ListTasks(), TaskState.Completed, new TimeSpan(0, 30, 0));
            var results = "";
            var errors = "";
            var jsonResults = new Dictionary<string, string>();
            foreach (CloudTask task in submissionJob.ListTasks())
            {
                errors += string.Format("Task {0} says:\n{1}\n\n", task.Id, task.GetNodeFile(Constants.StandardErrorFileName).ReadAsString());
                results += string.Format("Task {0} says:\n{1}\n\n", task.Id, task.GetNodeFile(Constants.StandardOutFileName).ReadAsString());
                
               jsonResults.Add(task.Id, GetJsonFromStorage(task.Id));
            }
            submissionJob.Terminate();
        }
        private IEnumerable<string> GetFilesInBatch(Guid batchId)
        {
            var storageAccount = CloudStorageAccount.Parse(ConfigurationService.ConfigurationValue(ConfigurationService.AzureStorageConnectionString));
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference("batches");
            var query = new TableQuery<BatchTableEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, batchId.ToString()));
            return table.ExecuteQuery(query).Select(x => x.RowKey);
        }

        private string GetJsonFromStorage(string imageUrl)
        {
            var storageAccount = CloudStorageAccount.Parse(ConfigurationService.ConfigurationValue(ConfigurationService.AzureStorageConnectionString));
            var client = storageAccount.CreateCloudBlobClient();
            var container = client.GetContainerReference("output");
            container.CreateIfNotExists();
            var blob = container.GetBlockBlobReference(imageUrl + ".json");

            var stream = blob.OpenRead();
            var bytes = new byte[stream.Length];
            stream.Read(bytes, 0, (int)stream.Length);
            var json = new String(Encoding.UTF8.GetChars(bytes));
            return json;
            
        }
    }
}
