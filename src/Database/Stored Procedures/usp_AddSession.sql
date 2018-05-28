CREATE PROCEDURE [dbo].[usp_AddSession]
    @sectionID int NOT NULL, 
    @fallBuildingID int NULL, 
	@fallRoomNumber nvarchar(MAX),
	@winterBuildingID int, 
	@winterRoomNumber nvarchar(MAX) NULL,
    @startTime float NULL, 
    @endTime float NULL
AS
	INSERT INTO [dbo].[Session] (SectionID, Fall_BuildingID, Fall_RoomNumber, Winter_BuildingID, Winter_RoomNumber, StartTime, EndTime)
	VALUES (@sectionID, @fallBuildingID, @fallRoomNumber, @winterBuildingID, @winterRoomNumber, @startTime, @endTime);
RETURN 0
