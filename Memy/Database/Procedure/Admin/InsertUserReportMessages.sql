CREATE PROCEDURE dbo.InsertUserReportMessages
@token NVARCHAR(MAX),
@header NVARCHAR(MAX) , 
@body NVARCHAR(MAX) ,
@UserId INT,
@level INT,
@fileSimpleId INT

AS
DECLARE @adminId int

BEGIN

--pobranie id admina
	EXEC dbo.GetAdminId @token, @adminId OUTPUT
--dodanie wiadomości uzytkowniak jako admin
	INSERT INTO dbo.UserReportMessages
	(AdminId,UserId,Header,Body,Level,FileSimpleId)
	VALUES(@adminId,@UserId,@header,@body,@level,@fileSimpleId)

END