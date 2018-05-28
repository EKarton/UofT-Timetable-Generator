CREATE PROCEDURE [dbo].[usp_AddInstructorToSection]
	@instructorID int,
	@sectionID int
AS
	INSERT INTO [dbo].[InstructorToSection] (InstructorID, SectionID) VALUES (@instructorID, @sectionID);
RETURN 0
