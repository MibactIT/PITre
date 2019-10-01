<%@ Page language="c#" Codebehind="trasmDatiTrasm_dx.aspx.cs" AutoEventWireup="false" Inherits="DocsPAWA.trasmissione.trasmDatiTrasm_dx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>DOCSPA > trasmDatiTrasm_dx</title>
		<meta content="Microsoft Visual Studio 7.0" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script language="javascript" src="../LIBRERIE/DocsPA_Func.js"></script>
		<LINK href="../CSS/docspa_30.css" type="text/css" rel="stylesheet">
		<script language='javascript'>
			var w = window.screen.width;
			var h = window.screen.height;
			var new_w = (w-100)/2;
			var new_h = (h-400)/2;
			
			function redirect()
			{
				top.principale.document.location='../documento/gestionedoc.aspx?tab=trasmissioni';
			}
			
			function apriSalvaModTrasm()
			{
				window.showModalDialog('../documento/AnteprimaProfDinModal.aspx?Chiamante=../popup/salvaModTrasm.aspx','','dialogWidth:490px;dialogHeight:280px;status:no;resizable:yes;scroll:yes;center:yes;help:no;close:no;top:'+ new_h +';left:'+new_w);				
			}
			
			function apriSceltaNuovoPropietario(tipo)
			{
			    window.showModalDialog('../popup/sceltaNuovoProprietario.aspx?tipo='+tipo,'NuovoProprietario','dialogWidth:600px;dialogHeight:550px;status:no;resizable:yes;scroll:yes;center:yes;help:no;close:no');				
			}			
		</script>		
	</HEAD>
	<body>
		<form id="trasmDatiTrasm_dx" method="post" runat="server">
		    <INPUT id="flag_save" type="hidden" name="flag_save" runat="server">
		    <input id="azione" type=hidden name="azione" runat=server /> 
			<table cellSpacing="0" cellPadding="0" width="100%" align="right" border="0">
				<tr>
					<td class="infoDT" align="center" height="20"><asp:label id="titolo" Runat="server" CssClass="titolo_rosso"></asp:label></td>
				</tr>
				<tr>
					<td><asp:table id="tbl_Lista" runat="server" Width="100%" CellPadding="0" CellSpacing="1" HorizontalAlign="Right"></asp:table></td>
				</tr>
				<tr>
					<td><asp:button id="Button1" runat="server" CssClass="PULSANTE" Visible="False" Text="SALVA"></asp:button></td>
				</tr>
			</table>
            <div id="please_wait" style="display:none; z-index:1000; border-right: #000000 2px solid; border-top: #000000 2px solid;
                border-left: #000000 2px solid; border-bottom: #000000 2px solid; position: absolute;
                background-color: #d9d9d9">
                <table cellspacing="0" cellpadding="0" width="350px" border="0">
                    <tr>
                        <td valign="middle" style="font-weight: bold; font-size: 12pt; font-family: Verdana"
                            align="center" width="350px" height="90px">
                            Attendere, prego...
                        </td>
                    </tr>
                </table>
            </div>
		</form>
	</body>
</HTML>
