CREATE TABLE [dbo].[FileData]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FileSimpleId] INT NOT NULL, 
    [ImgName] NVARCHAR(MAX) NULL, 
    [ImgType] NVARCHAR(10) NULL
)
