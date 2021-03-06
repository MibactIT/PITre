USE [PCM_DEPOSITO_1]
GO
/****** Object:  UserDefinedFunction [DOCSADM].[fn_ARCHIVE_GetDocumentsByTransferPolicyList]    Script Date: 05/02/2013 11:36:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =========================================================
-- Author:		Giovanni Olivari
-- Create date: 29/04/2013
-- Description:	Restituisce la lista dei documenti aggregati
-- =========================================================
ALTER PROCEDURE [DOCSADM].[sp_ARCHIVE_BE_GetDocumentsByTransferPolicyList]
(	
	@transferPolicyList VARCHAR(1000)
)
AS
BEGIN

	DECLARE @sql_string nvarchar(MAX)
	DECLARE @countDistinct int = 0



	-- Create temp table
	--
	IF OBJECT_ID('tempdb..#transferPolicyListTable') IS NOT NULL DROP TABLE #transferPolicyListTable
	CREATE TABLE #transferPolicyListTable
	(
		ID int
	)

	SET @sql_string = CAST(N'
		INSERT INTO #transferPolicyListTable (ID)
		SELECT SYSTEM_ID FROM ARCHIVE_TransferPolicy
		WHERE SYSTEM_ID IN (' AS NVARCHAR(MAX)) + CAST(@transferPolicyList AS NVARCHAR(MAX)) + CAST(N')' AS NVARCHAR(MAX))
		
	PRINT @sql_string;

	EXECUTE sp_executesql @sql_string;



	-- Conteggio DISTINCT dei documenti
	--
	SELECT @countDistinct = COUNT(DISTINCT Profile_ID) 
	FROM ARCHIVE_TEMPPROFILE D 
	WHERE D.TRANSFERPOLICY_ID IN (SELECT ID FROM #transferPolicyListTable)

	PRINT @countDistinct


	-- Nell'aggregazione vengono considerati soltanto le classi titolario dei fascicoli procedimentali (P) inclusi nel versamento
	-- e le classi titolario dei fascicoli generali (G)
	--		
	SET @sql_string = CAST(N'
		SELECT REGISTRO, TITOLARIO, CLASSETITOLARIO, TIPOLOGIA, ANNO_CREAZIONE
		, COUNT(*) TOTALE, ' AS NVARCHAR(MAX)) + CAST(@countDistinct AS NVARCHAR(MAX)) + CAST(' COUNTDISTINCT
		FROM 
		(
			SELECT DISTINCT D.PROFILE_ID, D.REGISTRO, F.TITOLARIO, F.CLASSETITOLARIO, D.TIPOLOGIA, YEAR(D.DATACREAZIONE) ''ANNO_CREAZIONE''
			FROM ARCHIVE_TEMPPROFILE D 
			LEFT OUTER JOIN
				(
				SELECT DISTINCT DF.PROFILE_ID, F.TITOLARIO, F.CLASSETITOLARIO
				FROM ARCHIVE_TEMP_PROJECT_PROFILE DF
				INNER JOIN ARCHIVE_TEMPPROJECT F ON DF.TRANSFERPOLICY_ID = F.TRANSFERPOLICY_ID AND DF.PROJECT_ID = F.PROJECT_ID
				WHERE F.TRANSFERPOLICY_ID IN (' AS NVARCHAR(MAX)) + CAST(@transferPolicyList AS NVARCHAR(MAX)) + CAST(') 
				AND DF.TRANSFERPOLICY_ID IN (' AS NVARCHAR(MAX)) + CAST(@transferPolicyList AS NVARCHAR(MAX)) + CAST(')
				) F ON D.PROFILE_ID = F.PROFILE_ID
			WHERE D.TRANSFERPOLICY_ID IN (' AS NVARCHAR(MAX)) + CAST(@transferPolicyList AS NVARCHAR(MAX)) + CAST(')
		) T1
		GROUP BY REGISTRO, TITOLARIO, CLASSETITOLARIO, TIPOLOGIA, ANNO_CREAZIONE'AS NVARCHAR(MAX))

		
	PRINT @sql_string
		
	EXECUTE sp_executesql @sql_string;

END
