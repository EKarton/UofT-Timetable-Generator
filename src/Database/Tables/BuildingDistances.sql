CREATE TABLE [dbo].[BuildingDistances]
(
	[BuildingDistanceID] INT NOT NULL PRIMARY KEY IDENTITY(1, 1), 
    [BuildingID1] INT NOT NULL FOREIGN KEY REFERENCES Building([BuildingID]), 
    [BuildingID2] INT NOT NULL FOREIGN KEY REFERENCES Building([BuildingID]), 
    [WalkingDuration] FLOAT NULL, 
    [WalkingDistance] FLOAT NULL, 
    [TransitDuration] FLOAT NULL, 
    [TransitDistance] FLOAT NULL, 
    [CyclingDuration] FLOAT NULL, 
    [CyclingDistance] FLOAT NULL, 
    [DrivingDuration] FLOAT NULL, 
    [DrivingDistance] FLOAT NULL, 
)
