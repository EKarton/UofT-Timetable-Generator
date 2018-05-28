CREATE PROCEDURE [dbo].[usp_AddInstructor]
	@name nvarchar(MAX),
	@rating int NULL
AS
	INSERT INTO [dbo].[Instructor] (Name, Rating) VALUES(@name, @rating);
RETURN 0
