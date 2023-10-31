CREATE TABLE [dbo].[GroupMessages]
(
	[Id] INT PRIMARY KEY IDENTITY (1, 1) NOT NULL, 
    [GroupId] INT NOT NULL,
    [Content] NVARCHAR(1000) NOT NULL, 
    [SenderId] INT NOT NULL, 
    [CreatedAt] DATETIME2 NOT NULL, 
    [RepliedToId] INT NOT NULL DEFAULT 0,
    [RepliedContent] NVARCHAR(1000) NULL,
    [IsReply] INT NOT NULL DEFAULT 0,
    [Type] NVARCHAR(30) NULL ,  
    CONSTRAINT [FK_GroupMessages_GrpId_To_Groups] FOREIGN KEY (GroupId) REFERENCES dbo.Groups(Id),
)
