/****** Script for SelectTopNRows command from SSMS  ******/
--SELECT TOP (1000) * FROM [Memy].[dbo].[UserSimple]

--SELECT TOP (1000) * FROM [Memy].[dbo].[UserToken] 

--select HASHBYTES('SHA2_512',N'73-6C-61-77-65-6B-73-6D-6F-74-65-72-38-32-40-67-6D-61-69-6C-2E-63-6F-6D')
--select HASHBYTES('SHA2_512',N'slaweksmoter82@gmail.com')

--delete from UserSimple
--delete from [UserToken]
--exec dbo.RegisterUser N'slaweksmoter82@gmail.com',N'slaweksmoter82@gmail.com',N'73-6C-61-77-65-6B-73-6D-6F-74-65-72-38-32-40-67-6D-61-69-6C-2E-63-6F-6D','Admin'

--exec LoginUser 'slaweksmoter82@gmail.com',N'73-6C-61-77-65-6B-73-6D-6F-74-65-72-38-32-40-67-6D-61-69-6C-2E-63-6F-6D',1
--exec logoutUser 'CACA62A3-D773-47B8-B046-2914D70438FA'

--exec [CheckToken] 'CACA62A3-D773-47B8-B046-2914D70438FA'
--exec [CheckToken] '5D2E8152-733A-4F8B-B43E-984539777DA9'

--exec [CheckToken] '5D2E8152-733A-4F8B-B43E-984539777DAa'

--SELECT TOP (1000) * FROM [Memy].[dbo].[FileSimple] order by 1 desc
--SELECT TOP (1000) * FROM [Memy].[dbo].[FileData]
--SELECT TOP (1000) * FROM [Memy].[dbo].[FileTagList]
--SELECT TOP (1000) * FROM [Memy].[dbo].[FileTagConnected]
--SELECT * FROM memy.dbo.FilerReaction
--delete FROM memy.dbo.FilerReaction

--EXEC dbo.[InsertReactionFile] 8005, -5,'789105F2-DAFC-493D-83A8-6352EEC55A50'  

--delete FROM [dbo].[FileSimple]
--delete FROM [Memy].[dbo].[FileData]
--delete FROM [Memy].[dbo].[FileTagConnected]
--delete FROM [Memy].[dbo].[FileTagList]

--exec Memy.dbo.GetFileByDate 0,10,'waiting',0,'empty','today','907B2193-EB7D-4AA3-94A9-249A02C3527C'





