USE [PCM_DEPOSITO_1]
GO
/****** Object:  StoredProcedure [DOCSADM].[sp_ARCHIVE_Select_TransferPolicy_ProfileType_By_TransferPolicy_ID]    Script Date: 08/14/2013 11:50:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CReate PROC   [DOCSADM].[sp_ARCHIVE_Select_TransferPolicy_ProfileType_By_TransferPolicy_ID]  ( @TransferPolicy_ID int  )
AS
BEGIN
SELECT [ARCHIVE_TransferPolicy_ProfileType].[TransferPolicy_ID], [ARCHIVE_TransferPolicy_ProfileType].[ProfileType_ID]
FROM [DOCSADM].[ARCHIVE_TransferPolicy_ProfileType] WHERE ( [TransferPolicy_ID] = @TransferPolicy_ID )
END
GO
