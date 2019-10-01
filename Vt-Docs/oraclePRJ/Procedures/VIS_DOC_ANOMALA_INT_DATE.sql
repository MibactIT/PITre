--------------------------------------------------------
--  DDL for Procedure VIS_DOC_ANOMALA_INT_DATE
--------------------------------------------------------
set define off;

  CREATE OR REPLACE PROCEDURE "ITCOLL_6GIU12"."VIS_DOC_ANOMALA_INT_DATE" (p_id_amm NUMBER, p_start_date VARCHAR, p_end_date VARCHAR) IS

--DICHIARAZIONI
s_idg_security NUMBER;
s_ar_security NUMBER;
s_td_security VARCHAR(2);
s_vn_security VARCHAR(255);
s_idg_r_sup NUMBER;
s_doc_number NUMBER;
s_id_fascicolo NUMBER;
s_cha_cod_t_a_fasc VARCHAR(1024);
n_id_gruppo NUMBER;
codice_atipicita VARCHAR(255);

BEGIN

--CURSORE DOCUMENTI
DECLARE CURSOR documenti IS select profile.docnumber from profile, people where creation_time between TO_DATE(p_start_date, 'dd/mm/yyyy hh24.mi.ss') and TO_DATE(p_end_date, 'dd/mm/yyyy hh24.mi.ss') AND profile.author = people.system_id AND people.id_amm = p_id_amm;
   
