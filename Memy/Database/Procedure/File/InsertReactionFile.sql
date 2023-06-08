CREATE PROCEDURE [dbo].[InsertReactionFile]
@fileSimpleId int,
@value int,
@token NVARCHAR(MAX)

AS
DECLARE @exists int
DECLARE @userId int
BEGIN

SET @userId = (SELECT UserId
				FROM dbo.UserToken
				WHERE Value = TRY_CAST (@token as UNIQUEIDENTIFIER))

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

	--pobranie danych o reakcjach
	SELECT SUM(FilerReaction.Value) AS 'ValueSum'
	--pobranie czy dany użytkownik dodał reakcje i jaką
	,(SELECT TOP (1) Value
		FROM FilerReaction 
		WHERE FilerReaction.FileSimpleId=@fileSimpleId
		AND FilerReaction.UserId=@userId) AS 'Value'
	FROM [FilerReaction]
 	WHERE FilerReaction.FileSimpleId=@fileSimpleId
	FOR JSON PATH,without_array_wrapper
END
