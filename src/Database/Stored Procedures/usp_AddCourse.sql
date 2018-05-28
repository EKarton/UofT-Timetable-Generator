CREATE PROCEDURE [dbo].[usp_AddCourse]
	@courseCode nvarchar(10),
	@campus nvarchar(MAX),
	@term char(1),
	@title nvarchar(MAX),
	@description nvarchar(MAX)
AS
	INSERT INTO [dbo].[Course] (Code, Campus, Term, Description) VALUES (@courseCode, @campus, @term, @description);
RETURN 0
