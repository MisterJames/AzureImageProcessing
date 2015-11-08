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
            var processor = new Processor();
            Console.WriteLine("Processing image " + imageUrl);
            processor.Process(imageUrl, visionKey);

        }
    }

    class Processor
    {
        public void Process(string imageUrl, string visionKey)
        {
            //process with oxford
            scanImage(imageUrl, visionKey);
            //process internally
            //report back
        }

        private void scanImage(string imageUrl, string visionKey)
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
                }
            }
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
