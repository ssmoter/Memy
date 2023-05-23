CREATE PROCEDURE [dbo].[GetFileByDate]
@start INT = 0,
@max INT = 10,
@category NVARCHAR(MAX) = 'main',
@banned bit = 0,
@dateEnd NVARCHAR(MAX) = 'empty',
@dateStart NVARCHAR(MAX) = 'today'
AS
BEGIN	
DECLARE @dateEndAsDate datetimeoffset(7)
DECLARE @dateStartAsDate datetimeoffset(7)
IF(@start <= 0)
BEGIN
	SET @start = (SELECT COUNT(*) FROM FileSimple)
END
ELSE
BEGIN
	SET @start = (SELECT COUNT(*) FROM FileSimple) - (@max * @start)
END


IF(@dateEnd ='empty')
BEGIN
	SET @dateEndAsDate = TODATETIMEOFFSET('2000-05-21 13:25:26', '+02:00');
END
ELSE
BEGIN
	SET @dateEndAsDate = CONVERT(datetimeoffset, @dateEnd);
END

IF(@dateStart ='today')
BEGIN
	SET @dateStartAsDate = GETDATE();
END
ELSE
BEGIN
	SET @dateStartAsDate = CONVERT(datetimeoffset, @dateStart);
END


	SELECT TOP(@max) 
	 FileModel.Id as'Id'
	,FileModel.Title as'Title' 
	,FileModel.Description as 'Description'
	,FileModel.Date as 'CreatedDate'

	,UserSimple.Nick AS 'User.Name'

	,(SELECT FileData.ImgName as 'Name',FileData.ImgType as'Typ'
	FROM [FileData] 
	WHERE [FileData].FileSimpleId=FileModel.Id
	FOR JSON PATH) AS 'FileModel'

	,(SELECT Value
	FROM [FileTagList] 
	LEFT JOIN [FileTagConnected] 
	ON FileTagList.Id= FileTagConnected.FileTagListId
	WHERE FileTagConnected.FileSimpleId=FileModel.Id 
	FOR JSON PATH) as 'Tag'

	FROM [dbo].FileSimple AS FileModel

	LEFT JOIN UserSimple ON FileModel.UserId = UserSimple.Id

	WHERE FileModel.Id >= @start 
	AND FileModel.Category = @category 
	AND FileModel.Banned = @banned
	AND FileModel.Date BETWEEN @dateEndAsDate AND @dateStartAsDate
	ORDER BY FileModel.Date DESC

	FOR JSON 
	--AUTO
	--,ROOT('')
	PATH,ROOT('TaskModel')

	END
	