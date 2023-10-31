CREATE TABLE [dbo].[Notifications]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [SenderId] INT NOT NULL, 
    [ReceiverId] INT NOT NULL, 
    [GroupId] INT NULL, 
    [Content] NVARCHAR(50) NOT NULL,
    [DateTime] DATETIME2 NOT NULL, 
    CONSTRAINT [FK_Notifications_FromId_To_Profiles] FOREIGN KEY (SenderId) REFERENCES dbo.PRofiles(Id),
    CONSTRAINT [FK_Notifications_ToId_To_Profiles] FOREIGN KEY (ReceiverId) REFERENCES dbo.Profiles(Id),
    CONSTRAINT [FK_Notifications_GroupID_To_Groups] FOREIGN KEY (GroupId) REFERENCES dbo.Groups(Id),
)
