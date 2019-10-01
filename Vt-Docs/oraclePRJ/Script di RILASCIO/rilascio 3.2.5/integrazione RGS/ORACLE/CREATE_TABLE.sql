CREATE TABLE DPA_TIPO_MESSAGGI_FLUSSO
(
  ID_MESSAGGIO           NUMBER NOT NULL,
  DESCRIZIONE     	 	 VARCHAR(2000)      NOT NULL ,
  CHA_MESSAGGIO_INIZIALE	CHAR(1),
  CHA_MESSAGGIO_FINALE		CHAR(1),
  CONSTRAINT CONSTRANT_TIPO_MESSAGGI_FLUSSO PRIMARY KEY(ID_MESSAGGIO)
);

CREATE TABLE DPA_FLUSSO_PROCEDURALE
(
  SYSTEM_ID           	 NUMBER NOT NULL,
  ID_PROCESSO    	 	 VARCHAR(2000)      NOT NULL,
  ID_MESSAGGIO			 NUMBER  NOT NULL,
  DTA_ARRIVO   		     DATE,
  ID_PROFILE			 NUMBER NOT NULL,
  NOME_REGISTRO			 VARCHAR(2000),  --INFO RGS
  NUMERO_REGISTRO		 NUMBER,			 --INFO RGS
  DTA_REGISTRO			 DATE		 --INFO RGS
);


CREATE TABLE DPA_CONTESTO_PROCEDURALE
(
	SYSTEM_ID           		NUMBER NOT NULL,	
	TIPO_CONTESTO_PROCEDURALE	VARCHAR(100),
	NOME						VARCHAR(256),
	FAMIGLIA					NUMBER,
	VERSIONE					NUMBER
);

ALTER TABLE DPA_TIPO_ATTO 
ADD ID_CONTESTO_PROCEDURALE NUMBER;


CREATE TABLE DPA_CORR_INTEROP
(
  ID_CORR           NUMBER NOT NULL,
  CHA_INTEROPERANTE_RGS    	 	CHAR(1),
  CONSTRAINT CONSTRANT_DPA_CORR_INTEROP PRIMARY KEY(ID_CORR)
)

CREATE TABLE DPA_FLUSSO_MESSAGGI
(
  SYSTEM_ID           		NUMBER NOT NULL,	
  ID_MESSAGGIO              NUMBER,
  ID_MESSAGGIO_SUCCESSIVO   NUMBER
);
