using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace StampaRegistri.DocsPaWR25
{
	[GeneratedCode("System.Web.Services", "2.0.50727.42"), DesignerCategory("code"), DebuggerStepThrough]
	public class amministrazioneGetParametroConfigurazioneCompletedEventArgs : AsyncCompletedEventArgs
	{
		private object[] results;

		public Configurazione Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return (Configurazione)this.results[0];
			}
		}

		internal amministrazioneGetParametroConfigurazioneCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState) : base(exception, cancelled, userState)
		{
			this.results = results;
		}
	}
}
