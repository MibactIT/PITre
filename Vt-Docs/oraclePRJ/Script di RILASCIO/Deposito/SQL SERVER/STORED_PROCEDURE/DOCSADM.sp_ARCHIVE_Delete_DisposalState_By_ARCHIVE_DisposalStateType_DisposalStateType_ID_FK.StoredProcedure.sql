USE [PCM_DEPOSITO_1]
GO
/****** Object:  StoredProcedure [DOCSADM].[sp_ARCHIVE_Delete_DisposalState_By_ARCHIVE_DisposalStateType_DisposalStateType_ID_FK]    Script Date: 08/14/2013 11:50:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC   [DOCSADM].[sp_ARCHIVE_Delete_DisposalState_By_ARCHIVE_DisposalStateType_DisposalStateType_ID_FK]  ( @DisposalStateType_ID int  )
AS
BEGIN
DELETE [DOCSADM].[ARCHIVE_DisposalState]
WHERE ( [DisposalStateType_ID] = @DisposalStateType_ID )
END
GO
