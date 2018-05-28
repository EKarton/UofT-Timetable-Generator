CREATE PROCEDURE [dbo].[usp_AddActivity]
	@courseID int,
	@activityType char(1)
AS
	INSERT INTO [dbo].[Activity] (CourseID, ActivityType) VALUES(@courseID, @activityType);
RETURN 0
