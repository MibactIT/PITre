<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StartProcessSignature.aspx.cs"
    Inherits="NttDataWA.Popup.StartProcessSignature" MasterPageFile="~/MasterPages/Popup.Master" %>

<%@ Register Assembly="NttDatalLibrary" Namespace="NttDatalLibrary" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ajaxpopup2.ascx" TagPrefix="uc" TagName="ajaxpopup2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/Calendar.ascx" TagPrefix="uc6" TagName="Calendar" %>
<asp:Content ID="ContentPlaceHolderHeader" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/chosen.jquery.min.js" type="text/javascript"></script>
    <link href="../Css/chosen.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .container
        {
            position: fixed;
            top: 1px;
            left: 0px;
            bottom: 71px;
            right: 0px;
            overflow: auto;
            background: #ffffff;
            text-align: left;
            padding: 10px;
        }
        .containerTreeView
        {
            clear: both;
            min-height: 18px;
            text-align: left;
            vertical-align: top;
            max-height: 150px;
            max-width: 100%;
            overflow: auto;
        }
        .TreeSignatureProcess
        {
            padding: 0;       
            margin: 0 0 10px 0;
            color: #0f64a1;
        }
        
        
        .TreeSignatureProcess img
        {
            width: 20px;
            height: 20px;
        }
        
        .TreeSignatureProcess_node a:link, .TreeSignatureProcess_node a:visited, .TreeSignatureProcess_node a:hover
        {
            padding: 0 5px;
        }
        
        .TreeSignatureProcess_selected
        {
            background-color: #477FAF;
            color: #fff;
        }
        
        .TreeSignatureProcess_selected a:link, .TreeSignatureProcess_selected a:visited, .TreeSignatureProcess_selected a:hover
        {
            padding: 0 5px;
            background-color: transparent;
            color: #fff;
        }
        .txt_textarea
        {
            width: 100%;
            border: 1px solid #cccccc;
            line-height: 18px;
            font-family: Verdana, Arial, Verdana, sans-serif;
            font-size: 13px;
            height: 50px;
            overflow: auto;
            border-radius: 5px;
            -ms-border-radius: 5px; /* ie */
            -moz-border-radius: 5px; /* firefox */
            -webkit-border-radius: 5px; /* safari, chrome */
            -o-border-radius: 5px; /* opera */
        }
        
        .txt_textarea_disabled
        {
            width: 100%;
            border: 1px solid #cccccc;
            line-height: 18px;
            font-family: Verdana, Arial, Verdana, sans-serif;
            font-size: 13px;
            height: 50px;
            overflow: auto;
            background-color: #f8f8f8;
            border-radius: 5px;
            -ms-border-radius: 5px; /* ie */
            -moz-border-radius: 5px; /* firefox */
            -webkit-border-radius: 5px; /* safari, chrome */
            -o-border-radius: 5px; /* opera */
        }
        .disabled
        {
           color: #848484;
           text-decoration: line-through;
        }
    </style>
