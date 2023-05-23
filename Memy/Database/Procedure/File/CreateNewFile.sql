CREATE PROCEDURE [dbo].[CreateNewFile]
@token NVARCHAR(MAX),
@Title NVARCHAR(MAX),
@Description NVARCHAR(MAX) = null,
@Category NVARCHAR(MAX) = null


AS
DECLARE @UserId int
BEGIN	

	SELECT @UserId=UserId FROM [dbo].[UserToken] 
	WHERE @token = Value

	INSERT INTO [dbo].[FileSimple] 	
	(UserId,Date,Title,Description,Category,Banned)	
	OUTPUT inserted.Id
	VALUES (@UserId,GETDATE(),@Title,@Description,@Category,0)


	END