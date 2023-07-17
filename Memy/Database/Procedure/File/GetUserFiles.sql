CREATE PROCEDURE [dbo].[GetUserFiles]
@name NVARCHAR(MAX),
@start INT = 0,
@max INT = 10,
@orderTyp int = 0,
@banned bit = 0
AS
BEGIN

DECLARE @userId INT

SET @userId = (SELECT TOP(1) UserSimple.Id FROM UserSimple WHERE UserSimple.Nick = @name)


--pobieranie podstawowych parametrów rekordu
	SELECT 
	 FileModel.Id as'Id'
	,FileModel.Title as'Title' 
	,FileModel.Description as 'Description'
	,FileModel.Date as 'CreatedDate'
	,FileModel.Banned as 'Banned'


--pobranie danych użytkownika
	,(SELECT Nick AS 'Name' 
			,Avatar AS 'Avatar'
		FROM UserSimple 
		JOIN UserData on UserSimple.Id = UserData.UserId
		WHERE UserSimple.Id = FileModel.UserId
		FOR JSON PATH,WITHOUT_ARRAY_WRAPPER) AS 'User'

--pobranie listy zdjęć
	,(SELECT 
	FileData.ObjName as 'ObjName'
	,FileData.ObjType as'ObjTyp'
	,FileData.ObjOrder as 'ObjOrder'
	FROM [FileData] 
	WHERE [FileData].FileSimpleId=FileModel.Id
	FOR JSON PATH) AS 'FileModel'

--pobranie listy tag
	,(SELECT Value
	FROM [FileTagList] 
	LEFT JOIN [FileTagConnected] 
	ON FileTagList.Id= FileTagConnected.FileTagListId
	WHERE FileTagConnected.FileSimpleId=FileModel.Id 
	FOR JSON PATH) as 'Tag'

--pobranie ilosci reakcji + i -
	,(SELECT SUM(FilerReaction.Value) AS 'ValueSumPositive'
	,(SELECT SUM(FilerReaction.Value)
		FROM FilerReaction
		WHERE FilerReaction.FileSimpleId=FileModel.Id
		AND FilerReaction.Value < 0)  AS 'ValueSumNegative'
--pobranie czy dany użytkownik dodał reakcje i jaką
	,(SELECT TOP (1) Value 
		FROM FilerReaction 
		WHERE FilerReaction.FileSimpleId=FileModel.Id
		AND FilerReaction.UserId=@userId) AS 'Value'
	FROM FilerReaction
	WHERE FilerReaction.FileSimpleId=FileModel.Id
	AND FilerReaction.Value > 0
	FOR JSON PATH,WITHOUT_ARRAY_WRAPPER)AS 'Reaction'

	FROM [dbo].FileSimple AS FileModel
	WHERE FileModel.UserId = @userId AND (banned = 0 OR @banned = 1)


	ORDER BY
	CASE WHEN @orderTyp = 0 THEN FileModel.Date END DESC
	,CASE WHEN @orderTyp = 1 THEN FileModel.Date END ASC
	,CASE WHEN @orderTyp = 2 THEN (SELECT SUM(FilerReaction.Value) FROM FilerReaction WHERE FilerReaction.FileSimpleId=FileModel.Id) END DESC
	,CASE WHEN @orderTyp = 3 THEN (SELECT SUM(FilerReaction.Value) FROM FilerReaction WHERE FilerReaction.FileSimpleId=FileModel.Id) END ASC
	OFFSET @start*@max ROWS FETCH NEXT @max ROWS ONLY


END