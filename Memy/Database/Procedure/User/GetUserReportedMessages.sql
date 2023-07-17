CREATE PROCEDURE dbo.GetUserReportedMessages
@token NVARCHAR(MAX)
AS
DECLARE @userId INT
BEGIN

	EXEC dbo.SelectUserId @token,@userId OUTPUT

	SELECT 
	UserReportMessages.Id				AS 'Id'
	,UserReportMessages.UserId			AS 'UserId'
	,UserReportMessages.Header			AS 'Header'
	,UserReportMessages.Body			AS 'Body'
	,UserReportMessages.Level			AS 'Level'
	,UserReportMessages.CreatedDate		AS 'CreatedDate'
	,UserReportMessages.BeenChecked		AS 'BeenChecked'
	,UserReportMessages.FileSimpleId	AS 'FileSimpleId'
	FROM dbo.UserReportMessages
	WHERE UserReportMessages.UserId=@userId
	AND UserReportMessages.BeenDelete = 0
	ORDER BY UserReportMessages.CreatedDate DESC

END