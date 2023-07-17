CREATE PROCEDURE dbo.UpdateUserReportedMessages
@token NVARCHAR(MAX),
@id INT,
@beenChecked BIT,
@beenDelete BIT

AS
DECLARE @userId INT
BEGIN

	EXEC dbo.SelectUserId @token,@userId OUTPUT

	UPDATE dbo.UserReportMessages
	SET UserReportMessages.BeenChecked=@beenChecked
	,UserReportMessages.BeenDelete = @beenDelete
	WHERE 
	UserReportMessages.UserId = @userId
	AND UserReportMessages.Id = @id

	EXEC dbo.GetUserReportedMessages @token

END