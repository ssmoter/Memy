CREATE PROCEDURE [dbo].[GetEmail]
@token NVARCHAR(MAX)
AS
DECLARE @userId INT

BEGIN

EXEC dbo.SelectUserId @token, @userid OUTPUT

SELECT 
	UserSimple.Email as 'Email'

FROM dbo.UserSimple 
WHERE UserSimple.Id = @userId

END