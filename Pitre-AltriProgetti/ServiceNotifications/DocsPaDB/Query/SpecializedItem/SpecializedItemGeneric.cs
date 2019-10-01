﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocsPaVO.NotificationCenter;
using System.Data;
using System.Configuration;
using System.IO;
using log4net;

namespace DocsPaDB.Query.SpecializedItem
{
    public class SpecializedItemGeneric : DBProvider, ISpecializedItem
    {
        private static ILog logger = LogManager.GetLogger(typeof(SpecializedItemConvertPdf));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public string CreateSpecializedItem(Event e)
        {
            string specializedItem = string.Empty;

            try
            {
                DataSet ds = new DataSet();
                DocsPaUtils.Query q;
                string queryString;
                switch (e.DOMAIN_OBJECT)
                {
                    case SupportStructures.ListDomainObject.DOCUMENT:
                        q = DocsPaUtils.InitQuery.getInstance().getQuery("S_ITEMS_SPECIALIZED_GENERIC_DOCUMENT");
                        q.setParam("idEvent", e.SYSTEM_ID.ToString());
                        queryString = q.getSQL();
                        this.ExecuteQuery(out ds, "SpecializedItem", q.getSQL());
                        break;
                    case SupportStructures.ListDomainObject.FASCICOLO:
                        q = DocsPaUtils.InitQuery.getInstance().getQuery("S_ITEMS_SPECIALIZED_GENERIC_FOLDER");
                        q.setParam("idEvent", e.SYSTEM_ID.ToString());
                        queryString = q.getSQL();
                        this.ExecuteQuery(out ds, "SpecializedItem", q.getSQL());
                        break;
                }
                CreateSpecializedItemObject(ds, ref specializedItem);
            }
            catch (Exception exc)
            {
                // traccia l'eccezione nel file di log
                logger.Error(exc);
            }
            return specializedItem;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="specializedItem"> </param>
        public void CreateSpecializedItemObject(DataSet ds, ref string specializedItem)
        {
            try
            {
                System.Text.StringBuilder strbuilder = new StringBuilder();
                if (ds.Tables["SpecializedItem"] != null && ds.Tables["SpecializedItem"].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables["SpecializedItem"].Rows[0];
                    if (!string.IsNullOrEmpty(dr["DESC_OBJECT"].ToString()))
                    {
                        strbuilder.Append(SupportStructures.TagItem.LINE + SupportStructures.TagItem.LABEL +
                           "lblObjectDescription" + SupportStructures.TagItem.CLOSE_LABEL + dr["DESC_OBJECT"].ToString() + SupportStructures.TagItem.CLOSE_LINE);

                    }
                    if (!string.IsNullOrEmpty(dr["DESC_SENDER"].ToString()))
                    {
                        strbuilder.Append(SupportStructures.TagItem.LINE + SupportStructures.TagItem.LABEL +
                            "lblSender" + SupportStructures.TagItem.CLOSE_LABEL + dr["DESC_SENDER"].ToString() + SupportStructures.TagItem.CLOSE_LINE);
                    }
                    if (!string.IsNullOrEmpty(dr["TIPO_ATTO"].ToString()))
                    {
                        strbuilder.Append(SupportStructures.TagItem.LINE + SupportStructures.TagItem.LABEL +
                            "lblDocType" + SupportStructures.TagItem.CLOSE_LABEL + dr["TIPO_ATTO"].ToString() + SupportStructures.TagItem.CLOSE_LINE);
                    }
                    if (!string.IsNullOrEmpty(dr["DESCRIZIONE_EVENTO"].ToString()))
                    {
                        strbuilder.Append(SupportStructures.TagItem.LINE + SupportStructures.TagItem.LABEL +
                            "lblEventDescription" + SupportStructures.TagItem.CLOSE_LABEL + dr["DESCRIZIONE_EVENTO"].ToString() + SupportStructures.TagItem.CLOSE_LINE);
                    }
                }
                specializedItem = strbuilder.ToString();
            }
            catch (Exception exc)
            {
                // traccia l'eccezione nel file di log
                logger.Error(exc);
            }
        }
    }
}
