using System;
using System.Diagnostics;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using SimpleAzureTraceListener.Listeners.Base;
using SimpleAzureTraceListener.Models;

namespace SimpleAzureTraceListener.Listeners
{
    public class AzureTableTraceListener : AzureTraceListener
    {
        private readonly string _tableName;
        private readonly TableServiceContext _table;

        public AzureTableTraceListener(string applicationName, string tableConnectionString, string tableName = "TraceLogs")
        {
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