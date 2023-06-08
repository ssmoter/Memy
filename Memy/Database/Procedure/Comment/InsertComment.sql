CREATE PROCEDURE [dbo].[InsertComment]
@token NVARCHAR(MAX),
@json NVARCHAR(MAX),
@OrderTyp int = 0


AS
DECLARE @userId int
DECLARE @fileSimpleId int
BEGIN

EXEC [dbo].[SelectUserId] @token,@userId OUTPUT
SET @fileSimpleId = (SELECT JSON_VALUE(@json,'$.FileSimpleId'))


INSERT INTO [dbo].[Comment]
(FileSimpleId,Date,Description,UserId)
VALUES(
(SELECT JSON_VALUE(@json,'$.FileSimpleId'))
,GETDATE()
,(SELECT JSON_VALUE(@json,'$.Description'))
,@userId)

EXEC [dbo].[GetComment] @fileSimpleId,@OrderTyp,@token

END