using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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