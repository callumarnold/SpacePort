CREATE TABLE [dbo].[Spaceships]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Owner] NVARCHAR(50) NOT NULL, 
    [Crew Size] INT NOT NULL, 
    [DockId] INT NOT NULL, 
    CONSTRAINT [FK_Spaceship_ToDock] FOREIGN KEY ([DockId]) REFERENCES [Docks]([Id]) ON DELETE CASCADE
)
