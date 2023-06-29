CREATE PROCEDURE [dbo].[GetFileByDate]
@start INT = 1,
@max INT = 10,
@category NVARCHAR(MAX) = 'main',
@banned bit = 0,
@dateEnd NVARCHAR(MAX) = 'empty',
@dateStart NVARCHAR(MAX) = 'today',
@token NVARCHAR(MAX) = '0'
AS
BEGIN	
DECLARE @dateEndAsDate datetimeoffset(7)
DECLARE @dateStartAsDate datetimeoffset(7)
DECLARE @tableID table(Id int)
DECLARE @type int
DECLARE @userId INT

EXEC dbo.SelectUserId @token,@userid OUTPUT

--ustawienie start jako id ostatniego rekordu
IF(@start <= 1)
BEGIN
	SET @start = (SELECT TOP(1) Id FROM FileSimple ORDER BY Id DESC)
END
ELSE
BEGIN
--ustawienie start jako id ostatniego rekordu odjąc zakres plus zasięg
	 INSERT INTO @tableID
	 SELECT TOP(@max * @start) Id FROM FileSimple ORDER BY Id DESC
	 SET @start = (SELECT TOP(1) * FROM @tableID ORDER BY 1)
END

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
	SELECT TOP(@max) 
	 FileModel.Id as'Id'
	,FileModel.Title as'Title' 
	,FileModel.Description as 'Description'
	,FileModel.Date as 'CreatedDate'

--pobranie danych użytkownika
	,(SELECT Nick AS 'Name' 
		FROM UserSimple 
		WHERE UserSimple.Id = FileModel.UserId
		FOR JSON PATH,WITHOUT_ARRAY_WRAPPER) AS 'User'

--pobranie listy zdjęć
	,(SELECT 
	FileData.ObjName as 'Name'
	,FileData.ObjType as'Typ'
	,FileData.ObjOrder as 'Order'
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

	WHERE FileModel.Id <= @start 
	AND FileModel.Category = @category 
	AND FileModel.Banned = @banned
	AND FileModel.Date BETWEEN @dateEndAsDate AND @dateStartAsDate
	ORDER BY FileModel.Date DESC
	--ORDER BY (SELECT SUM(FilerReaction.Value) FROM FilerReaction WHERE FilerReaction.FileSimpleId = FileModel.Id) DESC

	--FOR JSON 
	--PATH,ROOT('TaskModel')

	END