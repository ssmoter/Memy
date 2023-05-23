CREATE TABLE [dbo].[FileTagConnected]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FileSimpleId] INT NOT NULL, 
    [FileTagListId] INT NOT NULL
)
