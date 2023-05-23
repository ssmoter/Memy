CREATE TABLE [dbo].[FileSimple]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserId] INT NOT NULL, 
    [Date] DATETIMEOFFSET NOT NULL, 
    [Title] NVARCHAR(200) NOT NULL, 
    [Description] NVARCHAR(MAX) NULL, 
    [Category] NVARCHAR(200) NULL, 
    [Banned] BIT NULL
)

GO
