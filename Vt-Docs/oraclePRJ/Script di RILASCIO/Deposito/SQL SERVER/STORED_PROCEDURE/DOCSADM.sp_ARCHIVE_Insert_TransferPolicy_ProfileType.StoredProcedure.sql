USE [PCM_DEPOSITO_1]
GO
/****** Object:  StoredProcedure [DOCSADM].[sp_ARCHIVE_Insert_TransferPolicy_ProfileType]    Script Date: 08/14/2013 11:50:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC   [DOCSADM].[sp_ARCHIVE_Insert_TransferPolicy_ProfileType]  ( @TransferPolicy_ID int , @ProfileType int, @RowsAffected int out)
AS
BEGIN
INSERT INTO [DOCSADM].[ARCHIVE_TransferPolicy_ProfileType] ( [TransferPolicy_ID], [ProfileType_ID] ) 
VALUES ( @TransferPolicy_ID  , @ProfileType ) 
set @RowsAffected = @@ROWCOUNT
END
GO
