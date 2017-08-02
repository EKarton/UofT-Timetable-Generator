CREATE TABLE [dbo].[Course]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1), 
    [Code] NVARCHAR(10) NULL, 
    [Title] NVARCHAR(MAX) NULL, 
    [Term] CHAR(1) NULL 
)
