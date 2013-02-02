using System.Configuration;
using System.Data;
using System.Diagnostics;
using AzureTraceListeners.Listeners;
using AzureTraceListeners.Models;
using NUnit.Framework;
using ServiceStack.OrmLite;

namespace AzureTraceListeners.Tests.Listeners
{
    [TestFixture]
    public class AzureSqlTraceListenerShould
    {
        private IDbConnection _connection;
        private string _connectionString;

        [SetUp]
        public void Setup()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
            var dbFactory = new OrmLiteConnectionFactory(_connectionString, SqlServerDialect.Provider);
            _connection = dbFactory.Open();
            _connection.DropAndCreateTable<AzureTraceMessage>();
        }

         [Test]
         public void Suppress_exceptions_by_default()
         {
             _connection.DropTable<AzureTraceMessage>();

             Assert.DoesNotThrow(() => Trace.WriteLine("Test", "Warning"));
         }

        [Test]
        public void Correctly_log_all_fields([Values(true, false)] bool useCategoryExtension)
        {
            const string applicationName = "Tests";
            const string testMessage = "Test Message";
            const string testCategory = "Error";

            Trace.Listeners.Add(new AzureSqlTraceListener(applicationName, _connectionString, "AzureTraceMessage"));

            try
            {
                if (useCategoryExtension)
                    Trace.TraceError(testMessage);
                else
                    Trace.WriteLine(testMessage, testCategory);

                var messages = _connection.Select<AzureTraceMessage>();
                Assert.That(messages, Has.Count.EqualTo(1));
                Assert.That(messages[0].ApplicationName, Is.EqualTo(applicationName));
                Assert.That(messages[0].Category, Is.EqualTo(testCategory));
                Assert.That(messages[0].Message, Is.EqualTo(testMessage));
            }
            finally
            {
                _connection.DeleteAll<AzureTraceMessage>();
            }
        }
    }
}