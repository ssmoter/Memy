CREATE TABLE [dbo].[CommentReaction]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserId] INT NOT NULL, 
    [CommantId] INT NOT NULL, 
    [Value] INT NOT NULL, 
)
