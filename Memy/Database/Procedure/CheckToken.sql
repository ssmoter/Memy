CREATE PROCEDURE CheckToken
@Value UNIQUEIDENTIFIER

AS
DECLARE @Result bit=0
DECLARE @UserId int=0

BEGIN	
		
		IF 0=(Select DoNotLogout from dbo.UserToken where Value = @Value) 
			Select @Result=1,@UserId=UserId from dbo.UserToken where Value=@Value and ExpiryDate > GETDATE()
		else
			Select @Result=1,@UserId=UserId from dbo.UserToken where Value=@Value 

		Select 
		 @Result [Result]
		--,@UserId [UserId]

	END