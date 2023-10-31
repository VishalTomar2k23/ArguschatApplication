CREATE TABLE [dbo].[Profiles]
(
	[Id] INT PRIMARY KEY IDENTITY (1, 1) NOT NULL, 
    [FirstName] NVARCHAR(1000) NOT NULL, 
    [LastName] NVARCHAR(1000) NULL, 
    [UserName] NVARCHAR(1000) NOT NULL, 
    [Email ] NVARCHAR(1000) NOT NULL, 
    [Password] NVARCHAR(MAX) NOT NULL, 
    [ProfileType] INT NOT NULL,
    [CreatedAt] DATETIME2 NULL DEFAULT GETDATE(), 
    [CreatedBy] INT NULL, 
    [LastUpdatedAt] DATETIME2 NULL, 
    [LastUpdatedBy] INT NULL, 
    [ImagePath] NVARCHAR(1000) NULL, 
    [Designation] INT NOT NULL DEFAULT 1 ,
    [Status] NVARCHAR(40)  NULL ,
    [IsDeleted] INT NULL
)
