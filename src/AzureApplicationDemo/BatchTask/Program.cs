using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Vision;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.ProjectOxford.Vision.Contract;
using System.IO;

namespace BatchTask
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("starting up batch processing");
            var imageUrl = args[0];
            var visionKey = args[1];
            var baseUrl = args[2];
            var connectionString = args[3];
            var processor = new Processor();
            Console.WriteLine("Processing image " + imageUrl);
            Console.WriteLine("Base URL " + baseUrl);
            Console.WriteLine("connection string " + connectionString);
            
            processor.Process(imageUrl, visionKey, connectionString);

        }
    }

    class Processor
    {
        public void Process(string imageUrl, string visionKey, string connectionString)
        {
            //process with oxford
            scanImage(imageUrl, visionKey, connectionString);
            //process internally
            //report back
        }

        private void scanImage(string imageUrl, string visionKey, string connectionString)
        {
            var visionClient = new VisionServiceClient(visionKey);

            AnalysisResult result;
            if (imageUrl.StartsWith("http"))
                result = visionClient.AnalyzeImageAsync(imageUrl).Result;
            else
            {
                using (FileStream stream = File.Open(imageUrl, FileMode.Open))
                {
                    result = visionClient.AnalyzeImageAsync(stream).Result;
                    var jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                    File.WriteAllText("jsonresult.txt", jsonResult);
                    UploadResult(jsonResult, connectionString, imageUrl);
                }
            }
        }

        private void UploadResult(string json, string connectionString, string imageUrl)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var client = storageAccount.CreateCloudBlobClient();
            var container = client.GetContainerReference("output");
            container.CreateIfNotExists();
            var blob = container.GetBlockBlobReference(imageUrl + ".json");
            var bytes = Encoding.UTF8.GetBytes(json);
            var stream = new MemoryStream(bytes);
            blob.UploadFromStream(stream);
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
