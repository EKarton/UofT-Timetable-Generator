CREATE PROCEDURE [dbo].[usp_GetSessionInfo]
	@sectionID INT
AS
	SELECT * FROM [dbo].[Session] T WHERE T.SectionID = @sectionID;
RETURN 0
