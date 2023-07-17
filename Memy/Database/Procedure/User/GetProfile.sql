CREATE PROCEDURE [dbo].[GetProfile]
@name NVARCHAR(MAX)
AS
DECLARE @userId INT

BEGIN

SET @userId = (SELECT TOP(1) Id
FROM dbo.UserSimple
WHERE Nick = @name)

SELECT 
UserData.Avatar as 'Avatar'

,UserData.CreatedDate as 'CreatedDate'

,(SELECT SUM(FilerReaction.Value)
	FROM dbo.FileSimple
	LEFT JOIN dbo.FilerReaction ON FileSimple.Id = FilerReaction.FileSimpleId 
	WHERE FileSimple.UserId = @userId and FilerReaction.Value > 0) as 'SumTaskLike'

,(SELECT SUM(FilerReaction.Value)
	FROM dbo.FileSimple
	LEFT JOIN dbo.FilerReaction ON FileSimple.Id = FilerReaction.FileSimpleId 
	WHERE FileSimple.UserId = @userId and FilerReaction.Value < 0) as 'SumTaskUnLike'

,(SELECT COUNT(*) FROM dbo.FileSimple WHERE FileSimple.UserId = @userId) as 'NumberOfTask'

FROM dbo.UserData 
WHERE UserData.UserId = @userId

END
