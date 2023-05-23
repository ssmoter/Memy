CREATE PROCEDURE [dbo].[AddTag]
@FileSimpleId int,
@Value NVARCHAR(MAX)

AS
DECLARE @id int
DECLARE @output_table TABLE (id INT)
BEGIN	

	SET @id = (SELECT Id FROM [dbo].[FileTagList] WHERE Value = @Value)
	IF @id IS NULL
		BEGIN	
			INSERT INTO [dbo].[FileTagList]
			(Value)
			OUTPUT inserted.Id INTO @output_table
			VALUES (@Value)
			select @id = id FROM @output_table
		END
			Insert INTO [dbo].[FileTagConnected] 
			(FileSimpleId,FileTagListId)
			VALUES(@FileSimpleId,@id)
	select 1
	END


