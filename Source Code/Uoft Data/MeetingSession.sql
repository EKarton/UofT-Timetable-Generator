CREATE TABLE [dbo].[MeetingSession]
(
	[SessionID] INT NOT NULL PRIMARY KEY, 
    [CourseID] INT NOT NULL FOREIGN KEY REFERENCES Course(CourseID), 
    [MeetingCode] VARCHAR(MAX) NULL, 
    [Type] TINYINT NULL DEFAULT 0
)
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'0 = Lecture, 1 = Tutorial, 2 = Practical',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'MeetingSession',
    @level2type = N'COLUMN',
    @level2name = N'Type'