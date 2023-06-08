CREATE TABLE [dbo].[Comment]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FileSimpleId] INT NOT NULL, 
    [UserId] INT NOT NULL, 
    [Date] DATETIMEOFFSET NOT NULL, 
    [Description] NVARCHAR(MAX) NOT NULL
)
