CREATE PROCEDURE dbo.GetAnswerComment
@id int,
@OrderTyp int = 0,
@token NVARCHAR(MAX) = '0'
AS
DECLARE @userId int

BEGIN

EXEC [dbo].[SelectUserId] @token,@userId OUTPUT

	SELECT 
	 AnswerComment.Id
	,AnswerComment.CommentId
	,AnswerComment.Date
	,AnswerComment.Description

	--pobranie danych użytkownika
	,(SELECT Nick AS 'Name' 
		FROM UserSimple 
		WHERE UserSimple.Id = AnswerComment.UserId
		FOR JSON PATH,WITHOUT_ARRAY_WRAPPER) AS 'User'


		--pobranie ilosci reakcji + i -
	,(SELECT SUM(AnswerCommentReaction.Value) AS 'ValueSumPositive'
	,(SELECT SUM(AnswerCommentReaction.Value)
		FROM AnswerCommentReaction
		WHERE AnswerCommentReaction.CommantId=AnswerComment.Id
		AND AnswerCommentReaction.Value < 0)  AS 'ValueSumNegative'
--pobranie czy dany użytkownik dodał reakcje i jaką
	,(SELECT TOP (1) Value 
		FROM AnswerCommentReaction 
		WHERE AnswerCommentReaction.CommantId=AnswerComment.Id
		AND AnswerCommentReaction.UserId=@userId) AS 'Value'
	FROM AnswerCommentReaction
	WHERE AnswerCommentReaction.CommantId=AnswerComment.Id
	AND AnswerCommentReaction.Value > 0
	FOR JSON PATH,WITHOUT_ARRAY_WRAPPER)AS 'Reaction'

	FROM [dbo].AnswerComment 
	WHERE AnswerComment.CommentId=@id


END