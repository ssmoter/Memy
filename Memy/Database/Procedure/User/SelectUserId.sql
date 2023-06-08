CREATE PROCEDURE [dbo].[SelectUserId]
@token NVARCHAR(MAX),
@userId INT OUTPUT
AS
BEGIN

SET @userId = (SELECT TOP(1) UserId
FROM dbo.UserToken
WHERE Value = TRY_CAST (@token as UNIQUEIDENTIFIER))

END