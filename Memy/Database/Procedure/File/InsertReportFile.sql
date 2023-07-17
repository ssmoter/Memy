
CREATE PROCEDURE dbo.InsertReportFile
@fileSimpleId int,
@value int,
@token NVARCHAR(MAX)

AS
DECLARE @exists int
DECLARE @userId int
DECLARE @userFileTitle NVARCHAR(MAX)
DECLARE @sum int

BEGIN
	
--pobranie id user
	EXEC dbo.SelectUserId @token, @userId OUTPUT

--sprawdzenie czy user juz dodał zgłoszenie
	SET @exists = (SELECT COUNT(*) 
					FROM dbo.FileReported 
					WHERE FileSimpleId=@fileSimpleId 
					AND UserId=@userId )
--dodanie lub aktualizacja zgłoszenia
	IF(@exists>0)
		BEGIN
			UPDATE dbo.FileReported
			SET Value=@value
			WHERE FileSimpleId=@fileSimpleId 
			AND UserId = @userId
		END
	ELSE
		BEGIN
			INSERT INTO dbo.FileReported
			(FileSimpleId,Value,UserId)
			VALUES(@fileSimpleId,@value,@userId)
		END


--sprawdzenie automatycznego banowania
--jezeli jest za dożo zgłoszeń post jest banowany
	SET @sum = (SELECT SUM(Value) FROM dbo.FileReported WHERE FileReported.FileSimpleId=@fileSimpleId)

	IF @sum > 200
	BEGIN
		UPDATE dbo.FileSimple
		SET Banned=1
		WHERE FileSimple.Id = @fileSimpleId

	SELECT UserId=@userId,Title=@userFileTitle FROM FileSimple WHERE FileSimple.Id=@fileSimpleId


	INSERT INTO dbo.UserReportMessages
	(AdminId,UserId,Header,Body,Level)
	VALUES(-1,@userId,@userFileTitle,'Post został zbanowany zapoznaj się z regulaminem w celu jego przywrócenia',3)

	END

	Select @value


END