using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
    public class BatchTableEntity : TableEntity
    {
        public BatchTableEntity(Guid batchId, string fileName)
        {
            this.PartitionKey = batchId.ToString();
            this.RowKey = fileName;
        }
        public BatchTableEntity() { }
    }

    public class BatchProcessorHandler : INotificationHandler<BatchUploadComplete>, INotificationHandler<FileUploaded>
    {
        public BatchProcessorHandler()
        {

        }

        public void Handle(FileUploaded notification)
        {
            var storageAccount = CloudStorageAccount.Parse(ConfigurationService.ConfigurationValue(ConfigurationService.AzureStorageConnectionString));
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference("batches");
            table.CreateIfNotExists();
            var insertOperation = TableOperation.Insert(new BatchTableEntity(notification.BatchId, notification.FileName));
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
            job.PoolInformation = new PoolInformation { PoolId = ConfigurationService.ConfigurationValue(ConfigurationService.BatchPoolId) };
            job.Commit();

            var submissionJob = client.JobOperations.GetJob(notification.BatchId.ToString());
            foreach (var file in GetFilesInBatch(notification.BatchId))
            {
                var task = new CloudTask(file, string.Format("BatchTask.exe \"{0}\" \"{1}\"", file, ConfigurationService.ConfigurationValue(ConfigurationService.VisionAPIKey) ));
                var programFile = new ResourceFile(ConfigurationService.ConfigurationValue(ConfigurationService.AzureStorageBaseUrl) + "/batchcommand/" + "BatchTask.exe", "BatchTask.exe");
                var dataFile = new ResourceFile(ConfigurationService.ConfigurationValue(ConfigurationService.AzureStorageBaseUrl) + "/uploadedimages/" + file, file);

                var taskFiles = new List<ResourceFile> { programFile, dataFile };
                task.ResourceFiles = taskFiles;
                submissionJob.AddTask(task);
                submissionJob.Commit();
                submissionJob.Refresh();
            }
            client.Utilities.CreateTaskStateMonitor().WaitAll(submissionJob.ListTasks(), TaskState.Completed, new TimeSpan(0, 30, 0));
            var results = "";
            foreach (CloudTask task in submissionJob.ListTasks())
            {
                results += "Task " + task.Id + " says:\n" + task.GetNodeFile(Constants.StandardOutFileName).ReadAsString() + "\n\n";
                var json = task.GetNodeFile("jsonresult.txt");
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
    }
}
