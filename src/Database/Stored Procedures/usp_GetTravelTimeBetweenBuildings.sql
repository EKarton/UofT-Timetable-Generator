CREATE PROCEDURE [dbo].[usp_GetTravelTimeBetweenBuildings]
	@buildingID1 INT,
	@buildingID2 INT
AS
	SELECT * FROM [dbo].[BuildingDistances] T WHERE T.BuildingID1 = @buildingID1 AND T.BuildingID2 = @buildingID2;
RETURN 0
