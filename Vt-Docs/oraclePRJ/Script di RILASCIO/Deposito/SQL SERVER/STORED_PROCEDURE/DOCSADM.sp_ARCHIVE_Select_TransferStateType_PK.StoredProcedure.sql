USE [PCM_DEPOSITO_1]
GO
/****** Object:  StoredProcedure [DOCSADM].[sp_ARCHIVE_Select_TransferStateType_PK]    Script Date: 08/14/2013 11:50:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC   [DOCSADM].[sp_ARCHIVE_Select_TransferStateType_PK]  ( @System_ID int  )
AS
BEGIN
SELECT [ARCHIVE_TransferStateType].[System_ID], [ARCHIVE_TransferStateType].[Name] 
FROM [DOCSADM].[ARCHIVE_TransferStateType]
WHERE ( [System_ID] = @System_ID )
END
GO
