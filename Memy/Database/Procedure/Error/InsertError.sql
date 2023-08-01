CREATE PROCEDURE [dbo].[InsertError]
@track NVARCHAR(MAX),
@message NVARCHAR(MAX) 


AS
BEGIN

INSERT INTO dbo.ErrorTable
	(ErrorTable.Date,ErrorTable.Track,ErrorTable.Message)
	VALUES(GETDATE(),@track,@message)

	SELECT 1
END