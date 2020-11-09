using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using DocsPaWS.VtDocsWS;
using VtDocsWS.Services;
using log4net;

namespace VtDocsWS.WebServices
{
    /// <summary>
    /// Servizi per portale procedimenti MIBACT
    /// </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(Namespace = "http://nttdata.com/2012/Pi3")]
    public class Proceedings : IProceedings
    {
        private ILog logger = LogManager.GetLogger(typeof(Proceedings));

        public Services.Proceedings.StartProceeding.StartProceedingResponse StartProceeding(Services.Proceedings.StartProceeding.StartProceedingRequest request)
        {
            logger.Info("BEGIN");

            Services.Proceedings.StartProceeding.StartProceedingResponse response = Manager.ProceedingsManager.StartProceeding(request);

            Utils.CheckFaultException(response);

            return response;
        }

        public Services.Proceedings.GetProceeding.GetProceedingResponse GetProceeding(Services.Proceedings.GetProceeding.GetProceedingRequest request)
        {
            logger.Info("BEGIN");

            Services.Proceedings.GetProceeding.GetProceedingResponse response = Manager.ProceedingsManager.GetProceedings(request);

            Utils.CheckFaultException(response);

            return response;
        }

        public Services.Proceedings.AddDocToProceeding.AddDocToProceedingResponse AddDocToProceeding(Services.Proceedings.AddDocToProceeding.AddDocToProceedingRequest request)
        {
            logger.Info("BEGIN");

            Services.Proceedings.AddDocToProceeding.AddDocToProceedingResponse response = Manager.ProceedingsManager.AddDocToProceeding(request);

            Utils.CheckFaultException(response);

            return response;
        }

        public Services.Proceedings.GetUnreadNotifications.GetUnreadNotificationsResponse GetUnreadNotifications(Services.Proceedings.GetUnreadNotifications.GetUnreadNotificationsRequest request)
        {
            logger.Info("BEGIN");

            Services.Proceedings.GetUnreadNotifications.GetUnreadNotificationsResponse response = Manager.ProceedingsManager.GetUnreadNotifications(request);

            Utils.CheckFaultException(response);

            return response;
        }

        public Services.Proceedings.SetReadNotifications.SetReadNotificationsResponse SetReadNotifications(Services.Proceedings.SetReadNotifications.SetReadNotificationsRequest request)
        {
            logger.Info("BEGIN");

            Services.Proceedings.SetReadNotifications.SetReadNotificationsResponse response = Manager.ProceedingsManager.SetReadNotifications(request);

            Utils.CheckFaultException(response);

            return response;
        }

        public Services.Proceedings.GetAOO.GetAOOResponse GetAOO(Services.Proceedings.GetAOO.GetAOORequest request )
        {
            logger.Info("BEGIN");

            Services.Proceedings.GetAOO.GetAOOResponse response = Manager.ProceedingsManager.GetAOO(request);

            Utils.CheckFaultException(response);

            return response;
        }

        public Services.Proceedings.GetTipologies.GetTipologiesResponse GetTipologies(Services.Proceedings.GetTipologies.GetTipologiesRequest request)
        {
            logger.Info("BEGIN");

            Services.Proceedings.GetTipologies.GetTipologiesResponse response = Manager.ProceedingsManager.GetTipologies(request);

            Utils.CheckFaultException(response);

            return response;
        }
        
    }
}
