CREATE TABLE [dbo].[Section]
(
	[SectionID] INT NOT NULL PRIMARY KEY IDENTITY(1, 1), 
    [ActivityID] INT NOT NULL FOREIGN KEY REFERENCES Activity([ActivityID]), 
    [SectionCode] NVARCHAR(MAX) NULL,
)
