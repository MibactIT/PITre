--Tabella di Stato del Processo.
Begin
EXECUTE IMMEDIATE 
'CREATE TABLE CDC_ALERT_STATE
(
  DATESTART  VARCHAR2(20 BYTE)                  NOT NULL,
  STATE      NUMBER,
  DATEEND    VARCHAR2(20 BYTE)
)';

END;
/    