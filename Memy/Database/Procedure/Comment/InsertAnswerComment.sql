CREATE PROCEDURE dbo.InsertAnswerComment
@token NVARCHAR(MAX),
@json NVARCHAR(MAX),
@OrderTyp int = 0


AS
	DECLARE @userId int
	DECLARE @objectId int
	DECLARE @commentId int
	DECLARE @InsertedRows TABLE (id INT)
BEGIN

	EXEC [dbo].[SelectUserId] @token,@userId OUTPUT
	SET @objectId = (SELECT JSON_VALUE(@json,'$.ObjectId'))


	INSERT INTO [dbo].AnswerComment
		(CommentId,Date,Description,UserId)
	OUTPUT INSERTED.id INTO @InsertedRows
		VALUES(
		(SELECT JSON_VALUE(@json,'$.ObjectId'))
		,GETDATE()
		,(SELECT JSON_VALUE(@json,'$.Description'))
		,@userId)

	SET @commentId = (SELECT TOP(1) id FROM @InsertedRows)

	EXEC  dbo.InsertReactionAnswerComment @commentId ,0 ,@token,0

	EXEC [dbo].GetAnswerComment @objectId, @OrderTyp, @token
END