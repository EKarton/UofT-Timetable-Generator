CREATE PROCEDURE [dbo].[usp_AddSection]
	@activityID int,
	@sectionCode NVARCHAR(MAX)
AS
	INSERT INTO [dbo].[Section] (ActivityID, SectionCode) VALUES(@activityID, @sectionCode);
RETURN 0
