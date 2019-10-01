--------------------------------------------------------
--  DDL for Table MV_PROSPETTI_DOCUMENTI
--------------------------------------------------------

  CREATE TABLE "ITCOLL_6GIU12"."MV_PROSPETTI_DOCUMENTI" 
   (	"ID_REGISTRO_0FORNULL" NUMBER, 
	"CREATION_YEAR" DATE, 
	"PROTO_MONTH" DATE, 
	"NUM_ANNO_PROTO" NUMBER(10,0), 
	"ID_UO_PROT" NUMBER, 
	"CHA_DA_PROTO" VARCHAR2(1 BYTE), 
	"CHA_IN_CESTINO_0FORNULL" VARCHAR2(1 BYTE), 
	"VAR_SEDE" VARCHAR2(64 BYTE), 
	"CHA_TIPO_PROTO" VARCHAR2(1 BYTE), 
	"FLAG_IMMAGINE" VARCHAR2(4000 BYTE), 
	"FLAG_ANNULLATO" NUMBER, 
	"FLAG_NUM_PROTO" NUMBER, 
	"UNDISTINCT_COUNT" NUMBER
   ) ;
