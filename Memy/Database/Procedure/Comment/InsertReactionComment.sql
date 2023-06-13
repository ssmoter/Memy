CREATE PROCEDURE [dbo].[InsertReactionComment]
@CommentId int,
@value int,
@token NVARCHAR(MAX),
@get bit = 1
AS
DECLARE @exists int
DECLARE @userId int
BEGIN

EXEC dbo.SelectUserId @token, @userId OUTPUT


SET @exists = (SELECT COUNT(*) 
				FROM dbo.CommentReaction 
				WHERE CommantId=@CommentId 
				AND UserId=@userId )

IF(@exists>0)
	BEGIN
		UPDATE dbo.CommentReaction
		SET Value=@value
		WHERE CommantId=@CommentId 
		AND UserId = @userId
	END
ELSE
	BEGIN
		INSERT INTO dbo.CommentReaction
		(CommantId,Value,UserId)
		VALUES(@CommentId,@value,@userId)
	END

	IF(@get = 1)
		BEGIN
		--pobranie ilosci reakcji + i -
	 SELECT SUM(CommentReaction.Value) AS 'ValueSumPositive'
	,(SELECT SUM(CommentReaction.Value)
		FROM CommentReaction
		WHERE CommentReaction.CommantId=@CommentId
		AND CommentReaction.Value < 0)  AS 'ValueSumNegative'
--pobranie czy dany użytkownik dodał reakcje i jaką
	,(SELECT TOP (1) Value 
		FROM CommentReaction 
		WHERE CommentReaction.CommantId=@CommentId
		AND CommentReaction.UserId=@userId) AS 'Value'
	FROM CommentReaction
	WHERE CommentReaction.CommantId=@CommentId
	AND CommentReaction.Value > 0
	FOR JSON PATH,WITHOUT_ARRAY_WRAPPER
	END
END