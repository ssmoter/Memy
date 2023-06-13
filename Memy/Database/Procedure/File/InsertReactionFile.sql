CREATE PROCEDURE [dbo].[InsertReactionFile]
@fileSimpleId int,
@value int,
@token NVARCHAR(MAX)

AS
DECLARE @exists int
DECLARE @userId int
BEGIN

EXEC dbo.SelectUserId @token, @userId OUTPUT


SET @exists = (SELECT COUNT(*) 
				FROM dbo.FilerReaction 
				WHERE FileSimpleId=@fileSimpleId 
				AND UserId=@userId )

IF(@exists>0)
	BEGIN
		UPDATE dbo.FilerReaction
		SET Value=@value
		WHERE FileSimpleId=@fileSimpleId 
		AND UserId = @userId
	END
ELSE
	BEGIN
		INSERT INTO dbo.FilerReaction
		(FileSimpleId,Value,UserId)
		VALUES(@fileSimpleId,@value,@userId)
	END


	--pobranie ilosci reakcji + i -
	SELECT SUM(FilerReaction.Value) AS 'ValueSumPositive'
	,(SELECT SUM(FilerReaction.Value)
		FROM FilerReaction
		WHERE FilerReaction.FileSimpleId=@fileSimpleId
		AND FilerReaction.Value < 0)  AS 'ValueSumNegative'
--pobranie czy dany użytkownik dodał reakcje i jaką
	,(SELECT TOP (1) Value 
		FROM FilerReaction 
		WHERE FilerReaction.FileSimpleId=@fileSimpleId
		AND FilerReaction.UserId=@userId) AS 'Value'
	FROM FilerReaction
	WHERE FilerReaction.FileSimpleId=@fileSimpleId
	AND FilerReaction.Value > 0
	FOR JSON PATH,WITHOUT_ARRAY_WRAPPER


END
