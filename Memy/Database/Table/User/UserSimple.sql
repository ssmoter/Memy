﻿CREATE TABLE [dbo].[UserSimple]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Email] NVARCHAR(MAX) NOT NULL, 
    [Nick] NVARCHAR(MAX) NOT NULL, 
    [Password] BINARY(64) NOT NULL, 
    [Role] NVARCHAR(50) NULL
)
