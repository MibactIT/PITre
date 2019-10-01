using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace StampaRegistri.DocsPaWR25
{
	[GeneratedCode("System.Xml", "2.0.50727.42"), DesignerCategory("code"), DebuggerStepThrough, XmlType(Namespace = "http://localhost")]
	[Serializable]
	public class FascicolazioneClassificazione : MarshalByRefObject
	{
		private object[] childsField;

		private string codiceField;

		private string descrizioneField;

		private string systemIDField;

		private string accessRightsField;

		private Registro registroField;

		private string codUltimoField;

		private string idParentField;

		public object[] childs
		{
			get
			{
				return this.childsField;
			}
			set
			{
				this.childsField = value;
			}
		}

		public string codice
		{
			get
			{
				return this.codiceField;
			}
			set
			{
				this.codiceField = value;
			}
		}

		public string descrizione
		{
			get
			{
				return this.descrizioneField;
			}
			set
			{
				this.descrizioneField = value;
			}
		}

		public string systemID
		{
			get
			{
				return this.systemIDField;
			}
			set
			{
				this.systemIDField = value;
			}
		}

		public string accessRights
		{
			get
			{
				return this.accessRightsField;
			}
			set
			{
				this.accessRightsField = value;
			}
		}

		public Registro registro
		{
			get
			{
				return this.registroField;
			}
			set
			{
				this.registroField = value;
			}
		}

		public string codUltimo
		{
			get
			{
				return this.codUltimoField;
			}
			set
			{
				this.codUltimoField = value;
			}
		}

		public string idParent
		{
			get
			{
				return this.idParentField;
			}
			set
			{
				this.idParentField = value;
			}
		}
	}
}