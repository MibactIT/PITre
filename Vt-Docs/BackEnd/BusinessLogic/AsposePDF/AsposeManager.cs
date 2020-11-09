using Aspose.Pdf.Facades;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic.AsposePDF
{
    public class AsposeManager
    {
        private static ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static Aspose.Pdf.License _license = new Aspose.Pdf.License();
        private static bool _isLicensed = false;

        public static string RESERVED_SIGNATURE_FIELD_NAME = "ReservedAsposeSignatureField";

        /// <summary>
        /// Richaimare il seguente metodo prima di ogni operazione
        /// </summary>
        private static void _CheckLicense()
        {
            if (!_isLicensed)
            {
                _license.SetLicense("Aspose.Total.lic.xml");
                _isLicensed = true;
            }
        }


        public static byte[] ApponiSegnaturaPermanente(string[] value, byte[] fileContent, bool signed = false)
        {
            _logger.Info("START");
            byte[] _result = null;
            if(value == null || value.Length == 0) { return _result; }

            Aspose.Pdf.Document _documento;
            System.IO.MemoryStream _fileStream = null;

            try
            {
                AsposeManager._CheckLicense();
                _logger.DebugFormat("SegnaturaTest: '{0}'", value);
                _fileStream = new System.IO.MemoryStream(fileContent);
                _documento = new Aspose.Pdf.Document(_fileStream);

                if (signed)
                {
                    _result = _insertSignatureField(_documento, value);
                }
                else
                {
                    _result = _insertTextField(_documento, value);
                }

                _fileStream.Close();

                #region test 1
                //PdfFileSignature pdfSign = new PdfFileSignature();
                //pdfSign.BindPdf("C:\\NTTDATA\\pdf\\sample.pdf");
                //System.Drawing.Rectangle rect = new System.Drawing.Rectangle(100, 100, 100, 100);
                //Aspose.Pdf.Forms.PKCS1 signature = new Aspose.Pdf.Forms.PKCS1("C:\\NTTDATA\\certificati\\pdf\\root.pfx", "test");
                //pdfSign.Sign(1, "Signature Reason", "Contact", "Location", true, rect, signature);

                //pdfSign.Save("C:\\NTTDATA\\pdf\\sample_firmato_3.pdf");

                //return new byte[1];
                #endregion

                #region test 2
                //System.IO.MemoryStream _fileStream = new System.IO.MemoryStream(fileContent);

                //Aspose.Pdf.Document _documento = new Aspose.Pdf.Document(_fileStream);
                //FormEditor editor = new FormEditor(_documento);
                //long larghezza = (long)_documento.Pages[1].ArtBox.Width;
                //long altezza = (long)_documento.Pages[1].ArtBox.Height;
                //editor.AddField(FieldType.Signature, AsposeManager.RESERVED_SIGNATURE_FIELD_NAME, 1, 10, altezza - 40, larghezza - 10, altezza - 10);
                //// Aspose.Pdf.Forms.PKCS1 _signature = new Aspose.Pdf.Forms.PKCS1("C:\\NTTDATA\\certificati\\pdf\\CONVERTED.pfx", "test");

                //editor.Save(_tempStream);
                //Aspose.Pdf.Document _documentoFirmato = new Aspose.Pdf.Document(_tempStream);
                //PdfFileSignature _pdfSign = new PdfFileSignature(_documentoFirmato);

                //System.IO.Stream pfx = new System.IO.FileStream(@"C:\\NTTDATA\\certificati\\pdf\\root.pfx", System.IO.FileMode.Open);
                //Aspose.Pdf.Forms.PKCS7 _pcks = new Aspose.Pdf.Forms.PKCS7(pfx, "test");

                //_pdfSign.Sign(AsposeManager.RESERVED_SIGNATURE_FIELD_NAME, "Signature Reason", "John", "Kharkov", _pcks);
                #endregion


            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                throw ex;
            }
            finally
            {
                _fileStream?.Dispose();
                _logger.Info("END");
            }

            return _result;
        }

        public static System.Drawing.Bitmap TextToBitmap(string[] theText, int fontSize)
        {
            _logger.Info("START");
            System.Drawing.Font _drawFont = null;
            System.Drawing.SolidBrush _drawBrush = null;
            System.Drawing.Graphics _drawGraphics = null;
            System.Drawing.Bitmap _textBitmap = null;
            try
            {
                string _testoDaStampare = String.Join(" ", theText);


                // start with empty bitmap, get it's graphic's object
                // and choose a font
                _textBitmap = new System.Drawing.Bitmap(1, 1);
                _drawGraphics = System.Drawing.Graphics.FromImage(_textBitmap);
                _drawFont = new System.Drawing.Font("Arial", fontSize);


                // see how big the text will be
                int Width = (int)_drawGraphics.MeasureString(_testoDaStampare, _drawFont).Width;
                int Height = (int)_drawGraphics.MeasureString(_testoDaStampare, _drawFont).Height;


                // recreate the bitmap and graphic object with the new size
                _textBitmap = new System.Drawing.Bitmap(_textBitmap, Width, Height);
                _drawGraphics = System.Drawing.Graphics.FromImage(_textBitmap);


                // get the drawing brush and where we're going to draw
                _drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
                System.Drawing.PointF DrawPoint = new System.Drawing.PointF(0, 0);


                // clear the graphic white and draw the string
                _drawGraphics.Clear(System.Drawing.Color.White);
                _drawGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                _drawGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                _drawGraphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                _drawGraphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                _drawGraphics.DrawString(_testoDaStampare, _drawFont, _drawBrush, DrawPoint);

                // test
                _textBitmap.Save("C:\\NTTDATA\\pdf\\test.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                // fine test
                
            }
            catch(Exception ex)
            {
                _logger.Error(ex.Message, ex);
                throw ex;
            }
            finally
            {
                // don't dispose the bitmap, the caller needs it.
                _drawFont.Dispose();
                _drawBrush.Dispose();
                _drawGraphics.Dispose();
                _logger.Info("END");
            }
            return _textBitmap;
        }




        #region Private Method

        private static byte[] _insertSignatureField(Aspose.Pdf.Document documento, string[] text)
        {
            _logger.Info("START");

            byte[] _documentContent = null;

            string _certificatePath;
            string _certificatePassword;
            string _signatureReason;
            string _signatureContact;
            string _signatureLocation;

            bool _segnaturaVisibile = true;
            int _MARGINE_SINISTRO = 30;
            int _MARGINE_DESTRO = 30;
            int _MARGINE_SUPERIORE = 5;
            int _ALTEZZA_ETICHETTA = 30;
            PdfFileSignature _pdfSign = null;
            System.Drawing.Bitmap _bitmapText = null;
            Aspose.Pdf.Forms.PKCS1 _signature = null;
            System.Drawing.Rectangle _boxSegnatura;
            System.IO.MemoryStream _fsBitmapFinal = null;
            System.IO.MemoryStream _outputStream = null;

            try
            {
                _outputStream = new System.IO.MemoryStream();
                _pdfSign = new PdfFileSignature();
                _certificatePath = System.Configuration.ConfigurationManager.AppSettings["ASPOSE_SIGNATURE_FIELD_CERTIFICATE_PATH"];
                _certificatePassword = System.Configuration.ConfigurationManager.AppSettings["ASPOSE_SIGNATURE_FIELD_CERTIFICATE_PASSWORD"];
                _signatureReason = System.Configuration.ConfigurationManager.AppSettings["ASPOSE_SIGNATURE_FIELD__REASON"];
                _signatureContact = System.Configuration.ConfigurationManager.AppSettings["ASPOSE_SIGNATURE_FIELD__CONTACT"];
                _signatureLocation = System.Configuration.ConfigurationManager.AppSettings["ASPOSE_SIGNATURE_FIELD__LOCATION"];

                _pdfSign.BindPdf(documento);
                int larghezza = (int)_pdfSign.Document.Pages[1].ArtBox.Width;
                int altezza = (int)_pdfSign.Document.Pages[1].ArtBox.Height;
                _boxSegnatura = new System.Drawing.Rectangle(_MARGINE_SINISTRO, altezza - (_ALTEZZA_ETICHETTA + _MARGINE_SUPERIORE), larghezza - (_MARGINE_SINISTRO + _MARGINE_DESTRO), _ALTEZZA_ETICHETTA);
                _signature = new Aspose.Pdf.Forms.PKCS1(_certificatePath, _certificatePassword);
                _signature.ShowProperties = false;

                _fsBitmapFinal = new System.IO.MemoryStream();
                _bitmapText = TextToBitmap(text, 72); // font alto per migliorare la resa
                _bitmapText.Save(_fsBitmapFinal, System.Drawing.Imaging.ImageFormat.Jpeg);
                _pdfSign.SignatureAppearanceStream = _fsBitmapFinal;
                _logger.DebugFormat("bmp width: '{0}' larghezza: '{1}'", _bitmapText.Width, larghezza);
                _boxSegnatura.Width = _bitmapText.Width / 9 > larghezza ? larghezza : _bitmapText.Width / 9;
                _boxSegnatura.Height = _bitmapText.Height / 9;

                _pdfSign.Sign(1, _signatureReason, _signatureContact, _signatureLocation, _segnaturaVisibile, _boxSegnatura, _signature);
                _pdfSign.Save(_outputStream);

                _documentContent = _outputStream.ToArray();
                _outputStream.Close();
                _fsBitmapFinal.Close();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                _documentContent = null;
            }
            finally
            {
                _outputStream?.Dispose();
                _fsBitmapFinal?.Dispose();
            }

            _logger.Info("END");
            return _documentContent;
        }

        private static byte[] _insertTextField(Aspose.Pdf.Document documento, string[] text)
        {
            _logger.Info("START");
            byte[] _documentContent = null;
            int _MARGINE_SINISTRO = 30;
            int _MARGINE_DESTRO = 30;
            int _MARGINE_SUPERIORE = 5;
            int _ALTEZZA_ETICHETTA = 30;
            System.IO.MemoryStream _outputStream = null;

            try
            {
                string _testoDaStampare = String.Join(" ", text);

                _outputStream = new System.IO.MemoryStream();

                // Get first page
                Aspose.Pdf.Page pdfPage = (Aspose.Pdf.Page)documento.Pages[1];

                // Create text fragment
                int altezza = (int)pdfPage.ArtBox.Height;
                Aspose.Pdf.Text.TextFragment textFragment = new Aspose.Pdf.Text.TextFragment(_testoDaStampare);
                Aspose.Pdf.Rectangle _boxSegnatura = textFragment.Rectangle;
                _logger.Debug($"Dimensioni: {_boxSegnatura.Height} * { _boxSegnatura.Width }  -- Altezza: { altezza }");
                textFragment.Position = new Aspose.Pdf.Text.Position(_MARGINE_SINISTRO, altezza - _MARGINE_SUPERIORE - _boxSegnatura.Height);

                // Set text properties
                textFragment.TextState.FontSize = 14;
                textFragment.TextState.Font = Aspose.Pdf.Text.FontRepository.FindFont("TimesNewRoman");
                textFragment.TextState.BackgroundColor = Aspose.Pdf.Color.FromRgb(System.Drawing.Color.FromArgb(50, System.Drawing.Color.White));
                textFragment.TextState.ForegroundColor = Aspose.Pdf.Color.FromRgb(System.Drawing.Color.Red);

                // Create TextBuilder object
                Aspose.Pdf.Text.TextBuilder textBuilder = new Aspose.Pdf.Text.TextBuilder(pdfPage);

                // Append the text fragment to the PDF page
                textBuilder.AppendText(textFragment);

                documento.Save(_outputStream);
                _documentContent = _outputStream.ToArray();
                _outputStream.Close();
            }
            catch(Exception ex)
            {
                _logger.Error(ex.Message, ex);
                _documentContent = null;
            }
            finally
            {
                _outputStream?.Dispose();
            }


            _logger.Info("END");
            return _documentContent;
        }

        #endregion
    }
}
