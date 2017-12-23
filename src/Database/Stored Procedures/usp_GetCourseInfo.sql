CREATE PROCEDURE [dbo].[usp_GetCourseInfo] @courseCode NVARCHAR(10)
AS
	SELECT * FROM [dbo].[Course] c WHERE c.Code LIKE '%' + @courseCode + '%';
RETURN 0
