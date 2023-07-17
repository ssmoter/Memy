﻿CREATE PROCEDURE [dbo].[UpdateCategoryFileByAdmin]
@fileSimpleId INT,
@category NVARCHAR(MAX),
@token NVARCHAR(MAX),
@header NVARCHAR(MAX),
@body NVARCHAR(MAX),
@level INT

AS
DECLARE @adminId INT
DECLARE @userId INT
BEGIN

	EXEC dbo.GetAdminId @token, @adminId OUTPUT

	SET @userId = (SELECT FileSimple.UserId FROM FileSimple WHERE FileSimple.Id = @fileSimpleId)

	UPDATE dbo.FileSimple
	SET FileSimple.Category = @category
	WHERE FileSimple.Id = @fileSimpleId

	EXEC dbo.InsertUserReportMessages @token,@header,@body,@userId,@level,@fileSimpleId

	SELECT 1
END