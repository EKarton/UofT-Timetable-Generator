CREATE TABLE [dbo].[InstructorToActivity]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1), 
    [InstructorID] INT NOT NULL FOREIGN KEY REFERENCES Instructor(Id), 
    [ActivityID] INT NOT NULL FOREIGN KEY REFERENCES Activity(Id)
)
