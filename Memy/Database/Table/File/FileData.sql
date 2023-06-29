CREATE TABLE [dbo].[FileData]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FileSimpleId] INT NOT NULL, 
    [ObjName] NVARCHAR(MAX) NOT NULL, 
    [ObjType] INT NOT NULL, 
    [ObjOrder] INT NOT NULL
)
