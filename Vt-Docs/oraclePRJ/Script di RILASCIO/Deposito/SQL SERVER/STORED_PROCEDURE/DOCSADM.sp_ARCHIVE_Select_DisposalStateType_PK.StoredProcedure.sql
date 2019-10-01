USE [PCM_DEPOSITO_1]
GO
/****** Object:  StoredProcedure [DOCSADM].[sp_ARCHIVE_Select_DisposalStateType_PK]    Script Date: 08/14/2013 11:50:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC   [DOCSADM].[sp_ARCHIVE_Select_DisposalStateType_PK]  ( @System_ID int  )
AS
BEGIN
SELECT [ARCHIVE_DisposalStateType].[System_ID], [ARCHIVE_DisposalStateType].[Name] 
FROM [DOCSADM].[ARCHIVE_DisposalStateType]
WHERE ( [System_ID] = @System_ID )
END
GO
