using System;
using System.IO;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using AzureApplicationDemo.Services;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.Azure.Batch.Auth;
using Microsoft.Azure.Batch;
using System.Collections.Generic;

namespace AzureApplicationDemo
{
    public class BatchProcessPrimer
    {
     
        public static void Prime()
        {
            UploadBatchProcessExecutable();
            SetContainerPermission();
            CreatePool();
        }


        private static void UploadBatchProcessExecutable()
        {
            var storageAccount = CloudStorageAccount.Parse(ConfigurationService.ConfigurationValue(ConfigurationService.AzureStorageConnectionString));
            var client = storageAccount.CreateCloudBlobClient();
            var container = client.GetContainerReference("batchcommand");
            container.CreateIfNotExists();
            foreach (var fileName in new List<string> { "BatchTask.exe", "Newtonsoft.Json.dll", "Microsoft.ProjectOxford.Vision.dll", "Microsoft.WindowsAzure.Storage.dll" })
            {
                var blob = container.GetBlockBlobReference(fileName);
                blob.UploadFromFile(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/bin"), fileName), FileMode.Open);
            }
        }
      
        static void SetContainerPermission()
        {
            var storageAccount = CloudStorageAccount.Parse(ConfigurationService.ConfigurationValue(ConfigurationService.AzureStorageConnectionString));
            var client = storageAccount.CreateCloudBlobClient();
            var container = client.GetContainerReference("batchcommand");

            var permissions = new BlobContainerPermissions();
            permissions.PublicAccess = BlobContainerPublicAccessType.Blob;
            container.SetPermissions(permissions);
        }

        private static void CreatePool()
        {
            var creds = new BatchSharedKeyCredentials(
                                ConfigurationService.ConfigurationValue(ConfigurationService.BatchUrl),
                                ConfigurationService.ConfigurationValue(ConfigurationService.BatchAccountName),
                                ConfigurationService.ConfigurationValue(ConfigurationService.BatchAccountKey));
            var client = BatchClient.Open(creds);

            var poolId = ConfigurationService.ConfigurationValue(ConfigurationService.BatchPoolId);
            var pool = client.PoolOperations.GetPool(poolId);
            if (pool == null)
            { 
                pool = client.PoolOperations.CreatePool(poolId, "4", "small", 3);
                pool.Commit();
            }

        }

    }

}