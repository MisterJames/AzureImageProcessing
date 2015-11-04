using System;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;

namespace BatchTask
{
    class LogTableEntity : TableEntity
    {
        public string Message { get; set; }
        public LogTableEntity(string partitionKey, string rowKey, string message) : base(partitionKey, rowKey)
        {
            this.Message = message;
        }
    }
}
