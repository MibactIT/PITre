--------------------------------------------------------
--  DDL for Index INDX_UPPER_VAR_PROF_OGGETTO
--------------------------------------------------------

  CREATE INDEX "ITCOLL_6GIU12"."INDX_UPPER_VAR_PROF_OGGETTO" ON "ITCOLL_6GIU12"."PROFILE" (UPPER("VAR_PROF_OGGETTO")) 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 196608 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1 BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "PITRE_INFOTN_DATA_COLL" ;
