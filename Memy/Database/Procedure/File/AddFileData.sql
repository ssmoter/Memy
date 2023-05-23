CREATE PROCEDURE [dbo].[AddFileData]
@FileSimpleId int,
@ImgName NVARCHAR(MAX),
@ImgType NVARCHAR(MAX)

AS
BEGIN	

	INSERT INTO [dbo].[FileData] 	
	(FileSimpleId,ImgName,ImgType)	
	VALUES (@FileSimpleId,@ImgName,@ImgType)

	select 1
	END