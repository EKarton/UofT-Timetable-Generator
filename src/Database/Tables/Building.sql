CREATE TABLE [dbo].[Building]
(
	[BuildingID] INT NOT NULL PRIMARY KEY IDENTITY(1, 1), 
    [BuildingName] NVARCHAR(MAX) NULL, 
    [BuildingCode] NVARCHAR(2) NULL, 
    [Address] NVARCHAR(MAX) NULL, 
    [Latitude] FLOAT NULL, 
    [Longitude] FLOAT NULL
)
