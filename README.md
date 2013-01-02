SimpleAzureTraceListener
========================

Documentation to come.

SQL for creating the Azure SQL Log Table. You can configure the table name as desired.

CREATE TABLE [dbo].[TraceLogs] (
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationName] [nvarchar](max) NOT NULL,
	[Category] [nvarchar](max) NULL,
	[Timestamp] [datetime2](7) NOT NULL,
	[Message] [nvarchar](max) NOT NULL
)