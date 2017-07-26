CREATE TABLE [dbo].[Building]
(
	[BuildingID] INT NOT NULL PRIMARY KEY, 
    [BuildingName] NVARCHAR(MAX) NULL, 
    [BuildingCode] NCHAR(10) NULL, 
    [Address] NVARCHAR(MAX) NULL, 
    [Latitude] INT NULL, 
    [Longitude] INT NULL
)
