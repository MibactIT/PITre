

ALTER  PROCEDURE [DOCSADM].[utl_add_column]
	-- Add the parameters for the stored procedure here
		@versioneCD						Nvarchar(200),
		@nome_utente					Nvarchar(200), 
		@nome_tabella					Nvarchar(200),
		@nome_colonna					Nvarchar(200),
		@tipo_dato						Nvarchar(200),
		@val_default					Nvarchar(200),
	    @condizione_modifica_pregresso	Nvarchar(200),
	    @condizione_check				Nvarchar(200),
	    @RFU							Nvarchar(200)
AS
BEGIN

	-- controllo tipo dato da oracle a sql server
	IF SUBSTRING(@tipo_dato, 1,8) = 'varchar2' 	
	BEGIN
		SET @tipo_dato = (SELECT replace(@tipo_dato, 'varchar2', 'varchar'))	
	END
	IF SUBSTRING(@tipo_dato, 1,9) = 'nvarchar2'	
	BEGIN
		SET @tipo_dato = (SELECT replace(@tipo_dato, 'nvarchar2', 'nvarchar'))	
	END
	IF SUBSTRING(@tipo_dato, 1,6) = 'number'	
	BEGIN
		SET @tipo_dato = (SELECT replace(@tipo_dato, 'number', 'numeric'))	
	END
	IF SUBSTRING(@tipo_dato, 1,9) = 'timestamp'	
	BEGIN
		SET @tipo_dato = (SELECT replace(@tipo_dato, 'timestamp', 'datetime'))	
	END
	IF SUBSTRING(@tipo_dato, 1,4) = 'blob'		
	BEGIN
		SET @tipo_dato = (SELECT replace(@tipo_dato, 'blob', 'image'))	
	END
	
	DECLARE @istruzione			NVARCHAR(2000)
	DECLARE @insert_log			NVARCHAR(2000)
	DECLARE @data_eseguito		NVARCHAR(2000)
	DECLARE @comando_richiesto	NVARCHAR(2000)
	DECLARE @esito				NVARCHAR(2000)
	
	
	IF NOT EXISTS(SELECT * FROM SYSCOLUMNS WHERE name=@nome_colonna AND id IN (SELECT id FROM SYSOBJECTS WHERE name=@nome_tabella AND xtype='U'))
	BEGIN
	-- AGGIUNGO LA COLONNA 
   	
   		IF @val_default IS NOT NULL
		BEGIN
			SET @val_default = 'CONSTRAINT DF_'+@nome_tabella+'_'+@nome_colonna+' DEFAULT ('''+@val_default+''')'
		END 		
		ELSE 		
		BEGIN
			SET @val_default = ''
		END
   
		SET @istruzione = N'alter table [' + @nome_utente + '].
					[' + @nome_tabella + '] ADD ' + @nome_colonna +' 
					'+ @tipo_dato +' '+ @val_default

		EXECUTE sp_executesql @istruzione
   
		SET @comando_richiesto = 'Added column ' +@nome_colonna+' on '+@nome_tabella+''
		SET @esito = 'Esito positivo'
   
	END 	
	ELSE 	
	BEGIN	
	-- COLONNA GIÀ ESISTENTE
	
	    SET @comando_richiesto = 'Adding column ' +@nome_colonna+' on '+@nome_tabella+''
		SET @esito = 'ESITO NEGATIVO - COLONNA GIÀ ESISTENTE'
       
	END
	
	EXECUTE [DOCSADM].utl_insert_log @nome_utente, getdate, @comando_richiesto, @versioneCD, @esito
	
END       
