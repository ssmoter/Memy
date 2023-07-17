CREATE PROCEDURE [dbo].[GetFiles]
@start INT = 0,
@max INT = 10,
@category NVARCHAR(MAX) = 'main',
@banned bit = 0,
@dateEnd NVARCHAR(MAX) = 'empty',
@dateStart NVARCHAR(MAX) = 'today',
@orderTyp int = 0,
@token NVARCHAR(MAX) = '0'
AS
BEGIN	
DECLARE @dateEndAsDate datetimeoffset(7)
DECLARE @dateStartAsDate datetimeoffset(7)
DECLARE @userId INT
DECLARE @admin bit = 0


EXEC dbo.GetAdminId @token, @userId OUTPUT
IF @userId>0
	SET @admin=1
ELSE
	EXEC dbo.SelectUserId @token,@userid OUTPUT


--ustawienie zakresu dat
--defoult cała tabela
IF(@dateEnd = 'empty')
BEGIN
	SET @dateEndAsDate = TODATETIMEOFFSET('2000-05-21 13:25:26', '+02:00');
END
ELSE
BEGIN
	SET @dateEndAsDate = CONVERT(datetimeoffset, @dateEnd);
END

IF(@dateStart = 'today')
BEGIN
	SET @dateStartAsDate = GETDATE();
END
ELSE
BEGIN
	SET @dateStartAsDate = CONVERT(datetimeoffset, @dateStart);
END

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

	
--pobranie ilości reportów i czy user zgłosił
,(SELECT SUM(FileReported.Value) as'ValueSum'
	,(SELECT FileReported.Value 
	FROM FileReported
	WHERE FileReported.FileSimpleId=FileModel.Id
		AND FileReported.UserId=@userId) AS 'Value'
	FROM FileReported
	WHERE FileReported.FileSimpleId=FileModel.Id
	AND @admin=1
	FOR JSON PATH,WITHOUT_ARRAY_WRAPPER) AS 'Reported'


	FROM [dbo].FileSimple AS FileModel

	WHERE FileModel.Category = @category 
	AND FileModel.Banned = @banned
	AND FileModel.Date BETWEEN @dateEndAsDate AND @dateStartAsDate

	ORDER BY
	CASE WHEN @orderTyp = 0 THEN FileModel.Date END DESC
	,CASE WHEN @orderTyp = 1 THEN FileModel.Date END ASC
	,CASE WHEN @orderTyp = 2 THEN (SELECT SUM(FilerReaction.Value) FROM FilerReaction WHERE FilerReaction.FileSimpleId=FileModel.Id) END DESC
	,CASE WHEN @orderTyp = 3 THEN (SELECT SUM(FilerReaction.Value) FROM FilerReaction WHERE FilerReaction.FileSimpleId=FileModel.Id) END ASC
	OFFSET @start*@max ROWS FETCH NEXT @max ROWS ONLY

	END