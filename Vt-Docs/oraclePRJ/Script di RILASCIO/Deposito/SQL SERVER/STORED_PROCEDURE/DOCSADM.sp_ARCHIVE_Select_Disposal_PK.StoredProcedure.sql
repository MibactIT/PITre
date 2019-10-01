USE [PCM_DEPOSITO_1]
GO
/****** Object:  StoredProcedure [DOCSADM].[sp_ARCHIVE_Select_Disposal_PK]    Script Date: 08/14/2013 11:50:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC   [DOCSADM].[sp_ARCHIVE_Select_Disposal_PK]  ( @System_ID int  )
AS
BEGIN
SELECT [ARCHIVE_Disposal].[System_ID], [ARCHIVE_Disposal].[Description], [ARCHIVE_Disposal].[Note] , [ARCHIVE_Disposal].[ID_Amministrazione] 
FROM [DOCSADM].[ARCHIVE_Disposal]
WHERE ( [System_ID] = @System_ID )
END
GO
