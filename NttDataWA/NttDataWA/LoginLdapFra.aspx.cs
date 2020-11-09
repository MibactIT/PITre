using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NttDataWA
{
    public partial class LoginLdapFra : System.Web.UI.Page
    {
        private string sessionend = string.Empty;


        private string userid;
        private string codamm;
        private bool ldapok;
        public string UserId { get { return this.userid; } }
        public string CodAmm { get { return this.codamm; } }
        public bool LdapOK { get { return this.ldapok; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            userid = Request.Form.Get("userId");
            codamm = Request.Form.Get("amm");
            ldapok = (Request.Form.Get("LDapOk") != null) ? Convert.ToBoolean(Request.Form.Get("LDapOk")) : false;

            Session["userid"] = userid;
            Session["codamm"] = codamm;
            Session["ldapok"] = ldapok;
            
        }
    }        
}