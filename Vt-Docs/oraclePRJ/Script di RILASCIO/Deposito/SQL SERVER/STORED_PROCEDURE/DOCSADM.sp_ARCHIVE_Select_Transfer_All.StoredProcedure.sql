USE [PCM_DEPOSITO_1]
GO
/****** Object:  StoredProcedure [DOCSADM].[sp_ARCHIVE_Select_Transfer_All]    Script Date: 08/14/2013 11:50:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC   [DOCSADM].[sp_ARCHIVE_Select_Transfer_All] 
AS
BEGIN
SELECT [ARCHIVE_Transfer].[System_ID], [ARCHIVE_Transfer].[Description], [ARCHIVE_Transfer].[Note] ,[ARCHIVE_Transfer].[ID_Amministrazione]
FROM [DOCSADM].[ARCHIVE_Transfer]
END
GO
