CREATE TABLE [dbo].[Session]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1), 
    [ActivityID] INT NOT NULL FOREIGN KEY REFERENCES Activity([Id]), 
    [Fall_BuildingID] INT NULL FOREIGN KEY REFERENCES Building([Id]), 
	[Fall_RoomNumber] NVARCHAR(MAX) NULL,
	[Winter_BuildingID] INT NULL FOREIGN KEY REFERENCES Building([Id]), 
	[Winter_RoomNumber] NVARCHAR(MAX) NULL,
    [StartTime] FLOAT NULL, 
    [EndTime] FLOAT NULL
)