CREATE PROCEDURE LoginUser
@Email NVARCHAR(MAX),
@Password NVARCHAR(MAX),
@DoNotLogOut BIT = 0

AS
BEGIN
DECLARE @Id INT = 0
DECLARE @TValue UNIQUEIDENTIFIER

	DELETE FROM dbo.UserToken WHERE ExpiryDate > GETDATE()-5


	Select @Id=UserSimple.Id,@TValue=NEWID() 
	From dbo.UserSimple 
	JOIN dbo.UserData ON dbo.UserSimple.Id = dbo.UserData.UserId
	Where Email = @Email 
	AND Password = HASHBYTES('SHA2_512',@Password)

	IF(@Id > 0)
	
		INSERT INTO dbo.UserToken (UserId,Value,ExpiryDate,DoNotLogout)
		VALUES(@Id,@TValue,GETDATE()+1,@DoNotLogOut)

	IF (@Id > 0)
		SELECT TOP(1)
		UserSimple.Id[Id]
		,UserSimple.Nick[Nick]
		,UserSimple.Role[Role]
		,Token.Value[Value]
		,Token.ExpiryDate[ExpiryDate]
		,Token.DoNotLogout[DoNotLogOut]
		,UserData.EmailConfirm [EmailConfirm]

		from dbo.UserSimple as UserSimple
		INNER JOIN dbo.UserToken as Token
		ON UserSimple.Id = Token.UserId 
		LEFT JOIN dbo.UserData 
		ON UserSimple.Id = UserData.UserId
		WHERE Token.Value=@TValue
	ELSE
	SELECT 0[Id]

	END