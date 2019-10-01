using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace StampaRegistri.DocsPaWR305
{
	[GeneratedCode("System.Web.Services", "2.0.50727.42"), DesignerCategory("code"), DebuggerStepThrough]
	public class GetUOSmistamentoCompletedEventArgs : AsyncCompletedEventArgs
	{
		private object[] results;

		public UOSmistamento[] Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return (UOSmistamento[])this.results[0];
			}
		}

		internal GetUOSmistamentoCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState) : base(exception, cancelled, userState)
		{
			this.results = results;
		}
	}
}
