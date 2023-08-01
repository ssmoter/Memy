CREATE PROCEDURE [dbo].[RegisterUserConfirm]
@value NVARCHAR(MAX)
AS
DECLARE @userId INT
BEGIN
	
	SET @userId = (SELECT UserId FROM dbo.UserToken WHERE UserToken.Value = @value)

	IF (@userId > 0)
	BEGIN
		UPDATE dbo.UserData
		SET dbo.UserData.EmailConfirm = 1
		WHERE dbo.UserData.UserId = @userId

		SELECT TOP(1)
		UserSimple.Id[Id],UserSimple.Nick[Nick],UserSimple.Role[Role],
		Token.Value[Value],Token.ExpiryDate[ExpiryDate],Token.DoNotLogout[DoNotLogOut]
		from dbo.UserSimple as UserSimple
		INNER JOIN dbo.UserToken as Token
		on UserSimple.Id=Token.UserId 
		WHERE Token.Value = @value
	END
	ELSE
		SELECT 0
END