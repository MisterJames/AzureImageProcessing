using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using Microsoft.WindowsAzure.Storage;

namespace AzureApplicationDemo.Features.Upload
{
    public class UploadFileCommandHandler : RequestHandler<UploadFileCommand>
    {
        protected override void HandleCore(UploadFileCommand message)
        {
            var storageAccount = CloudStorageAccount.Parse(System.Configuration.ConfigurationManager.AppSettings["StorageConnectionString"]);
            var client = storageAccount.CreateCloudBlobClient();
            var container = client.GetContainerReference("uploadedimages");
            container.CreateIfNotExists();
            var blob = container.GetBlockBlobReference(Guid.NewGuid().ToString());
            blob.UploadFromStream(message.Stream);
        }
    }
}
