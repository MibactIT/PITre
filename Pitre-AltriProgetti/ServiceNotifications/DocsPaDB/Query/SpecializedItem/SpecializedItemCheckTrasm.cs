﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocsPaVO.NotificationCenter;
using System.Data;
using System.IO;
using System.Configuration;
using log4net;

namespace DocsPaDB.Query.SpecializedItem
{
    public class SpecializedItemBasicInfoDoc : DBProvider, ISpecializedItem
    {
        private static ILog logger = LogManager.GetLogger(typeof(SpecializedItemBasicInfoDoc));
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
                string queryString;
                DocsPaUtils.Query q;
                switch (e.DOMAIN_OBJECT)
                {
                    case SupportStructures.ListDomainObject.DOCUMENT:
                        q = DocsPaUtils.InitQuery.getInstance().getQuery("S_ITEMS_SPECIALIZED_BASIC_INFO_DOC");
                        q.setParam("idProfile", e.ID_OBJECT.ToString());
                        queryString = q.getSQL();
                        this.ExecuteQuery(out ds, "SpecializedItem", q.getSQL());
                        break;
                    case SupportStructures.ListDomainObject.FASCICOLO:
                        q = DocsPaUtils.InitQuery.getInstance().getQuery("S_ITEMS_SPECIALIZED_BASIC_INFO_FOLDER");
                        q.setParam("idProject", e.ID_OBJECT.ToString());
                        queryString = q.getSQL();
                        this.ExecuteQuery(out ds, "SpecializedItemFolder", q.getSQL());
                        break;
                }
                CreateSpecializedItemObject(ds, ref specializedItem);
            }
            catch (Exception exc) 
            {
                // traccia l'eccezione nel file di log
                logger.Error (exc);
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
            System.Text.StringBuilder strbuilder = new StringBuilder();
            DataRow dr = null;
            bool isFolder = false;
            try
            {
                if (ds.Tables["SpecializedItem"] != null && ds.Tables["SpecializedItem"].Rows.Count > 0)
                {
                    dr = ds.Tables["SpecializedItem"].Rows[0];
                }
                else if (ds.Tables["SpecializedItemFolder"] != null && ds.Tables["SpecializedItemFolder"].Rows.Count > 0)
                {
                    isFolder = true;
                    dr = ds.Tables["SpecializedItemFolder"].Rows[0];
                }
                if (dr != null)
                {
                    if (!string.IsNullOrEmpty(dr["DESC_OBJECT"].ToString()))
                    {
                        strbuilder.Append(SupportStructures.TagItem.LINE + SupportStructures.TagItem.LABEL +
                           "lblObjectDescription" + SupportStructures.TagItem.CLOSE_LABEL + dr["DESC_OBJECT"].ToString() + SupportStructures.TagItem.CLOSE_LINE);
                   
                    }
                    if (!isFolder && !string.IsNullOrEmpty(dr["DESC_SENDER"].ToString()))
                    {
                        strbuilder.Append(SupportStructures.TagItem.LINE + SupportStructures.TagItem.LABEL +
                            "lblSender" + SupportStructures.TagItem.CLOSE_LABEL + dr["DESC_SENDER"].ToString() + SupportStructures.TagItem.CLOSE_LINE);
                    }
                    if (!isFolder)
                    {
                        if (!string.IsNullOrEmpty(dr["TIPO_ATTO"].ToString()))
                        {
                            strbuilder.Append(SupportStructures.TagItem.LINE + SupportStructures.TagItem.LABEL +
                                "lblDocType" + SupportStructures.TagItem.CLOSE_LABEL + dr["TIPO_ATTO"].ToString() + SupportStructures.TagItem.CLOSE_LINE);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(dr["TIPO_FASC"].ToString()))
                        {
                            strbuilder.Append(SupportStructures.TagItem.LINE + SupportStructures.TagItem.LABEL +
                                "lblFascType" + SupportStructures.TagItem.CLOSE_LABEL + dr["TIPO_FASC"].ToString() + SupportStructures.TagItem.CLOSE_LINE);
                        }
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
