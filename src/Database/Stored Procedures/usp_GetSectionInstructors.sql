CREATE PROCEDURE [dbo].[usp_GetSectionInstructors]
	@sectionID INT
AS
	SELECT * FROM [dbo].[InstructorToSection] T WHERE T.SectionID = @sectionID;
RETURN 0
