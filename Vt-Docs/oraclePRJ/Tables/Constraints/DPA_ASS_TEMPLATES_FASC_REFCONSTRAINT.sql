--------------------------------------------------------
--  Ref Constraints for Table DPA_ASS_TEMPLATES_FASC
--------------------------------------------------------

  ALTER TABLE "ITCOLL_6GIU12"."DPA_ASS_TEMPLATES_FASC" ADD CONSTRAINT "FK_TEMPLATE_FASC" FOREIGN KEY ("ID_TEMPLATE")
	  REFERENCES "ITCOLL_6GIU12"."DPA_TIPO_FASC" ("SYSTEM_ID") ENABLE NOVALIDATE;
 
  ALTER TABLE "ITCOLL_6GIU12"."DPA_ASS_TEMPLATES_FASC" ADD FOREIGN KEY ("ID_OGGETTO")
	  REFERENCES "ITCOLL_6GIU12"."DPA_OGGETTI_CUSTOM_FASC" ("SYSTEM_ID") ENABLE;