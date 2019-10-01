--------------------------------------------------------
--  DDL for Table DPA_SUPPORTO
--------------------------------------------------------

  CREATE TABLE "ITCOLL_6GIU12"."DPA_SUPPORTO" 
   (	"SYSTEM_ID" NUMBER, 
	"COPIA" NUMBER, 
	"DATA_PRODUZIONE" DATE, 
	"VAR_COLLOCAZIONE_FISICA" VARCHAR2(64 BYTE), 
	"DATA_ULTIMA_VERIFICA" DATE, 
	"DATA_ELIMINAZIONE" DATE, 
	"ESITO_ULTIMA_VERIFICA" NUMBER, 
	"VERIFICHE_EFFETTUATE" NUMBER, 
	"DATA_PROX_VERIFICA" DATE, 
	"DATA_APPO_MARCA" DATE, 
	"DATA_SCADENZA_MARCA" DATE, 
	"VAR_MARCA_TEMPORALE" VARCHAR2(3000 CHAR), 
	"ID_CONSERVAZIONE" NUMBER, 
	"ID_TIPO_SUPPORTO" NUMBER, 
	"CHA_STATO" CHAR(1 CHAR), 
	"VAR_NOTE" VARCHAR2(256 BYTE), 
	"PERC_VERIFICA" NUMBER, 
	"NUM_MARCA" NUMBER
   ) ;