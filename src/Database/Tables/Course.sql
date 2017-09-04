CREATE TABLE [dbo].[Course]
(
	[CourseID] INT NOT NULL PRIMARY KEY IDENTITY(1, 1), 
    [Code] NVARCHAR(10) NULL, 
	[Campus] NVARCHAR(MAX) NULL,
    [Term] CHAR(1) NULL, 
	[Title] NVARCHAR(MAX) NULL, 
	[Description] NVARCHAR(MAX) NULL 
)
