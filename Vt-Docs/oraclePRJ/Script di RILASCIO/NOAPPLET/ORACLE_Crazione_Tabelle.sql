CREATE TABLE "DPA_DESKTOP_APPS" 
   (	"NOME" VARCHAR2(100 BYTE) NOT NULL ENABLE, 
	"VERSIONE" VARCHAR2(20 BYTE) NOT NULL ENABLE, 
	"PATH" VARCHAR2(1000 BYTE), 
	"DESCRIZIONE" VARCHAR2(2000 BYTE), 
	 CONSTRAINT "DPA_DESCKTOP_APP_PK" PRIMARY KEY ("NOME", "VERSIONE")
   );