using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;

namespace HostQueueProcessor
{
    public class Functions
    {
        public static void ProcessQueueMessage([QueueTrigger("creatednshost")] string message, TextWriter log)
        {

            log.WriteLine(message);
        }
    }
}
