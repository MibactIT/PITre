﻿using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Xml;
using System.Web.Services.Protocols;

namespace Rubrica
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SoapExceptionFactory
    {
        /// <summary>
        /// 
        /// </summary>
        private SoapExceptionFactory()
        { }

        /// <summary>
        /// Creazione SoapException custom con i dettagli dell'eccezione
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static SoapException Create(Exception exception)
        {
            XmlDocument doc = new XmlDocument();

            // Create detail node
            XmlNode detailNode = doc.CreateNode(XmlNodeType.Element, 
                        SoapException.DetailElementName.Name, SoapException.DetailElementName.Namespace);

            // Add original exception type
            XmlNode exNode = doc.CreateNode(XmlNodeType.Element, "ExceptionType", SoapException.DetailElementName.Namespace);
            exNode.InnerText = exception.GetType().ToString();

            // Add original exception message
            XmlNode exMessage = doc.CreateNode(XmlNodeType.Element, "ExceptionMessage", SoapException.DetailElementName.Namespace);
            exMessage.InnerText = exception.Message;

            // Add original exception stack trace
            XmlNode exStackTrace = doc.CreateNode(XmlNodeType.Element, "ExceptionTrace", SoapException.DetailElementName.Namespace);
            exStackTrace.InnerText = exception.StackTrace;

            // Append the extra details to main detail node
            detailNode.AppendChild(exNode);
            detailNode.AppendChild(exMessage);
            detailNode.AppendChild(exStackTrace);

            // Build and return new custom SoapException
            return new SoapException(string.Empty,
                                SoapException.ServerFaultCode,
                                HttpContext.Current.Request.Url.AbsoluteUri,
                                detailNode);
        }
    }
}
