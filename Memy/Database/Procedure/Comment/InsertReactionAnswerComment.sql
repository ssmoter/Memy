CREATE PROCEDURE dbo.InsertReactionAnswerComment
@answerCommentId int,
@value int,
@token NVARCHAR(MAX),
@get bit = 1

AS
DECLARE @exists int
DECLARE @userId int
BEGIN
EXEC dbo.SelectUserId @token, @userId OUTPUT

SET @exists = (SELECT COUNT(*) 
				FROM dbo.AnswerCommentReaction 
				WHERE CommantId=@answerCommentId 
				AND UserId=@userId )

IF(@exists>0)
	BEGIN
		UPDATE dbo.AnswerCommentReaction
		SET Value=@value
		WHERE CommantId=@answerCommentId 
		AND UserId = @userId
	END
ELSE
	BEGIN
		INSERT INTO dbo.AnswerCommentReaction
		(CommantId,Value,UserId)
		VALUES(@answerCommentId,@value,@userId)
	END

	IF(@get = 1)
		BEGIN
		--pobranie ilosci reakcji + i -
	 SELECT SUM(AnswerCommentReaction.Value) AS 'ValueSumPositive'
	,(SELECT SUM(AnswerCommentReaction.Value)
		FROM AnswerCommentReaction
		WHERE AnswerCommentReaction.CommantId=@answerCommentId
		AND AnswerCommentReaction.Value < 0)  AS 'ValueSumNegative'
--pobranie czy dany użytkownik dodał reakcje i jaką
	,(SELECT TOP (1) Value 
		FROM AnswerCommentReaction 
		WHERE AnswerCommentReaction.CommantId=@answerCommentId
		AND AnswerCommentReaction.UserId=@userId) AS 'Value'
	FROM AnswerCommentReaction
	WHERE AnswerCommentReaction.CommantId=@answerCommentId
	AND AnswerCommentReaction.Value > 0
	FOR JSON PATH,WITHOUT_ARRAY_WRAPPER
	END

END