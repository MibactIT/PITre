CREATE TABLE DPA_PREVIEW
   (	DOC_NUMBER VARCHAR2(100 BYTE) NOT NULL ENABLE, 
	VERSION_ID VARCHAR2(100 BYTE) NOT NULL ENABLE, 
	TOTAL_PAGES NUMBER NOT NULL ENABLE, 
	PAGE_TO NUMBER NOT NULL ENABLE, 
	PAGE_FROM NUMBER NOT NULL ENABLE, 
	FILE_HASH VARCHAR2(2000 BYTE), 
	PATH VARCHAR2(2000 BYTE)
 );