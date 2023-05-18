CREATE PROCEDURE [dbo].[RegisterUser]
@Email NVARCHAR(MAX),
@Nick NVARCHAR(MAX),
@Password NVARCHAR(MAX),
@Role NVARCHAR(MAX)='user'

AS
BEGIN
 
Insert Into dbo.UserSimple (Email,Nick,Password,Role) 
Values (@Email,@Nick,HASHBYTES('SHA2_512',@Password),@Role)

	END