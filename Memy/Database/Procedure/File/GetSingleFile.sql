CREATE PROCEDURE [dbo].[GetSingleFile]
@id INT,
@token NVARCHAR(MAX)=''

AS
DECLARE @userId INT
BEGIN

EXEC dbo.SelectUserId @token,@userid OUTPUT

SELECT TOP (1) 
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
	WHERE FileModel.Id = @id

END