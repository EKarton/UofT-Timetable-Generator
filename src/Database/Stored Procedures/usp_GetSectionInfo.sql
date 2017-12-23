CREATE PROCEDURE [dbo].[usp_GetSectionInfo]
	@activityID INT
AS
	SELECT * FROM [dbo].[Section] T WHERE T.ActivityID = @activityID;
RETURN 0
