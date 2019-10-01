create or replace 
FUNCTION getPolicyVersamentoDataExec(IDDOC VARCHAR)
RETURN VARCHAR IS
	DATA_ESECUZIONE VARCHAR(200);
  item VARCHAR(100);
  
  CURSOR cur IS
  SELECT TO_CHAR(V.DATA_ESECUZIONE_POLICY, 'DD/MM/YYYY')
		FROM DPA_POLICY_PARER P, DPA_VERSAMENTI_POLICY V
		WHERE P.SYSTEM_ID=V.ID_POLICY AND V.ID_PROFILE=IDDOC
    ORDER BY V.DATA_ESECUZIONE_POLICY
    ;
BEGIN
DATA_ESECUZIONE := NULL;

OPEN cur;
LOOP
FETCH cur INTO item;
EXIT WHEN cur%NOTFOUND;

IF(DATA_ESECUZIONE=NULL) THEN
DATA_ESECUZIONE := item;
ELSE
DATA_ESECUZIONE := DATA_ESECUZIONE || item || '; ';
END IF;

END LOOP;
  
RETURN SUBSTR(DATA_ESECUZIONE,1,LENGTH(DATA_ESECUZIONE)-2);
END getPolicyVersamentoDataExec;