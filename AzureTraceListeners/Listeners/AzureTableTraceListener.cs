using System;
using AzureTraceListeners.Listeners.Base;
using AzureTraceListeners.Models;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace AzureTraceListeners.Listeners
{
    public class AzureTableTraceListener : AzureTraceListener
    {
        private readonly string _tableName;
        private readonly TableServiceContext _table;

        public AzureTableTraceListener(string applicationName, string tableConnectionString, string tableName = "TraceLogs")
        {
            if (string.IsNullOrEmpty(applicationName))
                throw new ArgumentNullException("applicationName",
                    "You must define an ApplicationName to log trace messages");

            ApplicationName = applicationName;
            _tableName = tableName;
            var storageAccount = CloudStorageAccount.Parse(tableConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            tableClient.CreateTableIfNotExist(tableName);
            _table = tableClient.GetDataServiceContext();
        }

        protected override void SaveMessage(AzureTraceMessage azureTraceMessage)
        {
            _table.AddObject(_tableName, azureTraceMessage);
            _table.SaveChangesWithRetries();
        }
    }
}