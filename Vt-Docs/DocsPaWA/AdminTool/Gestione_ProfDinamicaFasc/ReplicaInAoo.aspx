<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReplicaInAoo.aspx.cs" Inherits="DocsPAWA.AdminTool.Gestione_ProfDinamicaFasc.ReplicaInAoo" %>
<%@ Register src="../../UserControls/AppTitleProvider.ascx" tagname="AppTitleProvider" tagprefix="uct" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link href="../CSS/AmmStyle.css" type="text/css" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
        <uct:AppTitleProvider ID="appTitleProvider" runat="server" PageName = "AMMINISTRAZIONE > Replica in Aoo" />
        <div>
            <table width="100%">
                <tr>
                    <td class="titolo" align="center" bgcolor="#e0e0e0" height="20">
                        <asp:Label ID="lbl_titolo" runat="server" Text="Replica in AOO"></asp:Label></td>
                    <td align="center" width="10%" bgcolor="#e0e0e0">
                        <asp:Button ID="btn_conferma" runat="server" Text="Conferma" CssClass="testo_btn_p" OnClick="btn_conferma_Click"></asp:Button></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div id="div_listaAoo" align="center" runat="server" style="overflow: auto; height: 310px; width: 100%;">
                            <asp:DataGrid ID="dg_listaAoo" runat="server" AutoGenerateColumns="False" Width="100%">
                                <AlternatingItemStyle CssClass="bg_grigioA"></AlternatingItemStyle>
                                <ItemStyle CssClass="bg_grigioN"></ItemStyle>
                                <HeaderStyle CssClass="menu_1_bianco_dg" BackColor="#810D06"></HeaderStyle>
                                <Columns>
                                    <asp:BoundColumn Visible="False" DataField="IDAmm" HeaderText="SystemId"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="Descrizione" HeaderText="Descrizione" ItemStyle-HorizontalAlign="Left">
                                        <HeaderStyle Width="60%"></HeaderStyle>
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="Selezione">
                                        <HeaderStyle HorizontalAlign="Center" Width="20%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbx_seleziona" runat="server"></asp:CheckBox>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                            </asp:DataGrid>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
