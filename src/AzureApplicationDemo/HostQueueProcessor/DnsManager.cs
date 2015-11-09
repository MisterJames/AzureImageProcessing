using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.Azure.Management.Dns;
using Microsoft.Azure.Management.Dns.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace HostQueueProcessor
{
    public class DnsManager
    {

        public void CreateDnsRecord(string name)
        {
            var subscriptionId = Environment.GetEnvironmentVariable("mva-subscription-id");

            var azureToken = GetJwtToken();
            var credentials = new TokenCloudCredentials(subscriptionId, azureToken);
            var dnsClient = new DnsManagementClient(credentials);

            // Example: getting a list of records
            //var rslist = dnsClient.RecordSets.ListAllAsync("AzureImageProcessing", "imagenomnom.com", new RecordSetListParameters { Filter = "a" }).Result;
            
            // add host
            var rsWwwA = new RecordSet("global")
            {
                Properties = new RecordSetProperties(60)
                {
                    ARecords = new List<ARecord>
                    {
                        new ARecord("40.112.142.148")
                    }
                }
            };
            var recordParams = new RecordSetCreateOrUpdateParameters(rsWwwA);
            var responseCreateA = dnsClient.RecordSets.CreateOrUpdate(
                    "AzureImageProcessing",
                    "imagenomnom.com",
                    name,
                    RecordType.A,
                    recordParams
                );

        }


        public static string GetJwtToken()
        {
            var tenantId = Environment.GetEnvironmentVariable("mva-azure-tenant-id");
            var clientId = Environment.GetEnvironmentVariable("mva-azure-client-id");
            var clientSecret = Environment.GetEnvironmentVariable("mva-azure-client-secret");

            var authenticationContext = new AuthenticationContext($"https://login.windows.net/{tenantId}");
            var credential = new ClientCredential(clientId: clientId, clientSecret: clientSecret);
            var result = authenticationContext.AcquireToken(resource: "https://management.core.windows.net/", clientCredential: credential);

            if (result == null)
            {
                throw new InvalidOperationException("Failed to obtain the JWT token");
            }

            var token = result.AccessToken;

            return token;
        }
    }
}
