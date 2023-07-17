CREATE TABLE [dbo].[UserReportMessages]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[AdminId] INT NOT NULL,
	[UserId] INT NOT NULL, 
    [Header] NVARCHAR(MAX) NULL, 
    [Body] NVARCHAR(MAX) NULL, 
    [Level] INT NULL,
    [CreatedDate] DATETIMEOFFSET NOT NULL DEFAULT GETDATE(), 
    [BeenChecked] BIT NOT NULL DEFAULT 0, 
    [BeenDelete] BIT NOT NULL DEFAULT 0, 
    [FileSimpleId] INT NOT NULL, 
)
