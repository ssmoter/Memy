CREATE PROCEDURE LogoutUser
@Value UNIQUEIDENTIFIER

AS
BEGIN	
		Delete from dbo.UserToken where Value=@Value
	END
GO
