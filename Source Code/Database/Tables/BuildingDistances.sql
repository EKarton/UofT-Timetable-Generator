CREATE TABLE [dbo].[BuildingDistances]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1), 
    [BuildingID1] INT NOT NULL FOREIGN KEY REFERENCES Building([Id]), 
    [BuildingID2] INT NOT NULL FOREIGN KEY REFERENCES Building([Id]), 
    [WalkingDuration] FLOAT NULL, 
    [WalkingDistance] FLOAT NULL, 
    [TransitDuration] FLOAT NULL, 
    [TransitDistance] FLOAT NULL, 
    [CyclingDuration] FLOAT NULL, 
    [CyclingDistance] FLOAT NULL, 
    [VehicleDuration] FLOAT NULL, 
    [VehicleDistance] FLOAT NULL, 
)
