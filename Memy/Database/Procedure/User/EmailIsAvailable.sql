CREATE PROCEDURE [dbo].[EmailIsAvailable]
@value NVARCHAR(MAX)
AS

BEGIN

	IF ((SELECT COUNT(*) FROM UserSimple WHERE UserSimple.Email = @value )>0)
	BEGIN
		SELECT 0 as 'Available'
	END
	ELSE
	BEGIN
		SELECT 1 as 'Available'
	END

END