CREATE TABLE [dbo].[Session]
(
	[SessionID] INT NOT NULL PRIMARY KEY IDENTITY(1, 1), 
    [SectionID] INT NOT NULL FOREIGN KEY REFERENCES Section([SectionID]), 
    [Fall_BuildingID] INT NULL FOREIGN KEY REFERENCES Building([BuildingID]), 
	[Fall_RoomNumber] NVARCHAR(MAX) NULL,
	[Winter_BuildingID] INT NULL FOREIGN KEY REFERENCES Building([BuildingID]), 
	[Winter_RoomNumber] NVARCHAR(MAX) NULL,
    [StartTime] FLOAT NULL, 
    [EndTime] FLOAT NULL
)