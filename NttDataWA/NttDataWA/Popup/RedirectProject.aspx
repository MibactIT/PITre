<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/Popup.Master" CodeBehind="RedirectProject.aspx.cs" Inherits="NttDataWA.Popup.RedirectProject" %>

<%@ Register Assembly="NttDatalLibrary" Namespace="NttDatalLibrary" TagPrefix="cc1" %>
<asp:Content ID="ContentPlaceHolderHeader" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/chosen.jquery.min.js" type="text/javascript"></script>
    <link href="../Css/chosen.css" rel="stylesheet" type="text/css" />
    <style type="text/css"></style>
</asp:Content>
<asp:Content ID="ContentPlaceHolderContent" ContentPlaceHolderID="ContentPlaceHolderContent" runat="server">
    <div class="content2">
        <div class="container" style="width: 98%;">
            <asp:Panel ID="PnlAoo" runat="server" Visible="true">
                <div class="row">
                    <div class="col-full">
                        <span class="weight">
                            <asp:Literal runat="server" ID="RedirectLitAoo"></asp:Literal>
                        </span>
                    </div>
                </div>
                <div class="row">
                    <div class="col-full">
                        <asp:DropDownList runat="server" ID="ddlAoo" CssClass="chzn-select-deselect" Width="100%" AutoPostBack="true"></asp:DropDownList>
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlNotes" runat="server">
                <div class="row">
                    <div class="col-full">
                        <asp:Literal runat="server" ID="RedirectLitNotes"></asp:Literal>
                    </div>
                </div>
                <div class="row">
                    <div class="col-full">
                        <cc1:CustomTextArea ID="RedirectTxtNotes" runat="server" TextMode="MultiLine" CssClass="txt_textarea"
                             CssClassReadOnly="txt_textarea_disabled" ClientIDMode="Static"></cc1:CustomTextArea>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
<asp:Content ID="ContentButtons" ContentPlaceHolderID="ContentPlaceOldersButtons" runat="server">
    <asp:UpdatePanel ID="UpButton" runat="server" UpdateMode="Conditional" ClientIDMode="Static">
        <ContentTemplate>
            <cc1:CustomButton ID="RedirectProjectBtnConfirm" runat="server" CssClass="btnEnable"
                 CssClassDisabled="btnDisable" OnMouseOver="btnHover" ClientIDMode="Static" OnClick="RedirectProjectBtnConfirm_Click"
                 OnClientClick="disallowOp('content2');" />
            <cc1:CustomButton ID="RedirectProjectBtnClose" runat="server" CssClass="btnEnable"
                 CssClassDisabled="btnDisable" OnMouseOver="btnHover" ClientIDMode="Static" OnClick="RedirectProjectBtnClose_Click"
                 OnClientClick="disallowOp(content2');" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        $(".chzn-select-deselect").chosen({ allow_single_deselect: true, no_results_text: "Nessun risultato trovato" });
        $(".chzn-select").chosen({ no_results_text: "Nessun risultato trovato" });
    </script>
</asp:Content>