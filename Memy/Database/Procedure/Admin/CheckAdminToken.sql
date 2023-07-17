CREATE PROCEDURE [dbo].[CheckAdminToken]
@Value UNIQUEIDENTIFIER

AS
DECLARE @Result bit=0
DECLARE @UserId int=0

BEGIN	
		
		IF 0=(Select DoNotLogout from dbo.UserToken where Value = @Value) 
			Select @Result=1,@UserId=UserId 
			from dbo.UserToken 
			LEFT JOIN UserSimple on UserToken.UserId = UserSimple.Id
			where Value=@Value 
			AND ExpiryDate > GETDATE()
			AND UserSimple.Role = 'Admin'
		else
			Select @Result=1,@UserId=UserId from dbo.UserToken 
			LEFT JOIN UserSimple on UserToken.UserId = UserSimple.Id
			where Value=@Value 
			AND UserSimple.Role = 'Admin'

		Select 
		 @Result [Result]

	END