USE [PCM_DEPOSITO_1]
GO
/****** Object:  StoredProcedure [DOCSADM].[sp_ARCHIVE_Insert_TransferPolicy]    Script Date: 08/14/2013 11:50:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC   [DOCSADM].[sp_ARCHIVE_Insert_TransferPolicy]  
( @Description varchar (200) , @Enabled int , @Transfer_ID int , @TransferPolicyType_ID int , @TransferPolicyState_ID int ,@Registro_ID int , @UO_ID int , 
@IncludiSottoalberoUO int , @Tipologia_ID int , @Titolario_ID int , @ClasseTitolario varchar (100) , 
@IncludiSottoalberoClasseTit int , @AnnoCreazioneDa int , @AnnoCreazioneA int , @AnnoProtocollazioneDa int , @AnnoProtocollazioneA int , @AnnoChiusuraDa int , @AnnoChiusuraA int , @System_ID int OUTPUT )
AS
BEGIN
INSERT INTO [DOCSADM].[ARCHIVE_TransferPolicy] ( [Description], [Enabled], [Transfer_ID], [TransferPolicyType_ID],[TransferPolicyState_ID], [Registro_ID], [UO_ID], [IncludiSottoalberoUO], [Tipologia_ID], [Titolario_ID], [ClasseTitolario], [IncludiSottoalberoClasseTit], [AnnoCreazioneDa], [AnnoCreazioneA], [AnnoProtocollazioneDa], [AnnoProtocollazioneA], [AnnoChiusuraDa], [AnnoChiusuraA] ) 
VALUES ( @Description, @Enabled, @Transfer_ID, @TransferPolicyType_ID,@TransferPolicyState_ID, @Registro_ID, @UO_ID, @IncludiSottoalberoUO, @Tipologia_ID, @Titolario_ID, @ClasseTitolario, @IncludiSottoalberoClasseTit, @AnnoCreazioneDa, @AnnoCreazioneA, @AnnoProtocollazioneDa, @AnnoProtocollazioneA, @AnnoChiusuraDa, @AnnoChiusuraA ) 
SET @System_ID = SCOPE_IDENTITY()
END
GO
