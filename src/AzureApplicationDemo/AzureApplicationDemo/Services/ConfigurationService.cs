using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureApplicationDemo.Services
{
    public static class ConfigurationService
    {
        public const string AzureStorageConnectionString = "mva-azure-storage-connectionstring";
        public const string TwitterKey = "mva-twitter-key";
        public const string TwitterSecret = "mva-twitter-secret";

        private static readonly Dictionary<string, string> ConfigurationValues = new Dictionary<string, string>();

        private static readonly IEnumerable<string> KnownConfigKeys = new List<string>
        {
            AzureStorageConnectionString,
            TwitterKey,
            TwitterSecret
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