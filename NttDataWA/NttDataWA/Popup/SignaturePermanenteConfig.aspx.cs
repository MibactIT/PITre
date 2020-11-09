using NttDataWA.DocsPaWR;
using NttDataWA.UIManager;
using NttDataWA.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NttDataWA.Popup
{
    public partial class SignaturePermanenteConfig : System.Web.UI.Page
    {
        private readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static DocsPaWR.DocsPaWebService _docsPaWS = ProxyManager.GetWS();

        private FileRequest _fileRequest;
        private DettaglioSegnaturaPosition _dettaglioSegnatura;
        private InfoUtente _infoUtente;
        PdfPageInfo _pageInfo;

        #region Proprieta'
        private FileDocumento _fileDocumento
        {
            get
            {
                if (HttpContext.Current.Session["fileDoc"] != null)
                    return HttpContext.Current.Session["fileDoc"] as FileDocumento;
                else return null;
            }
            set
            {
                HttpContext.Current.Session["fileDoc"] = value;
            }
        }

        /// <summary>
        /// Indentifica se siamo nella situazione di apertura della popup(inserita per evitare che alla chiusura della popup riesegua il tutto il page_load)
        /// </summary>
        private bool OpenSignaturePopup
        {
            get
            {
                if (HttpContext.Current.Session["OpenSignaturePopup"] != null)
                    return (bool)HttpContext.Current.Session["OpenSignaturePopup"];
                else return false;
            }
            set
            {
                HttpContext.Current.Session["OpenSignaturePopup"] = value;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this._logger?.Info("START");
            try
            {
                this._fileRequest = FileManager.GetFileRequest();
                this._initDettagliSegnaturaPermanente();

                if (!this.Page.IsPostBack && OpenSignaturePopup)
                {
                    this._initPage();
                    this._initLabels();
                }

            }
            catch (Exception ex)
            {
                this._logger?.Error(ex.Message, ex);
            }
            this._logger?.Info("END");
        }



        private void _initPage()
        {
            this._logger?.Info("START");
            string _position = string.Empty;

            try
            {
                this._infoUtente = UserManager.GetInfoUser();
                this._pageInfo = _docsPaWS.PdfService_GetPageInfo(this._fileRequest.docNumber, this._infoUtente);

                if (_dettaglioSegnatura == null)
                    _position = "TOP_L";
                else
                    _position = (string.IsNullOrEmpty(_dettaglioSegnatura.SegnaturaPosition) ? "TOP_L" : _dettaglioSegnatura.SegnaturaPosition);

                this.segnaturaPermanentePosition.Value = _position;

                string _function = $"SetDocumentPreviewDimension({this._pageInfo.Rectangle.Height.ToString("F0")},{this._pageInfo.Rectangle.Width.ToString("F0")},'{_position}')";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fixPageDimension", _function, true);


            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fixPageDimension", "alert('Errore nell''elaborazione dell'anteprima');", true);
                this._logger?.Error(ex.Message, ex);
            }
            this._logger?.Info("END");
        }

        private void _initDettagliSegnaturaPermanente()
        {
            this._logger?.Info("START");
            try
            {
                this._dettaglioSegnatura = DocumentManager.GetDettaglioSegnaturaPosition(this._fileRequest.docNumber);
            }
            catch (Exception ex)
            {
                this._logger?.Error(ex.Message, ex);
                throw ex;
            }
            this._logger?.Info("END");
        }

        private void _initLabels()
        {
            string _language = UIManager.UserManager.GetUserLanguage();
            this.SignaturePermanentBtnConfirm.Text = Utils.Languages.GetLabelFromCode("SignatureBtnConfirm", _language);
            this.SignaturePermanentBtnClose.Text = Utils.Languages.GetLabelFromCode("SignatureBtnClose", _language);
        }



        protected void SignaturePermanentBtnConfirm_Click(object sender, EventArgs e)
        {
            _logger.Info("START");
            try
            {
                var _position = this.segnaturaPermanentePosition.Value;
                // _logger.Warn($"POSITION: {_position}");
                if (String.IsNullOrWhiteSpace(_position)) { throw new Exception("Errore nel settaggio della posizione della segnatura"); }
                
                bool _result = false;
                _logger.Debug($"Posizione: '{_position}'");
                if(this._dettaglioSegnatura == null)
                {
                    this._dettaglioSegnatura = new DettaglioSegnaturaPosition()
                    {
                        ProfileID = this._fileRequest.docNumber,
                        SegnaturaPosition = _position
                    };
                    _result = _docsPaWS.DettaglioSegnaturaPosition_Insert(this._dettaglioSegnatura);
                }
                else
                {
                    this._dettaglioSegnatura.SegnaturaPosition = _position;
                    _result = _docsPaWS.DettaglioSegnaturaPosition_Update(this._dettaglioSegnatura);
                }

                
                if (!_result) { throw new Exception("Errore update posizione segnatura"); }

                HttpContext.Current.Session.Remove("OpenSignaturePopup");
            }
            catch(Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "test confirm", "alert('Errore nel salvataggio delle informazioni');", true);
                _logger.Error(ex.Message, ex);
            }
            _logger.Info("END");
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "test cornfirm", "alert('ok test confirm');", true);
            Response.Write("<html><body><script type=\"text/javascript\">parent.closeAjaxModal('SignaturePermanenteConfig','ritorna ok');</script></body></html>");
            Response.End();
        }

        protected void SignaturePermanentBtnClose_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session.Remove("OpenSignaturePopup");
            Response.Write("<html><body><script type=\"text/javascript\">parent.closeAjaxModal('SignaturePermanenteConfig','ritorna ok');</script></body></html>");
            Response.End();
        }
    }
}