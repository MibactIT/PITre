
INSERT INTO [DPA_PARAMETRO_SERVIZI]
	   ([DESCRIZIONE], [TIPO_VALORE])
 VALUES
	   ('DOCNUMBER', 'STRING')

INSERT INTO [DPA_SERVIZI_ESTERNI]
	   ([DESCRIZIONE], [SERVIZIO])
 VALUES
	   ('SERVIZIO FATTURAZIONE', 'http://sp5t.it.nttdata-emea.com')
           
INSERT INTO [DPA_PARAMETRI_SERVIZIO]
	   ([ID_SERVIZIO]
	   ,[ID_PARAMETRO])
 VALUES
	   ((SELECT SYSTEM_ID FROM DPA_SERVIZI_ESTERNI WHERE DESCRIZIONE = 'SERVIZIO FATTURAZIONE')
	   ,(SELECT SYSTEM_ID FROM DPA_PARAMETRO_SERVIZI WHERE DESCRIZIONE = 'ID_DOCUMENTO'))
