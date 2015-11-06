using System;
using System.Collections.Generic;
using System.Linq;
using AzureApplicationDemo.Services;
using MediatR;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AzureApplicationDemo.Features.Upload
{
    public class UploadFileCommandHandler : RequestHandler<UploadFileCommand>
    {
        private IMediator _mediator;
        public UploadFileCommandHandler(IMediator mediator) {
            _mediator = mediator;
        }

        protected override void HandleCore(UploadFileCommand message)
        {
            var storageAccount = CloudStorageAccount.Parse(ConfigurationService.ConfigurationValue(ConfigurationService.AzureStorageConnectionString));
            var client = storageAccount.CreateCloudBlobClient();
            var container = client.GetContainerReference("uploadedimages");
            container.CreateIfNotExists();

            var permissions = new BlobContainerPermissions();
            permissions.PublicAccess = BlobContainerPublicAccessType.Blob;
            container.SetPermissions(permissions);

            var fileName = Guid.NewGuid().ToString();
            var blob = container.GetBlockBlobReference(fileName);
            blob.UploadFromStream(message.Stream);

            _mediator.Publish(new FileUploaded { FileName = fileName, BatchId = message.BatchId });
        }
    }
}
