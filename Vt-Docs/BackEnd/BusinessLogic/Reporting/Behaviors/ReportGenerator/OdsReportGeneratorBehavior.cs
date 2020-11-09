﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenDocument;
using DocsPaVO.Report;
using DocsPaVO.documento;
using System.IO;
using AODL.Document.SpreadsheetDocuments;
using AODL.Document.Content.Tables;
using AODL.Document.Content.Text;
using AODL.Document;
using AODL.Document.Styles;

namespace BusinessLogic.Reporting.Behaviors.ReportGenerator
{
    /// <summary>
    /// Behavior per l'implementazione di un renderizzatore di file ODS (Fogli di calcolo
    /// Open Office)
    /// </summary>
    public class OdsReportGeneratorBehavior : IReportGeneratorBehavior
    {
        /// <summary>
        /// Metodo per la generazione di un report ODS
        /// </summary>
        /// <param name="request">Informazioni sul report da produrre</param>
        /// <param name="reports">Report da esportare</param>
        /// <returns>Foglio di calcolo Open Office</returns>
        public FileDocumento GenerateReport(PrintReportRequest request, List<DocsPaVO.Report.Report> reports)
        {
            // Instanziazione del writer ODS e creazione del documento
            SpreadsheetDocument spreadSheetDocument = new SpreadsheetDocument();
            spreadSheetDocument.New();

            // Generazione di un foglio per ogni report
            foreach (var report in reports)
                // Generazione di un foglio con i dati contenuti nel report
                this.AddWorksheet(spreadSheetDocument, report);

            // Inizializzazione del file name
            String fileName = String.Format("Report_{0}.ods", DateTime.Now.ToString("dd-MM-yyyy"));

            // Inizializzazione del path del file
            String filePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.InternetCache),
                String.Format(@"AODF\{0}", Guid.NewGuid()));

            // Creazione della cartella
            DirectoryInfo dirInfo = Directory.CreateDirectory(filePath);

            // Salvataggio del file Open Document
            spreadSheetDocument.SaveTo(Path.Combine(filePath, fileName));

            // Generazione del risultato dell'export
            FileDocumento document = new FileDocumento();
            using (MemoryStream stream = new MemoryStream(File.ReadAllBytes(Path.Combine(filePath, fileName))))
            {
                document.name = fileName;
                document.path = String.Empty;
                document.fullName = document.name;
                document.contentType = "application/vnd.oasis.opendocument.spreadsheet";
                document.content = new Byte[stream.Length];

                stream.Read(document.content, 0, document.content.Length);

                stream.Flush();
                stream.Close();
            }

            // Si provano a cancellare File e cartella
            this.DeleteTemporaryData(dirInfo);

