CREATE TABLE [dbo].[AnswerComment]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [CommentId] INT NOT NULL, 
    [UserId] INT NOT NULL, 
    [Date] DATETIMEOFFSET NOT NULL, 
    [Description] NVARCHAR(MAX) NOT NULL
)
