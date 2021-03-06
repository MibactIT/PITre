USE [PCM_DEPOSITO_1]
GO
/****** Object:  StoredProcedure [DOCSADM].[sp_ARCHIVE_Select_TempProfileViewByTransferID]    Script Date: 08/14/2013 11:50:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC   [DOCSADM].[sp_ARCHIVE_Select_TempProfileViewByTransferID]  ( @Transfer_ID int  )
AS
BEGIN
select distinct
	d.SYSTEM_ID DocID-- system_id della profile
	, d.NUM_PROTO -- numero di protocollo
	, d.NUM_ANNO_PROTO
	, tp.Registro
	, tp.UO
	, tp.OggettoDocumento
	, tp.TipoDocumento
	, tp.DataCreazione
	, tp.Tipologia
	, tp.TipoTrasferimento_Versamento
	, T1.Code,
	T2.Corr
from ARCHIVE_TempProfile tp
inner join ARCHIVE_TransferPolicy p on tp.TransferPolicy_ID = p.System_ID
left outer join PROFILE d on tp.Profile_ID = d.SYSTEM_ID
left outer join
	(
	SELECT 
	  [ID],
	  STUFF((
		SELECT ' ; ' + [Name]
		FROM 
			(
			select distinct tpp.profile_id ID, f.ProjectCode NAME
			from ARCHIVE_Temp_Project_Profile tpp
			inner join ARCHIVE_TransferPolicy tp on tpp.TransferPolicy_ID = tp.System_ID
			left outer join ARCHIVE_TempProject f on tpp.Project_ID = f.Project_ID
			where tp.Transfer_ID = @Transfer_ID and 
			tp.Enabled = 1
			) T 
		WHERE (ID = T1.ID) 
		FOR XML PATH (''))
	  ,1,2,'') AS Code
	FROM 
		(
		select distinct tpp.profile_id ID, f.ProjectCode NAME
		from ARCHIVE_Temp_Project_Profile tpp
		inner join ARCHIVE_TransferPolicy tp on tpp.TransferPolicy_ID = tp.System_ID
		left outer join ARCHIVE_TempProject f on tpp.Project_ID = f.Project_ID
		where tp.Transfer_ID = @Transfer_ID and
		tp.Enabled = 1
		) T1
	GROUP BY ID	
	) T1 on tp.Profile_ID = T1.ID
left outer join
	(
	SELECT 
	  [ID],
	  STUFF((
		SELECT ' ; ' + CORR 
		FROM 
			(
			select distinct tp.profile_id ID, case
				when tp.TipoDocumento = 'A' then COR_MITT.VAR_DESC_CORR
				when tp.TipoDocumento = 'P' then COR_DEST.VAR_DESC_CORR
			end CORR
			from ARCHIVE_TempProfile tp
				inner join ARCHIVE_TransferPolicy p on tp.TransferPolicy_ID = p.System_ID
				left outer join DPA_DOC_ARRIVO_PAR MITT on tp.Profile_ID = MITT.ID_PROFILE and MITT.CHA_TIPO_MITT_DEST = 'M'
				left outer join DPA_DOC_ARRIVO_PAR DEST on tp.Profile_ID = DEST.ID_PROFILE and DEST.CHA_TIPO_MITT_DEST = 'D'
				left outer join DPA_CORR_GLOBALI COR_MITT on MITT.ID_MITT_DEST = COR_MITT.SYSTEM_ID
				left outer join DPA_CORR_GLOBALI COR_DEST on DEST.ID_MITT_DEST = COR_DEST.SYSTEM_ID
			where p.Transfer_ID = @Transfer_ID and 
			p.Enabled = 1
			) T 
		WHERE (ID = T1.ID) 
		FOR XML PATH (''))
	  ,1,2,'') AS Corr
	FROM 
		(
		select distinct tp.profile_id ID, case
				when tp.TipoDocumento = 'A' then COR_MITT.VAR_DESC_CORR
				when tp.TipoDocumento = 'P' then COR_DEST.VAR_DESC_CORR
			end CORR
			from ARCHIVE_TempProfile tp
				inner join ARCHIVE_TransferPolicy p on tp.TransferPolicy_ID = p.System_ID
				left outer join DPA_DOC_ARRIVO_PAR MITT on tp.Profile_ID = MITT.ID_PROFILE and MITT.CHA_TIPO_MITT_DEST = 'M'
				left outer join DPA_DOC_ARRIVO_PAR DEST on tp.Profile_ID = DEST.ID_PROFILE and DEST.CHA_TIPO_MITT_DEST = 'D'
				left outer join DPA_CORR_GLOBALI COR_MITT on MITT.ID_MITT_DEST = COR_MITT.SYSTEM_ID
				left outer join DPA_CORR_GLOBALI COR_DEST on DEST.ID_MITT_DEST = COR_DEST.SYSTEM_ID
			where p.Transfer_ID = @Transfer_ID and 
			p.Enabled = 1
		) T1
	GROUP BY ID	
	) T2 on tp.Profile_ID=T2.ID
where p.Transfer_ID = @Transfer_ID and
p.Enabled = 1

END
GO
