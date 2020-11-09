using System; 
using System.Web; 
using System.Configuration; 
using System.Collections; 
using System.Text;
using log4net; 

namespace DocsPAWA.gestione.sessione
{
	/// <summary>
	/// Summary description for httpModule.
	/// </summary>
	public class SessionTimeout: IHttpModule 
	{
        private ILog logger = LogManager.GetLogger(typeof(SessionTimeout));
		public SessionTimeout() {} 

		public void Init(HttpApplication app) 
		{ 
			app.PreRequestHandlerExecute += new EventHandler(this.OnPreRequestHandler); 
		} 

		public void Dispose() {} 

		string GetAppPath (HttpContext ctx)
		{
			StringBuilder path=new StringBuilder(ctx.Request.Url.GetLeftPart(UriPartial.Authority)); 
			path.Append(ctx.Request.ApplicationPath); 
			path.Append((ctx.Request.ApplicationPath.Length > 0 ? "/" : "") ); 
			return path.ToString();
		}

		/// <summary>
		/// Verifica se la sessione corrente � relativa al sito accessibile
		/// </summary>
		/// <param name="ctx"></param>
		/// <returns></returns>
		private bool OnSitoAccessibile(HttpContext ctx)
		{
			return (ctx.Request.Path.ToLower().IndexOf("sitoaccessibile") >= 0);
		}

		bool IsAdminTool (HttpContext ctx)
		{
			return (ctx.Request.Path.ToLower().IndexOf("admintool") >= 0);
		}

		public void OnPreRequestHandler(object obj, EventArgs args) 
		{ 
			HttpApplication app =(HttpApplication) obj; 
			HttpContext ctx = app.Context;

            //logger.Debug(ctx.Request.Url.AbsolutePath.ToLower());
			// Modifica per integrazione con portale ANAS
			if (ctx.Request.Url.AbsolutePath.ToLower().IndexOf("portal_docspa.aspx") > 0)
				return; 
			// Modifica per evitare Pagina sessione scaduta quando passo di nuovo su login subito dopo logoff.
			
			if (ctx.Request.Url.AbsolutePath.ToLower().IndexOf("login.aspx") > 0)
				return; 
			//Evito ricorsione su pagina di uscita
			if (ctx.Request.Url.AbsolutePath.ToLower().IndexOf("exit.aspx") > 0)
				return; 

            //Evito ricorsione su pagina del link email "VisualizzaOggetto.aspx" 
            if (ctx.Request.Url.AbsolutePath.ToLower().IndexOf("visualizzaoggetto.aspx") > 0)
                return;

            //Evito ricorsione su pagina del link email "VisualizzaDocEsterno.aspx" 
            if (ctx.Request.Url.AbsolutePath.ToLower().IndexOf("visualizzadocesterno.aspx") > 0)
                return;

            //Evito ricorsione su pagina del link email "visualizzaLink.aspx" 
            if (ctx.Request.Url.AbsolutePath.ToLower().IndexOf("visualizzalink.aspx") > 0)
                return;

            //Evito ricorsione su session aborted
            if (ctx.Request.Url.AbsolutePath.ToLower().IndexOf("sessionaborted.aspx") > 0)
                return;

            if (ctx.Request.Url.AbsolutePath.ToLower().IndexOf("loginldap.aspx") > 0)
                return;

			if (ctx.Session !=null) 
			{ 
				if (ctx.Session.IsNewSession) 
				{ 										
					string cookieHeader=ctx.Request.Headers["Cookie"]; 
					if ( (null != cookieHeader) && (cookieHeader.IndexOf("ASP.NET_SessionId") >= 0) ) 
					{ 
						string path = GetAppPath(ctx);
						if (!IsAdminTool(ctx))
						{
							if (this.OnSitoAccessibile(ctx))
								path += ("SitoAccessibile/SessionAborted.aspx?result=" + DocsPAWA.DocsPaWR.ValidationResult.SESSION_EXPIRED);
							else
								path += ("SessionAborted.aspx?result=" + DocsPAWA.DocsPaWR.ValidationResult.SESSION_EXPIRED);
						}
						else
						{
							path += "AdminTool/login.htm";
						}
						
						System.Diagnostics.Debug.WriteLine("�����.... [Nuova Sessione con Cookie] ....�����");
						System.Diagnostics.Debug.WriteLine("�����.... [ABORT!] ....�����");
						System.Diagnostics.Debug.WriteLine("�����.... [Path] : " + ctx.Request.Path + " ....�����");
						logger.Error("Sessione Scaduta, ultima pagina chiamata pagina: "+ctx.Request.Path);
						ctx.Response.Redirect(path.ToString()); 
					} 					
				} 
				else
				{					
					DocsPaWR.Utente utente = (DocsPAWA.DocsPaWR.Utente) ctx.Session["userData"];
					if (utente != null)
					{						
						DocsPaWR.ValidationResult resultValidationPage = UserManager.ValidateLogin(utente.userId, utente.idAmministrazione, ctx.Session.SessionID);
						
						if (resultValidationPage == DocsPAWA.DocsPaWR.ValidationResult.SESSION_DROPPED)
						{
							string path = GetAppPath(ctx);
							if (!IsAdminTool(ctx))
							{
								if (this.OnSitoAccessibile(ctx))
									path += ("SitoAccessibile/SessionAborted.aspx?result=" + DocsPAWA.DocsPaWR.ValidationResult.SESSION_DROPPED);
								else
									path += ("SessionAborted.aspx?result=" + DocsPAWA.DocsPaWR.ValidationResult.SESSION_DROPPED);
							}
							else
							{
								path += "AdminTool/login.htm";
							}

							ctx.Session["userData"]=null;
			
							System.Diagnostics.Debug.WriteLine("�����.... [Sessione esistente con dati utente] ....�����");
							System.Diagnostics.Debug.WriteLine("�����.... [ABORT!] ....�����");
							System.Diagnostics.Debug.WriteLine("�����.... [Path] : " + path + " ....�����");

							ctx.Response.Redirect(path); 
						}
					}
				}
			} 
		} 
	} 

}
