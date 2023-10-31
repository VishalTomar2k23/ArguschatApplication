CREATE TABLE [dbo].[Groups]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [GroupName] NVARCHAR(50)  NOT NULL, 
    [CreatedAt] DATETIME2 NULL, 
    [CreatedBy] INT NULL, 
    [ImagePath] NVARCHAR(1000) NULL, 
    [Description] NVARCHAR(200) NULL 
    CONSTRAINT [FK_Groups_CreatedBy_To_Profiles] FOREIGN KEY (CreatedBy) REFERENCES dbo.Profiles(Id),
)

