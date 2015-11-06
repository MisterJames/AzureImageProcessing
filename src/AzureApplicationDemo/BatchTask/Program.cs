using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace BatchTask
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("starting up batch processing");
            var imageUrl = args[0];
            var processor = new Processor();
            Console.WriteLine("Processing image " + imageUrl);
            processor.Process(imageUrl);

        }
    }

    class Processor
    {
        public void Process(string imageUrl)
        {
            var data = RetrieveImageFromUrl(imageUrl);
            //process with oxford
            //process internally
            //report back
        }

        private Bitmap RetrieveImageFromUrl(string imageUrl)
        {
            var storageAccount = CloudStorageAccount.Parse(System.Configuration.ConfigurationManager.AppSettings["StorageConnectionString"]);
            var client = storageAccount.CreateCloudBlobClient();
            var tableClient = storageAccount.CreateCloudTableClient();
            LogToTableStorage(imageUrl, tableClient);

            var container = client.GetContainerReference("uploadedimages");
            container.CreateIfNotExists();
            var blob = container.GetBlockBlobReference(Guid.NewGuid().ToString());
            return new Bitmap(Image.FromStream(blob.OpenRead()));

        }

        private static void LogToTableStorage(string imageUrl, CloudTableClient tableClient)
        {
            var table = tableClient.GetTableReference("logs");
            table.CreateIfNotExists();
            var utcNow = DateTime.UtcNow;
            var insertOperation = TableOperation.Insert(new LogTableEntity(imageUrl, utcNow.ToLongDateString() + " " + utcNow.ToLongTimeString(), "Processing file"));
            table.Execute(insertOperation);
        }
    }
}
