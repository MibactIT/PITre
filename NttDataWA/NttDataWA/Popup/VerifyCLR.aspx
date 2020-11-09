<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Popup.Master" AutoEventWireup="true" CodeBehind="VerifyCLR.aspx.cs" Inherits="NttDataWA.Popup.VerifyCLR" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="uc1" %>
<%@ Register Assembly="NttDatalLibrary" Namespace="NttDatalLibrary" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .container
        {
            width: 95%;
            margin: 0 auto;
        }
        .message {text-align: left; line-height: 2em; width: 90%; min-height: 100px; margin: 80px auto 0 auto;}
        .message img {float: left; display: block; margin: 0 20px 20px 0;}
    </style>
    <script src="../Scripts/chosen.jquery.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderContent" runat="server">
<div class="container">
    <asp:UpdatePanel ID="UpPnlMessage" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:PlaceHolder ID="plcDate" Visible="false" runat="server">
                <div style=" padding-top:10px">
                    <div class="col2">
                        <asp:Literal runat="server" ID="LtlDateCheck"></asp:Literal>
                    </div>
                    <div class="col4">
                        <cc1:CustomTextArea ID="txt_DateCheck" runat="server" Width="80px" CssClass="txt_textdata datepicker"
                            CssClassReadOnly="txt_textdata_disabled"></cc1:CustomTextArea>
                    </div>
                </div>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="plcMessage" runat="server">
                <div class="message"  style=" float:left">
                    <asp:Literal ID="litMessage" runat="server" />
                </div>
            </asp:PlaceHolder>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceOldersButtons" runat="server">
    <asp:UpdatePanel ID="UpPnlButtons" runat="server" UpdateMode="Conditional" ClientIDMode="Static">
        <ContentTemplate>
        <cc1:CustomButton ID="BtnCheck" runat="server" CssClass="btnEnable" CssClassDisabled="btnDisable"
                OnMouseOver="btnHover" ClientIDMode="Static" OnClick="BtnCheck_Click" Visible="false" OnClientClick="disallowOp('Content2');" />
            <cc1:CustomButton ID="BtnClose" runat="server" CssClass="btnEnable" CssClassDisabled="btnDisable"
                OnMouseOver="btnHover" ClientIDMode="Static" OnClick="BtnClose_Click" OnClientClick="disallowOp('Content2');" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
