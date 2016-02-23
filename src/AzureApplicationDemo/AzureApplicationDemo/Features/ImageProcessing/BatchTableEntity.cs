using System;
using System.Collections.Generic;
using System.Linq;
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
}
