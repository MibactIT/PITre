using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;

namespace VtDocsWS.Services
{
    /// <summary>
    ///  Definizione dei servizi per l'autenticazione dei Product Integration Services.  
    /// </summary>
    [ServiceContract(Namespace = "http://nttdata.com/2012/Pi3")]
    public interface IAuthentication
    {
        [OperationContract]
        VtDocsWS.Services.Authentication.LogIn.LogInResponse LogIn(VtDocsWS.Services.Authentication.LogIn.LogInRequest request);

        [OperationContract]
        VtDocsWS.Services.Authentication.LogOut.LogOutResponse LogOut(VtDocsWS.Services.Authentication.LogOut.LogOutRequest request);

        [OperationContract]
        VtDocsWS.Services.Authentication.Authenticate.AuthenticateResponse Authenticate(VtDocsWS.Services.Authentication.Authenticate.AuthenticateRequest request);

        [OperationContract]
        VtDocsWS.Services.AddressBook.SearchUsers.SearchUsersResponse GetAllUsers();

        [OperationContract]
        VtDocsWS.Services.Authentication.GetAooListForUser.AooUserAssociationResponse GetAooListForUser(string username);

        [OperationContract]
        VtDocsWS.Services.Authentication.GetAooListForUser.AooUserAssociationResponse GetUsersForAoo(string idAoo);

        [OperationContract]
        VtDocsWS.Services.Authentication.GetAooListForUser.AooUserAssociationResponse GetAllAoo();

        [OperationContract]
        VtDocsWS.Services.Authentication.GetAooListForUser.AooUserAssociationResponse GetAssociationUserAooFromGiada(string id, string username);
    }
}