<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AssociazioneFasiStati.aspx.cs" Inherits="DocsPAWA.AdminTool.Gestione_DiagrammiStato.AssociazioneFasiStati" %>
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
        <uct:AppTitleProvider ID="appTitleProvider" runat="server" PageName = "AMMINISTRAZIONE > Associazione Fasi-Stati" />

        <div id="container">
            <table id="table3" align="center">
                <tr>
                    <td class="titolo"  align="left" bgColor="#e0e0e0" height="20">
				        <asp:Label id="lbl_titolo" runat="server"></asp:Label>				
			        </td>
                </tr>
                <tr>
                    <td>
                        <div id="DivDGList">
                            <asp:DataGrid ID="dg_AssStatiFasi" runat="server" BorderWidth="1px" CellPadding="1" BorderColor="Gray"
                                AutoGenerateColumns="false" Width="700px" OnItemDataBound="dg_AssStatiFasi_ItemDataBound">
                                <SelectedItemStyle CssClass="bg_grigioS" />
                                <AlternatingItemStyle CssClass="bg_grigioA" />
                                <ItemStyle CssClass="bg_grigioN" />
                                <HeaderStyle HorizontalAlign="Center" CssClass="menu_1_bianco_dg" BackColor="#810D06" />
                                <Columns>
                                    <asp:BoundColumn DataField="STATO" HeaderText="Stato">
                                        <HeaderStyle Width="25%"></HeaderStyle>
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="Fase">
                                        <HeaderStyle Width="25%" />
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlFasi" runat="server"></asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="IdFase" Visible="false">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hid_fase" Value='<%# DataBinder.Eval(Container, "DataItem.ID_FASE").ToString() %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="ID_STATO" HeaderText="Stato" Visible="false"></asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="Tipo doc. associato">
                                        <HeaderStyle Width="50%" />
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlTipoDoc" runat="server" Width="90%"></asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="IdTipoDoc" Visible="false">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hid_tipo_doc" Value='<%# DataBinder.Eval(Container, "DataItem.ID_TIPO_DOC").ToString() %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                            </asp:DataGrid>
                        </div>
                    </td>
                </tr>
            </table>
            <table id="table4" align="center">
                    <tr>
                        <td align="center">
                            <asp:Button ID="btn_save" runat="server" CssClass="testo_btn_p_large" Text="Salva" OnClick="btn_save_Click" />
                        </td>
                        <td align="center">
                            <asp:Button ID="btn_close" runat="server" CssClass="testo_btn_p_large" Text="Chiudi" OnClientClick="window.close();" />
                        </td>
                    </tr>
            </table>
        </div>

    </form>
</body>
</html>
