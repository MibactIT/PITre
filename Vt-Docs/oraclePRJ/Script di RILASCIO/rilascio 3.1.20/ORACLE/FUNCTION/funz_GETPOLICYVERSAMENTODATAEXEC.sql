CREATE OR REPLACE FUNCTION getPolicyVersamentoDataExec(IDDOC VARCHAR)
RETURN VARCHAR IS
	DATA_ESECUZIONE VARCHAR(200);
BEGIN

	BEGIN
		SELECT TO_CHAR(V.DATA_ESECUZIONE_POLICY, 'DD/MM/YYYY') INTO DATA_ESECUZIONE
		FROM DPA_POLICY_PARER P, DPA_VERSAMENTI_POLICY V
		WHERE P.SYSTEM_ID=V.ID_POLICY AND V.ID_PROFILE=IDDOC;
	EXCEPTION
		WHEN NO_DATA_FOUND THEN DATA_ESECUZIONE:=NULL;
	END;

RETURN DATA_ESECUZIONE;

END getPolicyVersamentoDataExec;