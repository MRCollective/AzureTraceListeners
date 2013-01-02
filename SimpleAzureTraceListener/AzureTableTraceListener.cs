using System;
using System.Diagnostics;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace SimpleAzureTraceListener
{
    public class AzureTableTraceListener : TraceListener
    {
        private readonly string _applicationName;
        private readonly string _tableName;
        private readonly TableServiceContext _table;

        public AzureTableTraceListener(string applicationName, string tableConnectionString, string tableName = "TraceLogs")
        {
            _applicationName = applicationName;
            _tableName = tableName;
            var storageAccount = CloudStorageAccount.Parse(tableConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            tableClient.CreateTableIfNotExist(tableName);
            _table = tableClient.GetDataServiceContext();
        }

        private void SaveMessage(AzureTraceMessage azureTraceMessage)
        {
            _table.AddObject(_tableName, azureTraceMessage);
            _table.SaveChangesWithRetries();
        }

        public override void Close()
        {
            Flush();
            base.Close();
        }

        #region Write/WriteLine
        public override void Write(string message)
        {
            WriteLine(message);
        }

        public override void Write(object o)
        {
            WriteLine(Convert.ToString(o));
        }

        public override void Write(object o, string category)
        {
            WriteLine(Convert.ToString(o), category);
        }

        public override void Write(string message, string category)
        {
            WriteLine(message, category);
        }

        public override void WriteLine(string message)
        {
            WriteLine(message, null);
        }

        public override void WriteLine(object o)
        {
            WriteLine(Convert.ToString(o), null);
        }

        public override void WriteLine(object o, string category)
        {
            base.WriteLine(Convert.ToString(o), category);
        }

        public override void WriteLine(string message, string category)
        {
            if (string.IsNullOrEmpty(message))
                return;

            var logMessage = new AzureTraceMessage(message, category, _applicationName);
            SaveMessage(logMessage);
        }
        #endregion
    }
}