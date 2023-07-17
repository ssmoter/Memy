CREATE TABLE [dbo].[UserData]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserId] INT NOT NULL, 
    [CreatedDate] DATETIMEOFFSET NOT NULL, 
    [Avatar] NVARCHAR(MAX) NULL, 
    [EmailConfirm] BIT NOT NULL DEFAULT 0 

)
