AzureTraceListeners
========================

Setting up the AzureTableTraceListener
------------------------------------

    Trace.Listeners.Add(new AzureTableTraceListener(applicationName, connectionString, tableName));

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
    
Roadmap
-------

* Improved documentation
* Splitting the listeners such that if you only need the Azure SQL listener, you don't need to include the table storage reference?
* As part of the above, remove the Azure SQL listener's use of TableServiceEntity
* Add Transient Fault Handling to the Azure SQL listener
* Add retry logic if a trace message cannot be logged?
* Ability to customise/extend the fields to be logged