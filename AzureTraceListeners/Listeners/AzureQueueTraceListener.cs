using System;
using System.Web.Script.Serialization;
using AzureTraceListeners.Listeners.Base;
using AzureTraceListeners.Models;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace AzureTraceListeners.Listeners
{
    public class AzureQueueTraceListener : AzureTraceListener
    {
        private readonly CloudQueue _queue;

        public AzureQueueTraceListener(string applicationName, string queueConnectionString, string queueName = "tracelogs")
        {
            if (string.IsNullOrEmpty(ApplicationName))
                throw new ArgumentNullException("applicationName",
                    "You must define an ApplicationName to log trace messages");

            ApplicationName = applicationName;
            var storageAccount = CloudStorageAccount.Parse(queueConnectionString);
            var queueClient = storageAccount.CreateCloudQueueClient();
            _queue = queueClient.GetQueueReference(queueName);
            _queue.CreateIfNotExist();
        }

        protected override void SaveMessage(AzureTraceMessage azureTraceMessage)
        {
            var serializedMessage = new JavaScriptSerializer().Serialize(azureTraceMessage);
            _queue.AddMessage(new CloudQueueMessage(serializedMessage));
        }
    }
}