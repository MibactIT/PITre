USE [GFD_SVIL]
GO
/****** Object:  StoredProcedure [DOCSADM].[IS_LoadSimpInteropRecordInfo]    Script Date: 03/08/2013 11:19:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER Procedure [DOCSADM].[IS_LoadSimpInteropRecordInfo] 
@p_DocumentId INT,
@p_SenderAdministrationCode VARCHAR(4000) OUTPUT ,
@p_SenderAOOCode VARCHAR(4000) OUTPUT ,
@p_SenderRecordNumber INT OUTPUT ,
@p_SenderRecordDate DATETIME OUTPUT ,
@p_ReceiverAdministrationCode VARCHAR(4000) OUTPUT ,
@p_ReceiverAOOCode VARCHAR(4000) OUTPUT ,
@p_ReceiverRecordNumber INT OUTPUT ,
@p_ReceiverRecordDate DATETIME OUTPUT ,
@p_SenderUrl VARCHAR(4000) OUTPUT ,
-- new parameter added lately  
 @p_ReceiverCode VARCHAR(4000) OUTPUT AS
Begin
-- Caricamento delle informazioni sul protocollo mittente
   Select   @p_SenderAdministrationCode = SenderAdministrationCode
   , @p_SenderAOOCode = AOOCode
   , @p_SenderRecordNumber = RecordNumber
   , @p_SenderRecordDate = RecordDate
   , @p_SenderUrl = SenderUrl
   , @p_ReceiverCode = ReceiverCode
   From SimpInteropReceivedMessage
   Where ProfileId = @p_DocumentId

-- Caricamento informazioni sul protocollo creato nell'amministrazione destinataria
   Select   @p_ReceiverAOOCode =(Select var_codice From dpa_el_registri Where System_Id = Id_Registro), @p_ReceiverRecordNumber = Num_Proto, @p_ReceiverRecordDate = Dta_proto, @p_ReceiverAdministrationCode =(Select var_codice_amm From dpa_amministra Where system_id =(Select id_amm From dpa_corr_globali Where system_id = Id_Uo_Prot))
   From profile
   Where System_id = @p_DocumentId

End 