</asp:Content>
<asp:Content ID="ContentPlaceHolderContent" ContentPlaceHolderID="ContentPlaceHolderContent"
    runat="server">
    <div class="container">
        <div class="row">
            <asp:PlaceHolder ID="plcNoSignatureProcess" runat="server" Visible="false">
                <asp:Label ID="lblNoVisibleSignatureProcess" runat="server"> </asp:Label>
            </asp:PlaceHolder>
            <asp:UpdatePanel runat="server" ID="upPnlSignatureProcesses" UpdateMode="Conditional"
                ClientIDMode="Static">
                <ContentTemplate>
                    <asp:PlaceHolder ID="plcSignatureProcesses" runat="server">
                        <div class="row">
                            <div class="col">
                                <span class="weight">
                                    <asp:Literal ID="lblStartProcessSignature" runat="server" /></span>
                            </div>
                        </div>
                        <div class="containerTreeView">
                            <asp:UpdatePanel runat="server" ID="UpdatePanelTreeView" UpdateMode="Conditional"
                                ClientIDMode="Static">
                                <ContentTemplate>
                                    <asp:TreeView ID="TreeProcessSignature" runat="server" ShowLines="true" NodeStyle-CssClass="TreeSignatureProcess_node"
                                        SelectedNodeStyle-CssClass="TreeSignatureProcess_selected" OnSelectedNodeChanged="TreeSignatureProcess_SelectedNodeChanged"
                                        OnTreeNodeCollapsed="TreeSignatureProcess_Collapsed" OnTreeNodeExpanded="TreeSignatureProcess_Collapsed"
                                        CssClass="TreeSignatureProcess" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <asp:PlaceHolder ID="PlcNote" runat="server">
                            <div class="row">
                                <span class="weight">
                                    <asp:Literal ID="ltlNote" runat="server"></asp:Literal>
                                </span>
                            </div>
                            <div style="width: 90%;">
                                <div>
                                    <cc1:CustomTextArea ID="txtNotes" runat="server" CssClass="txt_textarea" CssClassReadOnly="txt_textarea_disabled"
                                        ClientIDMode="Static" TextMode="MultiLine">
                                    </cc1:CustomTextArea>
                                </div>
                                <div class="row">
                                    <div class="col-right-no-margin">
                                        <span class="charactersAvailable">
                                            <asp:Literal ID="ltrNotes" runat="server" ClientIDMode="Static"></asp:Literal>
                                            <span id="txtNotes_chars" clientidmode="Static" runat="server"></span></span>
                                    </div>
                                </div>
                            </div>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="PlcNotificationOption" runat="server">
                            <div class="row">
                                <div class="col">
                                    <span class="weight">
                                        <asp:Literal ID="ltlNotificationOption" runat="server"></asp:Literal>
                                    </span>
                                </div>
                                <div class="row">
                                    <div class="col">
                                        <asp:CheckBoxList ID="cbxNotificationOption" runat="server" RepeatDirection="Vertical">
                                            <asp:ListItem Value="CP" runat="server" ID="cbxNotificationOptionOptCP"></asp:ListItem>
                                            <asp:ListItem Value="IP" runat="server" ID="cbxNotificationOptionOptIP" Selected="true"></asp:ListItem>
                                        </asp:CheckBoxList>
                                    </div>
                                </div>
                            </div>
                        </asp:PlaceHolder>
                    </asp:PlaceHolder>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="upReport" runat="server" UpdateMode="Conditional" ClientIDMode="Static">
                <ContentTemplate>
                    <asp:Panel ID="pnlReport" runat="server" CssClass="row" Visible="false">
                        <asp:GridView ID="grdReport" runat="server" Width="95%" AutoGenerateColumns="False"
                            CssClass="tbl_rounded_custom round_onlyextreme">
                            <RowStyle CssClass="NormalRow" />
                            <AlternatingRowStyle CssClass="AltRow" />
                            <PagerStyle CssClass="recordNavigator2" />
                            <Columns>
                                <asp:BoundField HeaderText='<%$ localizeByText:MassiveActionLblGrdReport%>' DataField="ObjId">
                                    <HeaderStyle HorizontalAlign="Center" Width="30%" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Esito" DataField="Result">
                                    <HeaderStyle HorizontalAlign="Center" Width="30%" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Dettagli" DataField="Details" HtmlEncode="false">
                                    <HeaderStyle HorizontalAlign="Center" Width="30%" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
<asp:Content ID="ContentButtons" ContentPlaceHolderID="ContentPlaceOldersButtons"
    runat="server">
    <asp:UpdatePanel ID="UpPnlButtons" runat="server" ClientIDMode="Static" UpdateMode="Conditional">
        <ContentTemplate>
            <cc1:CustomButton ID="StartProcessSignatureAssigns" runat="server" CssClass="btnEnable"
                OnClientClick="disallowOp('ContentPlaceHolderContent')" CssClassDisabled="btnDisable"
                OnMouseOver="btnHover" ClientIDMode="Static" OnClick="StartProcessSignatureAssigns_Click" />
            <cc1:CustomButton ID="StartProcessSignatureClose" runat="server" CssClass="btnEnable"
                OnClientClick="disallowOp('ContentPlaceHolderContent')" CssClassDisabled="btnDisable"
                OnMouseOver="btnHover" ClientIDMode="Static" OnClick="StartProcessSignatureClose_Click" />
            <cc1:CustomButton ID="BtnReport" runat="server" CssClass="btnEnable" CssClassDisabled="btnDisable"
                Visible="false" OnMouseOver="btnHover" ClientIDMode="Static" OnClick="BtnReport_Click"
                OnClientClick="disallowOp('ContentPlaceHolderContent');" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">        $(".chzn-select-deselect").chosen({ allow_single_deselect:
true, no_results_text: "Nessun risultato trovato"
        }); $(".chzn-select").chosen({
            no_results_text: "Nessun risultato trovato"
        }); </script>
</asp:Content>
