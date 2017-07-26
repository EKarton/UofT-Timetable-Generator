CREATE TABLE [dbo].[BuildingDistances]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [BuildingID1] INT NOT NULL FOREIGN KEY REFERENCES Building(BuildingID), 
    [BuildingID2] INT NOT NULL FOREIGN KEY REFERENCES Building(BuildingID), 
    [WalkingDuration] BIGINT NULL, 
    [WalkingDistance] FLOAT NULL, 
    [TransitDuration] BIGINT NULL, 
    [TransitDistance] FLOAT NULL, 
    [CyclingDuration] BIGINT NULL, 
    [CyclingDistance] FLOAT NULL, 
    [VehicleDuration] BIGINT NULL, 
    [VehicleDistance] FLOAT NULL, 
)
