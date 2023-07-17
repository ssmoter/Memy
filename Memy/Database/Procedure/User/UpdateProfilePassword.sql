CREATE PROCEDURE [dbo].[UpdateProfilePassword]
@oldPassword NVARCHAR(MAX),
@newPassword NVARCHAR(MAX),
@token NVARCHAR(MAX)
AS
DECLARE @userId INT
BEGIN

EXEC dbo.SelectUserId @token,@userId OUTPUT

Update dbo.UserSimple 
SET 
	Password=HASHBYTES('SHA2_512',@newPassword)
WHERE
	UserSimple.Password = HASHBYTES('SHA2_512',@oldPassword)
	AND UserSimple.Id = @userId

	SELECT 1
END