CREATE TABLE [dbo].[FilerReaction]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserId] INT NOT NULL, 
    [FileSimpleId] INT NOT NULL, 
    [Value] INT NOT NULL, 
)
