<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AssociazioneStatiScadenze.aspx.cs" Inherits="DocsPAWA.AdminTool.Gestione_DiagrammiStato.AssociazioneStatiScadenze" %>
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
    <script language="javascript">
        function OkAndClose() {
            alert('Associazione effettuata con successo');
            window.close();
        }
    </script>
</head>

<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeOut="3600"></asp:ScriptManager>
        <uct:AppTitleProvider ID="appTitleProvider" runat="server" PageName = "AMMINISTRAZIONE > Gestione scadenze stati" />

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
                            <asp:DataGrid ID="dg_AssStatiScadenze" runat="server" BorderWidth="1px" CellPadding="1" BorderColor="Gray"
                                AutoGenerateColumns="false" Width="700px" OnItemDataBound="dg_AssStatiScadenze_ItemDataBound">
                                <SelectedItemStyle CssClass="bg_grigioS" />
                                <AlternatingItemStyle CssClass="bg_grigioA" />
                                <ItemStyle CssClass="bg_grigioN" />
                                <HeaderStyle HorizontalAlign="Center" CssClass="menu_1_bianco_dg" BackColor="#810D06" />
                                <Columns>
                                    <asp:BoundColumn DataField="ID_STATO" HeaderText="Id Stato" Visible="false"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="STATO" HeaderText="Stato">
                                        <HeaderStyle Width="25%"></HeaderStyle>
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="Sospensivo">
                                        <HeaderStyle HorizontalAlign="Center" Width="8%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="ChkSospensivo" AutoPostBack="true" OnCheckedChanged="ChkSospensivo_CheckedChanged" runat="server" Checked='<%# IsCheckedSosp(DataBinder.Eval(Container, "DataItem.TIPO_STATO")) %>' />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Interruttivo">
                                        <HeaderStyle HorizontalAlign="Center" Width="8%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="ChkInterruttivo" AutoPostBack="true" OnCheckedChanged="ChkInterruttivo_CheckedChanged" runat="server" Checked='<%# IsCheckedInt(DataBinder.Eval(Container, "DataItem.TIPO_STATO")) %>' />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Termini temporali">
                                        <HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txt_termini" CssClass="testo" MaxLength="4" Width="30px" Text='<%# DataBinder.Eval(Container, "DataItem.TERMINI").ToString() %>' 
                                                 Enabled='<%#!String.IsNullOrEmpty(DataBinder.Eval(Container, "DataItem.TERMINI").ToString())%>' ></asp:TextBox>
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
