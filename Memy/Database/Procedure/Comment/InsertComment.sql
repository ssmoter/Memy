CREATE PROCEDURE [dbo].[InsertComment]
@token NVARCHAR(MAX),
@json NVARCHAR(MAX),
@OrderTyp int = 0

AS
DECLARE @userId int
DECLARE @fileSimpleId int
DECLARE @commentId int
DECLARE @InsertedRows TABLE (id INT)
DECLARE @jj TABLE(json NVARCHAR(MAX))
BEGIN

EXEC [dbo].[SelectUserId] @token,@userId OUTPUT
SET @fileSimpleId = (SELECT JSON_VALUE(@json,'$.ObjectId'))


INSERT INTO [dbo].[Comment]
	(FileSimpleId,Date,Description,UserId)
OUTPUT INSERTED.id INTO @InsertedRows
	VALUES(
	(SELECT JSON_VALUE(@json,'$.ObjectId'))
	,GETDATE()
	,(SELECT JSON_VALUE(@json,'$.Description'))
	,@userId)

SET @commentId = (SELECT TOP(1) id FROM @InsertedRows)

EXEC  dbo.InsertReactionComment @commentId ,0 ,@token,0

EXEC [dbo].[GetComment] @fileSimpleId,@OrderTyp,@token

END