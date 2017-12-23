CREATE PROCEDURE [dbo].[usp_GetInstructorInfo]
	@instructorID INT
AS
	SELECT * FROM [dbo].[Instructor] T WHERE T.InstructorID = @instructorID;
RETURN 0
