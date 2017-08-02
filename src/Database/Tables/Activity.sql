CREATE TABLE [dbo].[Activity]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1), 
    [CourseID] INT NOT NULL FOREIGN KEY REFERENCES Course([Id]), 
    [ActivityCode] NVARCHAR(MAX) NULL, 
    [ActivityType] CHAR NULL  
)
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'0 = Lecture, 1 = Tutorial, 2 = Practical',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Activity',
    @level2type = N'COLUMN',
    @level2name = 'ActivityType'