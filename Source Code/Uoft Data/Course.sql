CREATE TABLE [dbo].[Course]
(
	[CourseID] INT NOT NULL PRIMARY KEY, 
    [CourseCode] NCHAR(10) NULL, 
    [Name] NVARCHAR(MAX) NULL, 
    [Term] NCHAR(1) NULL 
)
