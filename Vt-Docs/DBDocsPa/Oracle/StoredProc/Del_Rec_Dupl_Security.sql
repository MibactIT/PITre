CREATE OR REPLACE PROCEDURE @db_user.Del_Rec_Dupl_Security (
retValue OUT NUMBER) IS

fc_THING NUMBER;
fc_PERSONORGROUP NUMBER;
fc_ACCESSRIGHTS NUMBER;
fc_COUNT NUMBER;

f_THING NUMBER;
f_PERSONORGROUP NUMBER;
f_ACCESSRIGHTS NUMBER;
f_ID_GRUPPO_TRASM NUMBER;
f_CHA_TIPO_DIRITTO CHAR(1);

ok_select NUMBER;
ok_delete NUMBER;
ok_insert NUMBER;

CURSOR cursor_A IS
SELECT /*+ index(SECURITY) */ DISTINCT
THING,ACCESSRIGHTS,PERSONORGROUP,COUNT(*)
FROM SECURITY
GROUP BY THING,ACCESSRIGHTS,PERSONORGROUP
HAVING COUNT(*) > 1;

BEGIN
retValue := 0;

OPEN cursor_A;
LOOP
FETCH cursor_A INTO fc_THING,fc_ACCESSRIGHTS,fc_PERSONORGROUP,fc_COUNT;
EXIT WHEN cursor_A%NOTFOUND;

ok_select := 1;
ok_delete := 1;
ok_insert := 1;

BEGIN
-- MEMORIZZA IL RECORD CON ACCESSRIGHTS MAGGIORE
SELECT
THING, PERSONORGROUP, ACCESSRIGHTS, ID_GRUPPO_TRASM, CHA_TIPO_DIRITTO
INTO
f_THING,f_PERSONORGROUP,f_ACCESSRIGHTS,f_ID_GRUPPO_TRASM,f_CHA_TIPO_DIRITTO
FROM (
SELECT *
FROM SECURITY
WHERE thing = fc_THING
AND PERSONORGROUP = fc_PERSONORGROUP
AND ACCESSRIGHTS = fc_ACCESSRIGHTS
ORDER BY ACCESSRIGHTS DESC, CHA_TIPO_DIRITTO DESC
)
WHERE ROWNUM = 1;
EXCEPTION
WHEN OTHERS THEN
ok_select := 0;
END;

IF ok_select = 1 THEN
BEGIN
-- ELIMINA TUTTI I RECORD
DELETE FROM SECURITY
WHERE THING = fc_THING AND PERSONORGROUP = fc_PERSONORGROUP;
EXCEPTION
WHEN OTHERS THEN
ok_delete := 0;
END;

IF ok_delete = 1 THEN
BEGIN
-- QUINDI INSERISCE IL RECORD MEMORIZZATO PRIMA
INSERT  INTO SECURITY
(THING, PERSONORGROUP, ACCESSRIGHTS, ID_GRUPPO_TRASM, CHA_TIPO_DIRITTO)
VALUES
(f_THING,f_PERSONORGROUP,f_ACCESSRIGHTS,f_ID_GRUPPO_TRASM,f_CHA_TIPO_DIRITTO);
EXCEPTION
WHEN OTHERS THEN
ok_insert := 0;
END;
END IF;

IF ok_select = 1 AND ok_delete = 1 AND ok_insert = 1 THEN
COMMIT;
ELSE
ROLLBACK;
retValue := 1;
RETURN;
END IF;

END IF;

END LOOP;
CLOSE cursor_A;

END;
/