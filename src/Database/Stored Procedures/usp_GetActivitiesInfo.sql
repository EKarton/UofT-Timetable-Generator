CREATE PROCEDURE [dbo].[usp_GetActivitiesInfo]
	@courseID INT
AS
	SELECT * FROM [dbo].[Activity] T WHERE T.CourseID = @courseID;
RETURN 0
