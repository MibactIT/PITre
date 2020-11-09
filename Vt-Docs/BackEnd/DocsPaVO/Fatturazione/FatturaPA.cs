﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace DocsPaVO.Fatturazione
{
    [Serializable()]
    public class FatturaPA
    {
        public string templateXML;

        // Campi HEADER fattura
        #region DatiTrasmissione

        public string trasmittenteIdPaese;
        public string trasmittenteIdCodice;
        public string trasmittenteTelefono;
        public string trasmittenteMail;
        public string formatoTrasmissione;

        #endregion

        public CedentePrestatore cedente;
        public CessionarioCommittente cessionario;

        // Campi BODY fattura

        // DatiGeneraliDocumento
        public string tipoDoc;
        public string divisa;
        public DateTime dataDoc;
        public string numeroFattura;
        public string importoTotaleDoc;

        public string idOrdineAcquisto;
        public string CUPOrdineAcquisto;
        public string CIGOrdineAcquisto;

        public string idContratto;
        public string CUPContratto;
        public string CIGContratto;

        public string codiceIPA;

        // DatiBeniServizi
        [XmlArray()]
        [XmlArrayItem(typeof(DocsPaVO.Fatturazione.DatiBeniServizi))]
        public ArrayList servizi = new ArrayList();

        // DatiRiepilogo
        public string aliquotaIVA;
        public string imponibileImporto;
        public string imposta;
        public string esigibilitaIVA;

        // DatiPagamento
        public string pagamentoCondizioni;
        public string pagamentoModalita;
        public string pagamentoImporto;
        public DateTime dataRifTerminiPagamento;
        public string giorniTerminiPagamento;
        public string istitutoFinanziario;
        public string IBAN;
        public string BIC;

    }
}