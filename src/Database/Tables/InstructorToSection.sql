CREATE TABLE [dbo].[InstructorToActivity]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1), 
    [InstructorID] INT NOT NULL FOREIGN KEY REFERENCES Instructor([InstructorID]), 
    [SectionID] INT NOT NULL FOREIGN KEY REFERENCES Section([SectionID])
)