            return document;
        }

        /// <summary>
        /// Metodo per l'aggiunta di un foglio per un dato report
        /// </summary>
        /// <param name="spreadsheetDocument">Documento Open Office</param>
        /// <param name="report">Report da renderizzare</param>
        private void AddWorksheet(SpreadsheetDocument spreadsheetDocument, DocsPaVO.Report.Report report)
        {
            // Aggiunta di un foglio al report
            Table sheet = new Table(spreadsheetDocument, String.Format("Foglio {0}", spreadsheetDocument.TableCount), String.Empty);
            spreadsheetDocument.TableCollection.Add(sheet);

            // Aggiunta del titolo e del sottotitolo
            this.SetTitle(report.Title, sheet);
            this.SetSubtitle(report.Subtitle, sheet);

            // Aggiunta dell'header
            this.AddHeaderRow(report.ReportHeader, sheet);

            // Aggiunta dei dati al report
            this.AddReportData(sheet, report.ReportMapRow);

        }

        /// <summary>
        /// Metodo per l'impostazione del titolo di un report
        /// </summary>
        /// <param name="title">Titolo da assegnare al foglio</param>
        /// <param name="sheet">Foglio a cui aggiungere il titolo</param>
        private void SetTitle(String title, Table sheet)
        {
            Cell titleCell = new Cell(sheet.Document, "cell001");
            titleCell.OfficeValueType = "string";
            Paragraph titleParagraph = ParagraphBuilder.CreateSpreadsheetParagraph(sheet.Document);
            FormatedText fText = new FormatedText(sheet.Document, "T1", title);
            fText.TextStyle.TextProperties.Bold = "bold";
            fText.TextStyle.TextProperties.FontSize = "20pt";
            titleParagraph.TextContent.Add(fText);
            titleCell.Content.Add(titleParagraph);
            sheet.Rows.Add(new Row(sheet));
            sheet.Rows[0].Cells.Add(titleCell);
        }

        /// <summary>
        /// Metodo per l'impostazione del sotto titolo di un report
        /// </summary>
        /// <param name="subtitle">Sottotitolo da assegnare al foglio</param>
        /// <param name="sheet">Foglio a cui aggiungere il sottotitolo</param>
        private void SetSubtitle(String subtitle, Table sheet)
        {
            Cell subTitleCell = new Cell(sheet.Document, "cell002");
            subTitleCell.OfficeValueType = "string";
            Paragraph subTitleParagraph = ParagraphBuilder.CreateSpreadsheetParagraph(sheet.Document);
            FormatedText fText = new FormatedText(sheet.Document, "T2", subtitle);
            fText.TextStyle.TextProperties.Bold = "bold";
            fText.TextStyle.TextProperties.FontSize = "15pt";
            subTitleParagraph.TextContent.Add(fText);
            subTitleCell.Content.Add(subTitleParagraph);
            sheet.Rows.Add(new Row(sheet));
            sheet.Rows[1].Cells.Add(subTitleCell);

        }

        /// <summary>
        /// Metodo per l'aggiunta dell'header ad un foglio Excel
        /// </summary>
        /// <param name="header">Header da aggiungere</param>
        /// <param name="sheet">Foglio a cui aggiungere l'instestazione</param>
        private void AddHeaderRow(DocsPaVO.Report.HeaderColumnCollection header, Table sheet)
        {
            sheet.Rows.Add(new Row(sheet));
            int actualRow = sheet.Rows.Count - 1;
            foreach (var headerColumn in header)
            {
                Cell headerCell = new Cell(sheet.Document, "cell003");
                headerCell.OfficeValueType = "string";
                headerCell.CellStyle.CellProperties.Border = Border.HeavySolid;
                headerCell.CellStyle.CellProperties.BackgroundColor = "#D0B1A1";
                Paragraph paragraph = ParagraphBuilder.CreateSpreadsheetParagraph(sheet.Document);
                FormatedText fText = new FormatedText(sheet.Document, "T3", headerColumn.ColumnName);
                fText.TextStyle.TextProperties.Bold = "bold";
                fText.TextStyle.TextProperties.FontSize = "10pt";
                paragraph.TextContent.Add(fText);
                headerCell.Content.Add(paragraph);
                sheet.Rows[actualRow].Cells.Add(headerCell);
            }

        }

        /// <summary>
        /// Metodo per l'aggiunta delle righe al report
        /// </summary>
        /// <param name="sheet">Foglio a cui aggiungere le righe</param>
        /// <param name="reportRows">Righe del report</param>
        private void AddReportData(Table sheet, DocsPaVO.Report.ReportMapRow reportRows)
        {
            foreach (var row in reportRows.Rows)
            {
                sheet.Rows.Add(new Row(sheet));
                foreach (var column in row.Columns)
                {
                    Cell columnItem = new Cell(sheet.Document, "cell004");
                    columnItem.OfficeValueType = "string";
                    columnItem.CellStyle.CellProperties.Border = Border.HeavySolid;
                    Paragraph paragraph = ParagraphBuilder.CreateSpreadsheetParagraph(sheet.Document);
                    FormatedText fText = new FormatedText(sheet.Document, "T4", column.Value);
                    fText.TextStyle.TextProperties.Bold = "bold";
                    fText.TextStyle.TextProperties.FontSize = "10pt";
                    paragraph.TextContent.Add(fText);
                    columnItem.Content.Add(paragraph);
                    int rowNum = sheet.Rows.Count - 1;
                    sheet.Rows[rowNum].Cells.Add(columnItem);
                }
                
            }

        }

        /// <summary>
        /// Metodo per la cancellazione dei dati temporanei
        /// </summary>
        /// <param name="directoryInfo">Directory da eliminare</param>
        private void DeleteTemporaryData(DirectoryInfo directoryInfo)
        {
            try
            {
                // Cancellazione della directory e di tutti i file in essa contenuti
                directoryInfo.Delete(true);
                
            }
            catch (Exception e)
            {

            }
        }
    }
}
