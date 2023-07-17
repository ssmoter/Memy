CREATE PROCEDURE [dbo].[DeleteFileAsAdmin]
@fileSimpleId INT,
@token NVARCHAR(MAX),
@header NVARCHAR(MAX),
@body NVARCHAR(MAX),
@level INT

AS
DECLARE @adminId INT
DECLARE @userId INT
DECLARE @commentReactionTable TABLE (id int)
DECLARE @answerCommentReactionTable TABLE (id int)
DECLARE @fileDataDelete TABLE (ObjName NVARCHAR(MAX))
BEGIN	


EXEC dbo.GetAdminId @token, @adminId OUTPUT

SET @userId = (SELECT FileSimple.UserId FROM FileSimple WHERE FileSimple.Id = @fileSimpleId)

	IF @adminId > 0
		BEGIN

		INSERT INTO @commentReactionTable 
		SELECT Id FROM dbo.Comment WHERE Comment.FileSimpleId = @fileSimpleId

		INSERT INTO @answerCommentReactionTable 
		SELECT Id FROM dbo.AnswerComment WHERE AnswerComment.CommentId IN (SELECT id FROM @commentReactionTable)

		INSERT INTO @fileDataDelete 
		SELECT ObjName FROM dbo.FileData WHERE FileData.FileSimpleId=@fileSimpleId


			DELETE FROM [dbo].AnswerCommentReaction WHERE AnswerCommentReaction.CommantId IN (SELECT id FROM @answerCommentReactionTable)
			DELETE FROM [dbo].AnswerComment WHERE AnswerComment.CommentId IN (SELECT id FROM @commentReactionTable)

			DELETE FROM [dbo].CommentReaction WHERE CommentReaction.CommantId IN (SELECT id FROM @commentReactionTable)
			DELETE FROM [dbo].Comment WHERE FileSimpleId=@fileSimpleId
			
			DELETE FROM [dbo].FilerReaction WHERE FileSimpleId=@fileSimpleId
			DELETE FROM [dbo].[FileTagConnected] WHERE FileSimpleId=@fileSimpleId

			DELETE FROM [dbo].[FileData] WHERE FileSimpleId=@fileSimpleId
			DELETE FROM [dbo].[FileSimple] WHERE Id=@fileSimpleId

			EXEC dbo.InsertUserReportMessages @token,@header,@body,@userId,@level,@fileSimpleId

			SELECT * FROM @fileDataDelete
		END
		
END