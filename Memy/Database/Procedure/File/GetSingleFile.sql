CREATE PROCEDURE [dbo].[GetSingleFile]
@id INT,
@token NVARCHAR(MAX)=''

AS
DECLARE @userId INT
DECLARE @admin bit = 0

BEGIN

EXEC dbo.GetAdminId @token, @userId OUTPUT
IF @userId>0
	SET @admin=1
ELSE
	EXEC dbo.SelectUserId @token,@userid OUTPUT


SELECT TOP (1) 
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
	WHERE FileReported.FileSimpleId=@id
		AND FileReported.UserId=@userId) AS 'Value'
	FROM FileReported
	WHERE FileReported.FileSimpleId=@id
	AND @admin=1
	FOR JSON PATH,WITHOUT_ARRAY_WRAPPER) AS 'Reported'



	FROM [dbo].FileSimple AS FileModel
	WHERE FileModel.Id = @id

END