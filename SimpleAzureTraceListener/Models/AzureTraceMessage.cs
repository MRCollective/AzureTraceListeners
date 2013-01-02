using System;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;

namespace SimpleAzureTraceListener.Models
{
    public class AzureTraceMessage : TableServiceEntity
    {
        public string Message { get; set; }
        public string Category { get; set; }
        public string ApplicationName { get; set; }
        
        public AzureTraceMessage(string message, string category, string applicationName)
        {
            var rowKey = new StringBuilder(applicationName);
            if (!string.IsNullOrEmpty(category))
                rowKey.AppendFormat(" ({0})", category);

            PartitionKey = DateTime.UtcNow.Ticks.ToString();
            RowKey = rowKey.ToString();
            ApplicationName = applicationName;
            Message = message;
            Category = category;
            Timestamp = DateTime.UtcNow;
        }
    }
}

