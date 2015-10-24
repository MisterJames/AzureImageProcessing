using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AzureApplicationDemo.Features.Common;
using AzureApplicationDemo.Services;
using MediatR;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace AzureApplicationDemo.Features.Onboarding
{
    public class QueueHostHandler : RequestHandler<QueueHostCommand>
    {

        protected override void HandleCore(QueueHostCommand message)
        {
            var storageAccount = CloudStorageAccount.Parse(ConfigurationService.ConfigurationValue(ConfigurationService.AzureStorageConnectionString));
            var client = storageAccount.CreateCloudQueueClient();
            var queue = client.GetQueueReference("creatednshost");
            queue.CreateIfNotExists();

            var hostInfo = new HostInformation
            {
                HostName = message.Host.HostName,
                UserId = message.Host.UserId
            };

            var entry = new CloudQueueMessage(JsonConvert.SerializeObject(hostInfo));
            queue.AddMessage(entry);
        }
    }
}