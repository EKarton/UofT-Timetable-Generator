CREATE PROCEDURE [dbo].[usp_GetBuildingInfo]
	@buildingID INT
AS
	SELECT * FROM [dbo].[Building] T WHERE T.BuildingID = @buildingID;
RETURN 0
