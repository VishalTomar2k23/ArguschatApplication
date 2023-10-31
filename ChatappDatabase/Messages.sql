CREATE TABLE [dbo].[Messages]
(
	[Id] INT PRIMARY KEY IDENTITY (1, 1) NOT NULL, 
    [Content] NVARCHAR(1000) NOT NULL, 
    [SenderId] INT NOT NULL, 
    [ReceiverId] INT NOT NULL,
    [DateTime] DATETIME2 NOT NULL, 
    [RepliedToId] INT NOT NULL DEFAULT 0,
    [RepliedContent] NVARCHAR(1000) NULL,
    [IsReply] INT NOT NULL DEFAULT 0,
    [IsSeen] INT NOT NULL DEFAULT 0, 
    [Type] NVARCHAR(30) NULL ,
)
