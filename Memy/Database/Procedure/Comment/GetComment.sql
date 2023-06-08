CREATE PROCEDURE [dbo].[GetComment]
@id int,
@OrderTyp int = 0,
@token NVARCHAR(MAX) = '0'

AS
BEGIN

	SELECT 
	 Comment.Id
	,Comment.FileSimpleId
	,Comment.Date
	,Comment.Description
	,UserSimple.Nick  as 'User.Name'

	--pobranie ilosci reakcji
		,(SELECT SUM(CommentReaction.Value) AS 'ValueSum'
		--pobranie czy dany użytkownik dodał reakcje i jaką
		,(SELECT TOP (1) Value
			FROM CommentReaction 
			WHERE CommentReaction.CommantId=Comment.Id
			AND CommentReaction.UserId=
					(SELECT UserId
					FROM [dbo].[UserToken] 
					WHERE Value = TRY_CAST (@token as UNIQUEIDENTIFIER))) AS 'Value'
		FROM CommentReaction
 		WHERE CommentReaction.CommantId = Comment.Id
		FOR JSON PATH) AS 'Reaction'

	FROM [dbo].[Comment] 
	LEFT JOIN [dbo].[UserSimple] AS UserSimple ON UserSimple.Id= Comment.UserId 
	WHERE FileSimpleId=@id
	ORDER BY
	CASE WHEN @OrderTyp = 0 THEN Comment.Date END DESC
	,CASE WHEN @OrderTyp = 1 THEN Comment.Date END ASC
	,CASE WHEN @OrderTyp = 2 THEN (SELECT SUM(CommentReaction.Value) FROM CommentReaction WHERE CommentReaction.CommantId=Comment.Id) END DESC
	,CASE WHEN @OrderTyp = 3 THEN (SELECT SUM(CommentReaction.Value) FROM CommentReaction WHERE CommentReaction.CommantId=Comment.Id) END ASC
			 
	FOR JSON PATH,ROOT('')

END