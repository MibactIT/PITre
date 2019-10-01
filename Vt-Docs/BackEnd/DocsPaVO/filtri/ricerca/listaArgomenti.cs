using System;
using System.Xml.Serialization;

namespace DocsPaVO.filtri.ricerca 
{
	// TODO: Aggiungere ASSEGNATARIO_RESPONSABILE, CODICE_FALDONE,	NOTIFICATO,	IN_EVIDENZA,ANNULLATO,NOTE,	FIRMATO_DA,	TIPO_ATTO
	
	/// <summary>
	/// </summary>
	[XmlType("FiltriDocumento")]
	public enum listaArgomenti 
	{
		TIPO,
		REGISTRO,
		OGGETTO,
		ID_OGGETTO,
        OGGETTO_DOCUMENTO_PRINCIPALE,
        OGGETTO_ALLEGATO,
		MITT_DEST,
		ID_MITT_DEST,
		VIS_STORICO_MITT_DEST,
		NUM_PROTOCOLLO,
		NUM_PROTOCOLLO_DAL,
		NUM_PROTOCOLLO_AL,
		DATA_PROT_IL,
		DATA_PROT_SUCCESSIVA_AL,
		DATA_PROT_PRECEDENTE_IL,
		TIPO_DOCUMENTO,
		MITTENTE_INTERMEDIO,
		ID_MITTENTE_INTERMEDIO,
		DATA_PROT_MITTENTE_IL,
		DATA_PROT_MITTENTE_SUCCESSIVA_AL,
		DATA_PROT_MITTENTE_PRECEDENTE_IL,
		DATA_ARRIVO_IL,
		DATA_ARRIVO_SUCCESSIVA_AL,
		DATA_ARRIVO_PRECEDENTE_IL,
		PROTOCOLLO_MITTENTE,
		DA_PROTOCOLLARE,
		MANCANZA_IMMAGINE,
		MANCANZA_ASSEGNAZIONE,
		MANCANZA_FASCICOLAZIONE,
		DATA_CREAZIONE_IL,
		DATA_CREAZIONE_SUCCESSIVA_AL,
		DATA_CREAZIONE_PRECEDENTE_IL,
        DATA_SCADENZA_IL,
        DATA_SCADENZA_SUCCESSIVA_AL,
        DATA_SCADENZA_PRECEDENTE_IL,
		PAROLE_CHIAVE,
		DOCNUMBER,
		DOCNUMBER_DAL,
		DOCNUMBER_AL,
		TIPO_ATTO,
		FIRMATARIO_NOME,
		FIRMATARIO_COGNOME,
		NUM_PROTO_EMERGENZA,
		DATA_PROTO_EMERGENZA_IL,
        DATA_PROTO_EMERGENZA_SUCCESSIVA_AL,
        DATA_PROTO_EMERGENZA_PRECEDENTE_IL,
		SEGNATURA,
		NUM_OGGETTO,
		COMMISSIONE_REF,
		ANNO_PROTOCOLLO,
		NOTE,
		ANNULLATO,
		EVIDENZA,
		VISUALIZZA_TOP_N_DOCUMENTI,
		ID_PARENT,
		RICERCA_FULL_TEXT,
		TESTO_RICERCA_FULL_TEXT,
		REGISTRI_UTENTE_IN_CONDITION,
        REGISTRI_UTENTE_IN_CONDITION_CON_NULL,
		PROFILAZIONE_DINAMICA,
		ID_UO_REF,
		ID_UO_PROT,
		ID_UO_PROT_GERARCHIA,
		DESC_UO_REF,
		ID_DESTINATARIO,
		ID_DESCR_DESTINATARIO,
		DIAGRAMMA_STATO_DOC,
        CONDIZIONE_STATO_DOCUMENTO,
		CONDIZIONE_ORDINAMENTO,
        ID_UO_CREATORE,
        ID_RUOLO_CREATORE,
        ID_PEOPLE_CREATORE,
        DESC_UO_CREATORE,
        DESC_RUOLO_CREATORE,
        DESC_PEOPLE_CREATORE,
        DOC_IN_ADL,
        VISIBILITA_NOTE,
        PROT_ARRIVO,
        PROT_PARTENZA,
        PROT_INTERNO,
        GRIGIO,
        PREDISPOSTO,
        ALLEGATO,
        TRASMESSI_CON,
        TRASMESSI_SENZA,
        MEZZO_SPEDIZIONE,
        CONSERVAZIONE,
        COD_MITT_DEST,
        MITT_DEST_STORICIZZATI,
        DEPOSITO,
        CODICE_FASCICOLO,
        ID_TITOLARIO,
        NUM_PROT_TITOLARIO,
        FIRMATO,
        TIPO_FILE_ACQUISITO,
        STAMPA_REG,
        DATA_PROT_SC,
        DATA_PROT_MC,
        DATA_PROT_TODAY,
        DATA_PROTO_YESTERDAY,
        DATA_PROTO_LAST_SEVEN_DAYS,
        DATA_PROTO_LAST_THIRTY_ONE_DAYS,
        DATA_CREAZ_SC,
        DATA_CREAZ_MC,
        DATA_CREAZ_TODAY,
        DATA_CREAZ_YESTERDAY,
        DATA_CREAZ_LAST_SEVEN_DAYS,
        DATA_CREAZ_LAST_THIRTY_ONE_DAYS,
        DATA_STAMPA_SC,
        DATA_STAMPA_MC,
        DATA_STAMPA_TODAY,
        DATA_SCAD_SC,
        DATA_SCAD_MC,
        DATA_SCAD_TODAY,
        DATA_PROT_MITTENTE_SC,
        DATA_PROT_MITTENTE_MC,
        DATA_PROT_MITTENTE_TODAY,
        DATA_ARRIVO_SC,
        DATA_ARRIVO_MC,
        DATA_ARRIVO_TODAY,
        DATA_PROTO_EMERGENZA_SC,
        DATA_PROTO_EMERGENZA_MC,
        DATA_PROTO_EMERGENZA_TODAY,
        DOC_SPEDITI,
        DOC_SPEDITI_TIPO,
        DOC_SPEDITI_ESITO,
        DATA_SPEDIZIONE_DA,
        DATA_SPEDIZIONE_A,
        TEMPLATE_CAMPI_COMUNI_DOC,
        TEMPLATE_CAMPI_COMUNI_FASC,
        CODICE_DESCRIZIONE_AMMINISTRAZIONE,
        CODICE_TIPO_NOTIFICA,
        DATA_TIPO_NOTIFICA_DA,
        DATA_TIPO_NOTIFICA_A,
        DATA_TIPO_NOTIFICA_TODAY,
        DATA_TIPO_NOTIFICA_NESSUNA,
        CODICE_TIPO_NOTIFICA_PITRE,
        DATA_TIPO_NOTIFICA_DA_PITRE,
        DATA_TIPO_NOTIFICA_A_PITRE,
        DATA_TIPO_NOTIFICA_TODAY_PITRE,
        DATA_TIPO_NOTIFICA_NESSUNA_PITRE,
        CON_TIMESTAMP,
        SENZA_TIMESTAMP,
        TIMESTAMP_SCADUTO,
        TIMESTAMP_SCADE_PRIMA_DI,
        STATO_CONSOLIDAMENTO,
        DATA_CONSOLIDAMENTO_DA,
        DATA_CONSOLIDAMENTO_A,
        ID_UTENTE_CONSOLIDANTE,
        ID_RUOLO_CONSOLIDANTE,
        ORACLE_FIELD_FOR_ORDER,
        SQL_FIELD_FOR_ORDER,
        PROFILATION_FIELD_FOR_ORDER,
        ORDER_DIRECTION,
        DATA_TIPO_CONSOLIDAMENTO,
        NUMERO_VERSIONI,
        DATA_ULTIMA_MODIFICA_IL,
        DATA_ULTIMA_MODIFICA_SUCCESSIVA_AL,
        DATA_ULTIMA_MODIFICA_PRECEDENTE_IL,
        CONTATORE_GRIGLIE_NO_CUSTOM,
        VISIBILITA_T_A,
        RIFERIMENTO_MITTENTE,
        NUMERO_ALLEGATI,
        REP_RF_AOO,
        IN_CONSERVAZIONE,
        FROM_RICERCA_VIS,
        COD_EXT_APP,
        ANNO_PUBBLICAZIONE_DEL,
        ANNO_PUBBLICAZIONE_DET,
        SEARCH_DOCUMENT_SIMPLE,
        IN_CONSERVAZIONE_ESIB,
        STATO_CONSERVAZIONE,
        DATA_VERSAMENTO_DA,
        DATA_VERSAMENTO_A,
        DATA_VERSAMENTO_IL,
        POLICY_CODICE,
        POLICY_NUM_ESECUZIONE,
        DATA_EXEC_POLICY_DA,
        DATA_EXEC_POLICY_A,
        DATA_EXEC_POLICY_IL,
        DATA_EXEC_POLICY_YESTERDAY,
        FIRMA_ELETTRONICA,
        ID_RUOLO_FIRMATARIO_ELETTRONICA,
        ID_UTENTE_FIRMATARIO_ELETTRONICA,
        DESC_FIRMATARIO_ELETTRONICA,
        DOC_MAI_SPEDITI,
        DOC_MAI_SPEDITI_DA_UTENTE,
        DOC_MAI_SPEDITI_DA_RUOLO,
        DOC_MAI_TRASMESSI_DA_UTENTE,
        DOC_MAI_TRASMESSI_DA_RUOLO,
        DOC_REPERTORIATO,
        NUM_REPERTORIO,
        NUM_REPERTORIO_DAL,
        NUM_REPERTORIO_AL,
        DATA_REPERTORIO_IL,
        DATA_REPERTORIO_SUCCESSIVA_AL,
        DATA_REPERTORIO_PRECEDENTE_IL,
        DATA_REPERTORIO_TODAY,
        DATA_REPERTORIO_MC,
        DATA_REPERTORIO_SC,
        ESTENDI_A_NODI_FIGLI_E_FASCICOLI,
        DOCUMENTI_IN_RISPOSTA
	}
}
