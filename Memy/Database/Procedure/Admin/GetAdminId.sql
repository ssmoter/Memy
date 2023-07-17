CREATE PROCEDURE dbo.GetAdminId 
@token NVARCHAR(MAX),
@AdminId INT OUTPUT

AS
DECLARE @userId INT
BEGIN

EXEC dbo.SelectUserId @token, @userId OUTPUT

SET @AdminId = ( SELECT TOP(1) Id 
					FROM UserSimple 
					WHERE UserSimple.Id=@userId 
					AND UserSimple.Role = 'Admin' ) 
END