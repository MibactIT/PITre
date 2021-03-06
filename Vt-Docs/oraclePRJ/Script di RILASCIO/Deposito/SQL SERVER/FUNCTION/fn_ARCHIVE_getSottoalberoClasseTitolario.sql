USE [PCM_040413]
GO
/****** Object:  UserDefinedFunction [DOCSADM].[fn_ARCHIVE_getSottoalberoClasseTitolario]    Script Date: 04/23/2013 15:52:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ===================================================
-- Author:		Giovanni Olivari
-- Create date: 23/04/2013
-- Description:	Restituisce la classe titolario
--              e il suo sottoalbero
-- ===================================================
ALTER FUNCTION [DOCSADM].[fn_ARCHIVE_getSottoalberoClasseTitolario]
(	
	@idTitolario int,              -- Identificativo del titolario
	@classeTitolario VARCHAR(200)  -- Voce di classificazione del titolario
)
RETURNS TABLE 
AS
RETURN 
(
	WITH GERARCHIA_CT AS
	(
	SELECT P.SYSTEM_ID, P.ID_PARENT, P.ID_FASCICOLO, P.VAR_CODICE, P.CHA_TIPO_FASCICOLO, P.CHA_TIPO_PROJ, P.ID_REGISTRO, P.VAR_COD_LIV1
	FROM DOCSADM.PROJECT P
	WHERE P.ID_TITOLARIO = @idTitolario
	AND P.VAR_CODICE = @classeTitolario
	UNION ALL
	SELECT P.SYSTEM_ID, P.ID_PARENT, P.ID_FASCICOLO, P.VAR_CODICE, P.CHA_TIPO_FASCICOLO, P.CHA_TIPO_PROJ, P.ID_REGISTRO, P.VAR_COD_LIV1
	FROM DOCSADM.PROJECT P INNER JOIN GERARCHIA_CT GCT ON GCT.SYSTEM_ID = P.ID_PARENT
	)
	SELECT DISTINCT P.SYSTEM_ID, P.ID_PARENT, P.ID_FASCICOLO, P.VAR_CODICE, P.CHA_TIPO_FASCICOLO, P.CHA_TIPO_PROJ, P.ID_REGISTRO, P.VAR_COD_LIV1 FROM GERARCHIA_CT P
)
