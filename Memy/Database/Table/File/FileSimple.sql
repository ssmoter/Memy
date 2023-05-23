CREATE TABLE [dbo].[FileSimple]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserId] INT NOT NULL, 
    [Date] DATETIMEOFFSET NOT NULL, 
    [Title] NVARCHAR(MAX) NOT NULL, 
    [Description] NVARCHAR(MAX) NULL, 
    [Main] BIT NULL, 
    [Banned] BIT NULL
)

GO