BEGIN OPEN documenti;
LOOP FETCH documenti INTO s_doc_number;
EXIT WHEN documenti%NOTFOUND;

    --Cursore sulla security per lo specifico documento
    DECLARE CURSOR c_idg_security IS 
    SELECT personorgroup, accessrights, cha_tipo_diritto, var_note_sec 
    FROM security 
    WHERE 
    thing = s_doc_number
    AND
    accessrights > 20;  
    BEGIN OPEN c_idg_security;
    LOOP FETCH c_idg_security INTO s_idg_security, s_ar_security, s_td_security, s_vn_security;
    EXIT WHEN c_idg_security%NOTFOUND;

        --Gerachia ruolo proprietario documento
        IF(upper(s_td_security) = 'P') THEN
            DECLARE CURSOR ruoli_sup IS 
            SELECT dpa_corr_globali.id_gruppo 
            FROM dpa_corr_globali INNER JOIN dpa_tipo_ruolo ON dpa_corr_globali.id_tipo_ruolo = dpa_tipo_ruolo.system_id 
            WHERE
                dpa_corr_globali.id_uo in (
                    SELECT dpa_corr_globali.system_id
                    FROM dpa_corr_globali
                    WHERE 
                    dpa_corr_globali.dta_fine IS NULL
                    CONNECT BY PRIOR dpa_corr_globali.id_parent = dpa_corr_globali.system_id START WITH dpa_corr_globali.system_id = (SELECT dpa_corr_globali.id_uo FROM dpa_corr_globali WHERE dpa_corr_globali.id_gruppo = s_idg_security)
                    )
            AND
            dpa_corr_globali.CHA_TIPO_URP = 'R'
            AND
            dpa_corr_globali.ID_AMM = p_id_amm
            AND
            dpa_corr_globali.DTA_FINE IS NULL
            AND 
            dpa_tipo_ruolo.num_livello < (SELECT dpa_tipo_ruolo.num_livello FROM dpa_corr_globali INNER JOIN dpa_tipo_ruolo ON dpa_corr_globali.id_tipo_ruolo = dpa_tipo_ruolo.system_id WHERE dpa_corr_globali.id_gruppo = s_idg_security);
        
            BEGIN OPEN ruoli_sup;
            LOOP FETCH ruoli_sup INTO s_idg_r_sup;
            EXIT WHEN ruoli_sup%NOTFOUND;
                --DBMS_OUTPUT.PUT_LINE('DOCUMENTO : ' || s_doc_number || ' Ruolo gerarchicamente superiore a ruolo proprietario: ' || s_idg_r_sup);
                INSERT INTO DPA_VIS_ANOMALA (ID_GRUPPO) VALUES(s_idg_r_sup);
            END LOOP;
            CLOSE ruoli_sup;
            END;
            --Si effettua una sottrazione fra l'insieme degli id_gruppo ricostruiti e quelli presenti in security
            --Se si ottiene un insieme vuoto, vuol dire che la visibilit non ha anomalie
            BEGIN
                n_id_gruppo := 0;
                SELECT COUNT(*) INTO n_id_gruppo FROM
                (
                SELECT ID_GRUPPO FROM DPA_VIS_ANOMALA
                MINUS
                SELECT PERSONORGROUP FROM SECURITY WHERE THING = s_doc_number
                );
                IF(n_id_gruppo <> 0 AND nvl(instr(codice_atipicita,'AGRP'), 0) = 0) THEN
                    codice_atipicita := codice_atipicita || 'AGRP-';
                END IF;
            END;
            COMMIT; 
        END IF;
        
        --Gerarchia destinatario trasmissione
        IF(upper(s_td_security) = 'T') THEN
            DECLARE CURSOR ruoli_sup IS
            SELECT dpa_corr_globali.id_gruppo 
            FROM dpa_corr_globali INNER JOIN dpa_tipo_ruolo ON dpa_corr_globali.id_tipo_ruolo = dpa_tipo_ruolo.system_id 
            WHERE
                dpa_corr_globali.id_uo in (
                    SELECT dpa_corr_globali.system_id
                    FROM dpa_corr_globali
                    WHERE 
                    dpa_corr_globali.dta_fine IS NULL
                    CONNECT BY PRIOR dpa_corr_globali.id_parent = dpa_corr_globali.system_id START WITH dpa_corr_globali.system_id = (SELECT dpa_corr_globali.id_uo FROM dpa_corr_globali WHERE dpa_corr_globali.id_gruppo = s_idg_security)
                    )
            AND
            dpa_corr_globali.CHA_TIPO_URP = 'R'
            AND
            dpa_corr_globali.ID_AMM = p_id_amm
            AND
            dpa_corr_globali.DTA_FINE IS NULL
            AND 
            dpa_tipo_ruolo.num_livello < (SELECT dpa_tipo_ruolo.num_livello FROM dpa_corr_globali INNER JOIN dpa_tipo_ruolo ON dpa_corr_globali.id_tipo_ruolo = dpa_tipo_ruolo.system_id WHERE dpa_corr_globali.id_gruppo = s_idg_security);
                        
            BEGIN OPEN ruoli_sup;
            LOOP FETCH ruoli_sup INTO s_idg_r_sup;
            EXIT WHEN ruoli_sup%NOTFOUND;                   
                --DBMS_OUTPUT.PUT_LINE('TRASMISSIONE DOCUMENTO : ' || s_doc_number || ' Ruolo Gerarchicamente superiore a ruolo destinatario della trasmissione :' || s_idg_r_sup);
                INSERT INTO DPA_VIS_ANOMALA (ID_GRUPPO) VALUES(s_idg_r_sup);          
            END LOOP;
            CLOSE ruoli_sup;
            END;
            --Si effettua una sottrazione fra l'insieme degli id_gruppo ricostruiti e quelli presenti in security
            --Se si ottiene un insieme vuoto, vuol dire che la visibilit non ha anomalie
            BEGIN
                n_id_gruppo := 0;
                SELECT COUNT(*) INTO n_id_gruppo FROM
                (
                SELECT ID_GRUPPO FROM DPA_VIS_ANOMALA
                MINUS
                SELECT PERSONORGROUP FROM SECURITY WHERE THING = s_doc_number
                );
                IF(n_id_gruppo <> 0 AND nvl(instr(codice_atipicita, 'AGDT'), 0) = 0) THEN
                    codice_atipicita := codice_atipicita || 'AGDT-';
                END IF;
            END;
            COMMIT; 
        END IF;
        
        --Fascicolazione documento
        IF(upper(s_td_security) = 'F') THEN
            DECLARE CURSOR fascicoli IS 
            select system_id from project where system_id in(
                select id_fascicolo from project where system_id in (
                    select project_id from project_components where link = s_doc_number
                    )
            ) and cha_tipo_fascicolo = 'P';
            BEGIN OPEN fascicoli;
            LOOP FETCH fascicoli INTO s_id_fascicolo;
            EXIT WHEN fascicoli%NOTFOUND;
                SELECT cha_cod_t_a INTO s_cha_cod_t_a_fasc FROM project WHERE system_id = s_id_fascicolo;
                IF(s_cha_cod_t_a_fasc is not null AND upper(s_cha_cod_t_a_fasc) <> 'T') THEN
                    IF(nvl(instr(codice_atipicita, 'AFCD'), 0) = 0) THEN
                        codice_atipicita := codice_atipicita || 'AFCD-';
                    END IF;    
                END IF;
            END LOOP;
            CLOSE fascicoli;
            END;
        END IF;
        
        --Gerarchia ruolo destinatario di copia visibilit
        IF(upper(s_td_security) = 'A' AND upper(s_vn_security) = 'ACQUISITO PER COPIA VISIBILITA') THEN
            DECLARE CURSOR ruoli_sup IS 
            SELECT dpa_corr_globali.id_gruppo 
            FROM dpa_corr_globali INNER JOIN dpa_tipo_ruolo ON dpa_corr_globali.id_tipo_ruolo = dpa_tipo_ruolo.system_id 
            WHERE
                dpa_corr_globali.id_uo in (
                    SELECT dpa_corr_globali.system_id
                    FROM dpa_corr_globali
                    WHERE 
                    dpa_corr_globali.dta_fine IS NULL
                    CONNECT BY PRIOR dpa_corr_globali.id_parent = dpa_corr_globali.system_id START WITH dpa_corr_globali.system_id = (SELECT dpa_corr_globali.id_uo FROM dpa_corr_globali WHERE dpa_corr_globali.id_gruppo = s_idg_security)
                    )
            AND
            dpa_corr_globali.CHA_TIPO_URP = 'R'
            AND
            dpa_corr_globali.ID_AMM = p_id_amm
            AND
            dpa_corr_globali.DTA_FINE IS NULL
            AND 
            dpa_tipo_ruolo.num_livello < (SELECT dpa_tipo_ruolo.num_livello FROM dpa_corr_globali INNER JOIN dpa_tipo_ruolo ON dpa_corr_globali.id_tipo_ruolo = dpa_tipo_ruolo.system_id WHERE dpa_corr_globali.id_gruppo = s_idg_security);
        
            BEGIN OPEN ruoli_sup;
            LOOP FETCH ruoli_sup INTO s_idg_r_sup;
            EXIT WHEN ruoli_sup%NOTFOUND;
                --DBMS_OUTPUT.PUT_LINE('TRASMISSIONE DOCUMENTO : ' || s_doc_number || ' Ruolo Gerarchicamente superiore a ruolo destinatario della trasmissione :' || s_idg_r_sup);
                INSERT INTO DPA_VIS_ANOMALA (ID_GRUPPO) VALUES(s_idg_r_sup);   
            END LOOP;
            CLOSE ruoli_sup;
            END;
            --Si effettua una sottrazione fra l'insieme degli id_gruppo ricostruiti e quelli presenti in security
            --Se si ottiene un insieme vuoto, vuol dire che la visibilit non ha anomalie
            BEGIN
                n_id_gruppo := 0;
                SELECT COUNT(*) INTO n_id_gruppo FROM
                (
                SELECT ID_GRUPPO FROM DPA_VIS_ANOMALA
                MINUS
                SELECT PERSONORGROUP FROM SECURITY WHERE THING = s_doc_number
                );
                IF(n_id_gruppo <> 0 AND nvl(instr(codice_atipicita, 'AGCV'), 0) = 0) THEN
                    codice_atipicita := codice_atipicita || 'AGCV-';
                END IF;
            END;
            COMMIT;  
        END IF;    

    END LOOP;
    CLOSE c_idg_security;
    END;  

    --Restituzione codice di atipicit
    IF(codice_atipicita is null) THEN
        codice_atipicita := 'T';
        --DBMS_OUTPUT.PUT_LINE('Codici Atipicit Documento ' || s_doc_number || ' - ' || codice_atipicita);
        update PROFILE set CHA_COD_T_A = codice_atipicita where DOCNUMBER = s_doc_number;
        COMMIT;
        codice_atipicita := null;        
    END IF;

    IF(substr(codice_atipicita, length(codice_atipicita)) = '-') THEN
        codice_atipicita := substr(codice_atipicita, 0, length(codice_atipicita)-1);
        --DBMS_OUTPUT.PUT_LINE('Codici Atipicit Documento ' || s_doc_number || ' - ' || codice_atipicita);
        update PROFILE set CHA_COD_T_A = codice_atipicita where DOCNUMBER = s_doc_number;
        COMMIT;
        codice_atipicita := null;        
    END IF;

END LOOP;
CLOSE documenti;
END;
    
EXCEPTION 
WHEN others THEN
DBMS_OUTPUT.PUT_LINE('Errore nell''esecuzione della procedura');

END; 

/
