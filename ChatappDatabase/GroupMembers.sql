CREATE TABLE [dbo].[GroupMembers]
(
	[Id] INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
    [ProfileId] INT NOT NULL, 
    [GroupId] INT NOT NULL,
    [JoinedAt] DATETIME2 NOT NULL, 
    [Admin] INT NOT NULL DEFAULT 0,   
    CONSTRAINT [FK_GroupMembers_GrpId_To_Groups] FOREIGN KEY (GroupId) REFERENCES dbo.Groups(Id),
    CONSTRAINT [FK_GroupMembers_ProfileId_To_Profiles] FOREIGN KEY (ProfileId) REFERENCES dbo.Profiles(Id),
)
