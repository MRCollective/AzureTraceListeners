AzureTraceListeners
========================

Introduction
------------

A simple set of Azure trace listeners for Table Storage, Queue Storage and Azure SQL - useful when Diagnostics is not available (ie when using the Azure Websites functionality) or when only simple logging is desired.

Configuration
-------------

See below for specific examples.

ApplicationName can be any value you wish to group trace logs on.

ConnectionString should be:

a storage client string for Tables and Queues ie:

    DefaultEndpointsProtocol=https;AccountName=ACCOUNTNAME;AccountKey=ACCOUNTKEY or useDevelopmentStorage=true
	
an ADO.NET connection string for Azure SQL ie:

    Server=tcp:dbserver.database.windows.net,1433;Database=dbname;User ID=dbuser@dbserver;Password=dbpassword;Trusted_Connection=False;Encrypt=True;

Table/Queue name defaults to TraceLogs - it can be customised as per the usual rules for Azure Table and Queue names. 

Setting up the AzureTableTraceListener
--------------------------------------

    Trace.Listeners.Add(new AzureTableTraceListener(applicationName, connectionString, tableName));

Setting up the AzureQueueTraceListener
--------------------------------------

    Trace.Listeners.Add(new AzureQueueTraceListener(applicationName, connectionString, queueName));
	
Setting up the AzureSqlTraceListener
------------------------------------

    CREATE TABLE [dbo].[TraceLogs] (
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [ApplicationName] [nvarchar](max) NOT NULL,
        [Category] [nvarchar](max) NULL,
        [Timestamp] [datetime2](7) NOT NULL,
        [Message] [nvarchar](max) NOT NULL,
        CONSTRAINT [PK_TraceLogs] PRIMARY KEY CLUSTERED 
        (
            [Id] ASC
        )
    )
    
    Trace.Listeners.Add(new AzureSqlTraceListener(applicationName, connectionString, tableName));
    
Logging
-------

After adding any desired listeners, trace messages from your code will be automatically logged:

    Trace.WriteLine("Testing logging!");
	
You can optionally specify a standard trace category:

    Trace.WriteLine("Testing an error...", "Error");
	
More advanced trace functionality (stack traces, etc) is not supported in this release.
	
Contributions
-------------

If you'd like to leave feedback or contributions, please contact me on Twitter @mdaviesnet or send a pull request.
	
Roadmap
-------

* Improved documentation
* Splitting the listeners such that if you only need the Azure SQL listener, you don't need to include the table storage reference?
* As part of the above, remove the Azure SQL / Queue listeners' use of TableServiceEntity
* Add Transient Fault Handling to the Azure SQL listener
* Add retry logic if a trace message cannot be logged?
* Ability to customise/extend the fields to be logged
* Add more advanced trace functionality support