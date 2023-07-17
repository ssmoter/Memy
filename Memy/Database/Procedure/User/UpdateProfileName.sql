CREATE PROCEDURE [dbo].[UpdateProfileName]
@name NVARCHAR(MAX),
@token NVARCHAR(MAX)
AS
DECLARE @userId INT
BEGIN

EXEC dbo.SelectUserId @token,@userId OUTPUT

Update dbo.UserSimple 
SET 
	Nick = @name
WHERE
	 UserSimple.Id = @userId

	 SELECT 1
END