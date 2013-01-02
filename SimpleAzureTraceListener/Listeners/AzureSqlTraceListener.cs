﻿using System.Data;
using System.Data.SqlClient;
using SimpleAzureTraceListener.Listeners.Base;
using SimpleAzureTraceListener.Models;

namespace SimpleAzureTraceListener.Listeners
{
    public class AzureSqlTraceListener : AzureTraceListener
    {
        private readonly string _tableName;
        private readonly SqlConnection _connection;

        public AzureSqlTraceListener(string applicationName, string sqlConnectionString, string tableName = "TraceLogs")
        {
            _tableName = tableName;
            ApplicationName = applicationName;
            _connection = new SqlConnection(sqlConnectionString);
            _connection.Open();
        }

        protected override void SaveMessage(AzureTraceMessage azureTraceMessage)
        {
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = string.Format(@"INSERT INTO {0} (ApplicationName, Message, Category, Timestamp) VALUES
                                            (@applicationName, @message, @category, @timeStamp)", _tableName);
                command.Parameters.Add(new SqlParameter("applicationName", SqlDbType.NVarChar) { Value = azureTraceMessage.ApplicationName });
                command.Parameters.Add(new SqlParameter("message", SqlDbType.NVarChar) { Value = azureTraceMessage.Message });
                command.Parameters.Add(new SqlParameter("category", SqlDbType.NVarChar) { Value = azureTraceMessage.Category });
                command.Parameters.Add(new SqlParameter("timeStamp", SqlDbType.DateTime2) { Value = azureTraceMessage.Timestamp });
                command.ExecuteNonQuery();
            }
        }
    }
}