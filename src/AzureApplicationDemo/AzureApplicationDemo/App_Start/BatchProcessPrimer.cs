using System;
using System.IO;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using AzureApplicationDemo.Services;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AzureApplicationDemo
{
    public class BatchProcessPrimer
    {
        public static void Prime()
        {
            UploadBatchProcessExecutable();
        }

        private static void UploadBatchProcessExecutable()
        {
            var storageAccount = CloudStorageAccount.Parse(ConfigurationService.ConfigurationValue(ConfigurationService.AzureStorageConnectionString));
            var client = storageAccount.CreateCloudBlobClient();
            var container = client.GetContainerReference("batchcommand");
            container.CreateIfNotExists();
            var fileName = "BatchTask.exe";
            var blob = container.GetBlockBlobReference(fileName);
            blob.UploadFromFile( Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/bin"), fileName), FileMode.Open);
        }
        private static void UploadBatchConfigFile()
        {
            var storageAccount = CloudStorageAccount.Parse(ConfigurationService.ConfigurationValue(ConfigurationService.AzureStorageConnectionString));
            var client = storageAccount.CreateCloudBlobClient();
            var container = client.GetContainerReference("batchcommand");
            container.CreateIfNotExists();

            var permissions = new BlobContainerPermissions();
            permissions.PublicAccess = BlobContainerPublicAccessType.Blob;
            container.SetPermissions(permissions);

            var fileName = "BatchTask.exe.config";
            var blob = container.GetBlockBlobReference(fileName);
            blob.UploadFromFile(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/bin"), fileName), FileMode.Open);
        }

    }

}