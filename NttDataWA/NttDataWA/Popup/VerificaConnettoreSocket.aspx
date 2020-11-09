<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Popup.Master" AutoEventWireup="true" CodeBehind="VerificaConnettoreSocket.aspx.cs" Inherits="NttDataWA.Popup.VerificaConnettoreSocket" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="uc1" %>
<%@ Register Assembly="NttDatalLibrary" Namespace="NttDatalLibrary" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/messager.ascx" TagPrefix="uc" TagName="messager" %>
<asp:Content ID="ContentPlaceHolderHeader" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="ContentPlaceHolderContent" ContentPlaceHolderID="ContentPlaceHolderContent" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div style="width: 99%">
                <uc:messager ID="messager" runat="server" />
            </div>
            <div class="no-applet-ul-conteiner">
                <ul>
                    <li>
                        <asp:Literal ID="ltlVersion" runat="server"></asp:Literal>
                        <asp:Label ID="lblVersion" runat="server"></asp:Label>
                    </li>
                    <li>
                        <asp:Literal ID="ltlLastVersion" runat="server"></asp:Literal>
                        <asp:Label ID="lblLastVesion" runat="server"></asp:Label>
                    </li>
                    <li>
                        <asp:LinkButton ID="LblPath" runat="server" OnClick="Path_Click"></asp:LinkButton>
                    </li>
                </ul>
            </div>
             <div class="no-applet-ul-conteiner">
                 <asp:Label ID="LblPassi" runat="server"></asp:Label>:
                <ul>
                    <li>
                        <asp:Label ID="LblDownload" runat="server"></asp:Label>
                    </li>
                    <li>
                        <asp:Label ID="LblDisinstalla" runat="server"></asp:Label>
                    </li>
                    <li>
                        <asp:Label ID="LblRiavvia" runat="server"></asp:Label>
                    </li>
                    <li>
                        <asp:Label ID="LblInstalla" runat="server"></asp:Label>
                    </li>
                    <li>
                        <asp:Label ID="LblRiavvia1" runat="server"></asp:Label>
                    </li>
                </ul>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    
</asp:Content>
<asp:Content ID="ContentPlaceHolderButtons" ContentPlaceHolderID="ContentPlaceOldersButtons" runat="server">
     <asp:UpdatePanel ID="UpPnlButtons" runat="server" ClientIDMode="Static" UpdateMode="Conditional">
        <ContentTemplate>
            <cc1:CustomButton ID="VerificaConnettoreSocketBtnCancel" runat="server" CssClass="btnEnable" CssClassDisabled="btnDisable"
                OnMouseOver="btnHover" ClientIDMode="Static" OnClick="VerificaConnettoreSocketBtnCancel_Click"/>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
