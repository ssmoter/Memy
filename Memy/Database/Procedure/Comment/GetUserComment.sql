﻿CREATE PROCEDURE [dbo].[GetUserComment]
@OrderTyp int = 0,
@name NVARCHAR(MAX) = '0'

AS
DECLARE @userId int

BEGIN

SET @userId = (SELECT TOP(1) Id FROM UserSimple WHERE UserSimple.Nick = @name)

	SELECT 
	 Comment.Id
	,Comment.FileSimpleId
	,Comment.Date
	,Comment.Description

--pobranie danych użytkownika
	,(SELECT Nick AS 'Name' 
			,Avatar AS 'Avatar'
		FROM UserSimple 
		JOIN UserData on UserSimple.Id = UserData.UserId
		WHERE UserSimple.Id = Comment.UserId
		FOR JSON PATH,WITHOUT_ARRAY_WRAPPER) AS 'User'


		--pobranie ilosci reakcji + i -
	,(SELECT SUM(CommentReaction.Value) AS 'ValueSumPositive'
	,(SELECT SUM(CommentReaction.Value)
		FROM CommentReaction
		WHERE CommentReaction.CommantId=Comment.Id
		AND CommentReaction.Value < 0)  AS 'ValueSumNegative'
--pobranie czy dany użytkownik dodał reakcje i jaką
	,(SELECT TOP (1) Value 
		FROM CommentReaction 
		WHERE CommentReaction.CommantId=Comment.Id
		AND CommentReaction.UserId=@userId) AS 'Value'
	FROM CommentReaction
	WHERE CommentReaction.CommantId=Comment.Id
	AND CommentReaction.Value > 0
	FOR JSON PATH,WITHOUT_ARRAY_WRAPPER)AS 'Reaction'


	FROM [dbo].[Comment] 
	WHERE Comment.UserId = @userId
	ORDER BY
	CASE WHEN @OrderTyp = 0 THEN Comment.Date END DESC
	,CASE WHEN @OrderTyp = 1 THEN Comment.Date END ASC
	,CASE WHEN @OrderTyp = 2 THEN (SELECT SUM(CommentReaction.Value) FROM CommentReaction WHERE CommentReaction.CommantId=Comment.Id) END DESC
	,CASE WHEN @OrderTyp = 3 THEN (SELECT SUM(CommentReaction.Value) FROM CommentReaction WHERE CommentReaction.CommantId=Comment.Id) END ASC
			 

END
