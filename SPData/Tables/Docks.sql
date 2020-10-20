CREATE TABLE [dbo].[Docks]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [ManagerId] INT NOT NULL, 
    [Max Capacity] INT NOT NULL DEFAULT 6, 
    [Current Capacity] INT NOT NULL, 
    CONSTRAINT [FK_Dock_ToDockManager] FOREIGN KEY ([ManagerId]) REFERENCES [DockManagers]([Id]),
)
