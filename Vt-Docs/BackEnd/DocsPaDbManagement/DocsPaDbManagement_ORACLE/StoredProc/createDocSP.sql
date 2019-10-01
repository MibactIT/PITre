CREATE OR REPLACE procedure createDocSP(p_idpeople number,p_doctype VARCHAR, p_systemId OUT number)
is

BEGIN

DECLARE
docnum  number;
verid number;
idDocType number;

BEGIN

p_systemId:=0;
 
<<REPERIMENTO_DOCUMENTTYPES>>
BEGIN

	 SELECT SYSTEM_ID into idDocType FROM
	 DOCUMENTTYPES
	 WHERE TYPE_ID = p_doctype;
	 	   EXCEPTION
	 	   			WHEN NO_DATA_FOUND THEN
			  	   		 p_systemId:=0;
				   		 RETURN;
				   
END REPERIMENTO_DOCUMENTTYPES;

SELECT SEQ.NEXTVAL INTO docnum FROM DUAL; 

p_systemId:= docnum;

<<INSERIMENTO_IN_PROFILE>>
BEGIN
	INSERT INTO Profile 
			(
				SYSTEM_ID,  
				TYPIST,  
				AUTHOR,
				DOCUMENTTYPE, 
				CREATION_DATE, 
				CREATION_TIME,
				DOCNUMBER
				
			) 
			VALUES 
			(
				docnum, 
				p_idpeople, 
				p_idpeople, 
				idDocType,
				 SYSDATE,
				 SYSDATE,
				docnum
				
			);
			EXCEPTION
	 				 WHEN OTHERS THEN  p_systemId:=0;
				   	 RETURN;
						  
	END INSERIMENTO_IN_PROFILE;	
		
	<<INSERIMENTO_IN_VERSIONS>>
	BEGIN
	
	SELECT SEQ.NEXTVAL INTO verid FROM DUAL; 
			
			INSERT INTO VERSIONS 
			(
				VERSION_ID,  DOCNUMBER, VERSION, SUBVERSION, VERSION_LABEL, AUTHOR, TYPIST, DTA_CREAZIONE
			) VALUES (
				verid, docnum, 1, '!', '1',p_idpeople, p_idpeople,  SYSDATE
			);
			
			EXCEPTION
	 				 WHEN OTHERS THEN p_systemId:=0;  
				   	 RETURN;
						  
	END INSERIMENTO_IN_VERSIONS;
			
	<<INSERIMENTO_IN_COMPONENTS>>	
	BEGIN
	
	INSERT INTO COMPONENTS 
			(
			VERSION_ID, DOCNUMBER, FILE_SIZE
			) VALUES (
				verid, docnum, 0
			)	;
			EXCEPTION		
					 WHEN OTHERS THEN p_systemId:=0;
					 RETURN;
					 
	END INSERIMENTO_IN_COMPONENTS;
	
	<<INSERIMENTO_SECURITY>>
	BEGIN 	
			INSERT INTO SECURITY 
			(
				THING,
				PERSONORGROUP,
				ACCESSRIGHTS,
				ID_GRUPPO_TRASM,
				CHA_TIPO_DIRITTO
			) VALUES (
				docnum, p_idpeople, 0, NULL, NULL			
			)	;
	 		EXCEPTION		
					 WHEN OTHERS THEN p_systemId:=0;
					 return;
	END INSERIMENTO_SECURITY;

END;

END;
/
