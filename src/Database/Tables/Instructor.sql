CREATE TABLE [dbo].[Instructor]
(
	[InstructorID] INT NOT NULL PRIMARY KEY IDENTITY(1, 1), 
    [Name] NVARCHAR(MAX) NULL, 
    [Rating] FLOAT NULL
)
