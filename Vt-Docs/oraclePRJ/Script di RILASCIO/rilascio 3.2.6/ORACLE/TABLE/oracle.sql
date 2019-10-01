CREATE TABLE DPA_REPORT_VERSAMENTO
(
ID_AMM					NUMBER,
VAR_FIXED_RECIPIENTS	VARCHAR2(2000),
CHA_ATTIVA_STRUTTURA	VARCHAR2(1),
CHA_MAIL_STRUTTURA		VARCHAR2(200),
MAIL_SUBJECT			VARCHAR2(100),
MAIL_BODY				VARCHAR2(2000),
VAR_SMTP_SERVER			VARCHAR2(128),
VAR_MAIL_FROM			VARCHAR2(128),
VAR_USERNAME_SMTP		VARCHAR2(128),
VAR_PASSWORD_SMTP		VARCHAR2(128),
VAR_PORT_SMTP			NUMBER,
CHA_SSL					CHAR(1)
)

ALTER TABLE DPA_EL_REGISTRI ADD
(
ID_UTENTE_RESP 			NUMBER
)

ALTER TABLE DPA_POLICY_PARER ADD 
(
CHA_ESCLUDI_FATTURE 	VARCHAR2(1)
)