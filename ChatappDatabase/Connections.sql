CREATE TABLE [dbo].[Connections]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [ProfileId] INT UNIQUE NOT NULL, 
    [SignalId] NVARCHAR(22),
    [DateTime] DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [FK_Messages_ProfileId_To_Profiles] FOREIGN KEY (ProfileId) REFERENCES dbo.Profiles(Id),
)
