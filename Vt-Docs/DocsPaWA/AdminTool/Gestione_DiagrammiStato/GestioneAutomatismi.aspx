<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GestioneAutomatismi.aspx.cs" Inherits="DocsPAWA.AdminTool.Gestione_DiagrammiStato.GestioneAutomatismi" %>
<%@ Register src="../../UserControls/AppTitleProvider.ascx" tagname="AppTitleProvider" tagprefix="uct" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"> 

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta content="Microsoft Visual Studio 7.0" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../CSS/AmmStyle.css" type="text/css" rel="stylesheet">
	<base target="_self" />
</head>

<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeOut="3600"></asp:ScriptManager>
        <uct:AppTitleProvider ID="appTitleProvider" runat="server" PageName = "AMMINISTRAZIONE > Gestione cambi di stato automatici" />

        <table id="table3" width="100%">
            <tr>
                <td class="titolo"  align="left" bgColor="#e0e0e0" height="20">
				    <asp:Label id="lbl_titolo" runat="server"></asp:Label>				
			    </td>
            </tr>
            <tr>
                <td>
                    <div id="DivDGList" align="center">
                        <asp:DataGrid ID="dg_cambiStatoAutomatici" runat="server" BorderWidth="1px" CellPadding="1" BorderColor="Gray"
                            AutoGenerateColumns="false" Width="98%" OnItemCommand="dg_cambiStatoAutomatici_ItemCommand">
                            <SelectedItemStyle CssClass="bg_grigioS" />
                            <AlternatingItemStyle CssClass="bg_grigioA" />
                            <ItemStyle CssClass="bg_grigioN" />
                            <HeaderStyle HorizontalAlign="Center" CssClass="menu_1_bianco_dg" BackColor="#810D06" />
                            <Columns>
                                <asp:BoundColumn HeaderText="Id stato" Visible="false" DataField="ID_STATO"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Stato succ." DataField="STATO"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Evento" DataField="DESC_EVENTO"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Tipo doc." DataField="DESC_TIPOLOGIA"></asp:BoundColumn>
                                <asp:BoundColumn HeaderText="Ragione" DataField="DESC_RAGIONE"></asp:BoundColumn>
                                <asp:ButtonColumn Text="&lt;img src=../Images/cestino.gif border=0 alt='Elimina'&gt;" HeaderText="Elimina" CommandName="Delete">
                                    <HeaderStyle Width="5%"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:ButtonColumn>
                            </Columns>
                        </asp:DataGrid>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="border-bottom-style:solid; border-bottom-color:#810D06; width:100%;"></div>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Button id="btn_add" runat="server" CssClass="testo_btn_p" Text="Aggiungi" OnClick="btn_add_Click"></asp:Button>
                    <asp:Button ID="btn_close" runat="server" CssClass="testo_btn_p" Text="Chiudi" OnClientClick="window.close();" />
                </td>
            </tr>
        </table>
        <asp:Panel ID="pnlAddAutomatismo" runat="server" Visible="false">
            <table width="98%" bgColor="#f6f4f4" style="BORDER-RIGHT: #810d06 1px solid; BORDER-TOP: #810d06 1px solid; BORDER-LEFT: #810d06 1px solid; BORDER-BOTTOM: #810d06 1px solid; margin: 10px;" align="center">
                <tr>
                    <td class="testo_grigio_scuro" width="30%">Stato successivo</td>
                    <td>
                        <asp:DropDownList ID="ddl_stati_successivi" runat="server" CssClass="testo" Width="150px"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="testo_grigio_scuro" width="30%">Evento</td>
                    <td>
                        <asp:DropDownList ID="ddl_eventi" runat="server" CssClass="testo" Width="150px" OnSelectedIndexChanged="ddl_eventi_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="testo_grigio_scuro" width="30%">Tipo documento</td>
                    <td>
                        <asp:DropDownList ID="ddl_tipo_doc" runat="server" CssClass="testo" Width="150px"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="testo_grigio_scuro" width="30%">Ragione trasm.</td>
                    <td>
                        <asp:DropDownList ID="ddl_ragione" runat="server" CssClass="testo" Width="150px"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="right" width="50%">
                        <asp:Button ID="btn_save" runat="server" CssClass="testo_btn_p_large" Text="Salva" OnClick="btn_save_Click" />
                    </td>
                    <td align="left" width="50%">
                        <asp:Button ID="btn_undo" runat="server" CssClass="testo_btn_p_large" Text="Annulla" OnClick="btn_undo_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>

    </form>
</body>
</html>
