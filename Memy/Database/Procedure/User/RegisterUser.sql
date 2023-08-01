CREATE PROCEDURE [dbo].[RegisterUser]
@Email NVARCHAR(MAX),
@Nick NVARCHAR(MAX),
@Password NVARCHAR(MAX),
@Role NVARCHAR(MAX)='user'

AS
DECLARE @userId INT
DECLARE @tableId TABLE (id INT)
DECLARE @guid UNIQUEIDENTIFIER 
BEGIN

	Insert Into dbo.UserSimple (Email,Nick,Password,Role) 
	OUTPUT  inserted.Id INTO @tableId 
	Values (@Email,@Nick,HASHBYTES('SHA2_512',@Password),@Role)
	
	SET @userId = (SELECT TOP(1) id FROM @tableId)
	
	INSERT INTO dbo.UserData (CreatedDate,EmailConfirm,UserId)
	VALUES (GETDATE(),0,@userId)
	
	SET @guid = NEWID()

	INSERT INTO dbo.UserToken (UserId,Value,ExpiryDate,DoNotLogout)
	VALUES(@userId,@guid,DATEADD(hour, 1, GETDATE()),0)

	SELECT @guid 

	END