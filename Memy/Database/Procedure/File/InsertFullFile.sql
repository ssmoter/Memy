CREATE PROCEDURE [dbo].[InsertFullFile]
@json NVARCHAR(MAX),
@token NVARCHAR(MAX)
AS
BEGIN	
DECLARE @id int
DECLARE @tagList TABLE (Value NVARCHAR(100) NOT NULL PRIMARY KEY)
DECLARE @myTable TABLE (id INT)

--dodawanie nowego rekordu
	INSERT INTO [dbo].[FileSimple]
		(UserId,Date,Title,Description,Category,Banned)
		OUTPUT inserted.Id INTO @myTable
	VALUES (
		(SELECT UserId FROM [dbo].[UserToken] 
		 WHERE @token = Value)
		,GETDATE()
		,(SELECT JSON_VALUE(@json,'$.Title'))
		,(SELECT JSON_VALUE(@json,'$.Description'))
		,(SELECT JSON_VALUE(@json,'$.Categories'))
		,0)
-- pobranie jego id
	SELECT  @id = id FROM @myTable	


--dodanie nazwy wysłanych plików 
	INSERT INTO [dbo].[FileData]
		(FileSimpleId,ImgName,ImgType)
		(SELECT @id,Name,Typ 
		FROM OPENJSON(@json,'$.FileUploadStatuses') 
		WITH (Name NVARCHAR(MAX), Typ NVARCHAR(MAX)))

-- tabela tymczasowa z tagami
    INSERT INTO @tagList(Value)
    SELECT DISTINCT value
    FROM OPENJSON(@json, '$.Tag')
        WITH (value NVARCHAR(100) '$')

-- dodanie nowy tagów
    INSERT INTO [dbo].[FileTagList] (Value)
    SELECT Value
    FROM @tagList t
    WHERE NOT EXISTS (SELECT 1 FROM [dbo].[FileTagList] ftl WHERE ftl.Value = t.Value)
	
--lączenie tabel z tagami
	INSERT INTO [dbo].[FileTagConnected] (FileSimpleId, FileTagListId)
    SELECT @id, ftl.Id
    FROM @tagList t
    JOIN [dbo].[FileTagList] ftl ON t.Value = ftl.Value

	select @id
END