CREATE PROCEDURE [dbo].[UpdateProfileAvatar]
@avatar NVARCHAR(MAX),
@token NVARCHAR(MAX)
AS
DECLARE @userId INT
BEGIN

EXEC dbo.SelectUserId @token,@userId OUTPUT

Update dbo.UserData 
SET 
	 Avatar = @avatar
WHERE
	 UserData.UserId = @userId

	 SELECT 1
END