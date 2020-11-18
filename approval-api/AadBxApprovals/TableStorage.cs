

using Azure;
using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace AadBxApprovals
{
    public class TableStorage
    {
        private const string TableName = "aadb2cusers";

        public async static Task<TableClient> GetTableClientAsync()
        {
            var accountName = Environment.GetEnvironmentVariable("CLOUDOVEN_STORAGE_ACCOUNT");
            var key = Environment.GetEnvironmentVariable("CLOUDOVEN_STORAGE_KEY");
            var cs = $"DefaultEndpointsProtocol=https;AccountName={accountName};AccountKey={key};EndpointSuffix=core.windows.net";
            var tableClient = new TableServiceClient(cs).GetTableClient(TableName);
            await tableClient.CreateIfNotExistsAsync();
            return tableClient;
        }

        public async static Task<PayloadEntity> GetAsync(string email)
        {
            var (pk, rk) = GetKeys(email);
            var tc = await GetTableClientAsync();
            var results = tc.Query<PayloadEntity>(filter: $"PartitionKey eq '{pk}' and RowKey eq '{rk}'");
            return results.FirstOrDefault();
        }

        public async static Task AddAsync(string email)
        {
            var (pk, rk) = GetKeys(email);
            var tc = await GetTableClientAsync();
            var entity = new PayloadEntity
            {
                State = ApprovalState.Pending,
                Email = email,
                PartitionKey = pk,
                RowKey = rk
            };
            await tc.AddEntityAsync(entity);
        }

        public static Tuple<string, string> GetKeys(string email) 
            => new Tuple<string, string>(GetPartitionKey(email), GetRowKey(email));

        public static string GetRowKey(string email) => email.Substring(0, email.IndexOf("@"));
        
        public static string GetPartitionKey(string email) => email.Substring(email.IndexOf("@") + 1);
    }

    public class PayloadEntity : ITableEntity
    {
        public string Email { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        public ApprovalState State { get; set; }
    }

    public enum ApprovalState
    {
        Pending,
        Approved,
        Denied
    }
}
