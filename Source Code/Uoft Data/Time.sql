CREATE TABLE [dbo].[Time]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [SessionID] INT NOT NULL FOREIGN KEY REFERENCES MeetingSession(SessionID), 
    [BuildingID] INT NULL FOREIGN KEY REFERENCES Building(BuildingID), 
    [StartTime] FLOAT NULL, 
    [EndTime] FLOAT NULL, 
    [Weekday] TINYINT NULL DEFAULT 1
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'0 = Sunday, 1 = Monday, 2 = Tuesday, 3 = Wednesday, 4 = Thursday, 5 = Friday, 6 = Saturday',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Time',
    @level2type = N'COLUMN',
    @level2name = N'Weekday'