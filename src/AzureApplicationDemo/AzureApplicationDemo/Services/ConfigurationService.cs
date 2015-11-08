using System;
using System.Linq;
using System.Collections.Generic;

namespace AzureApplicationDemo.Services
{
    public static class ConfigurationService
    {
        public const string AzureStorageConnectionString = "mva-azure-storage-connectionstring";
        public const string AzureStorageBaseUrl = "mva-azure-storage-base-url";
        public const string TwitterKey = "mva-twitter-key";
        public const string TwitterSecret = "mva-twitter-secret";

        public const string BatchUrl = "mva-batch-url";
        public const string BatchAccountName = "mva-batch-account-name";
        public const string BatchAccountKey = "mva-batch-account-key";
        public const string BatchPoolId = "mva-batch-pool-id";

        public const string VisionAPIKey = "mva-vision-api-key";



        private static readonly Dictionary<string, string> ConfigurationValues = new Dictionary<string, string>();

        private static readonly IEnumerable<string> KnownConfigKeys = new List<string>
        {
            AzureStorageConnectionString,
            AzureStorageBaseUrl,
            TwitterKey,
            TwitterSecret,
            BatchAccountKey,
            BatchAccountName, 
            BatchUrl,
            BatchPoolId
        };

        static ConfigurationService()
        {
            foreach (var key in KnownConfigKeys)
            {
                ConfigurationValues.Add(key, Environment.GetEnvironmentVariable(key));
            }
        }

        public static string ConfigurationValue(string key)
        {
            return ConfigurationValues[key];
        }
    }
}