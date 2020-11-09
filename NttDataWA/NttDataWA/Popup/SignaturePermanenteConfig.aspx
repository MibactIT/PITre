<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Popup.Master" AutoEventWireup="true" CodeBehind="SignaturePermanenteConfig.aspx.cs" Inherits="NttDataWA.Popup.SignaturePermanenteConfig" %>

<%@ Register Assembly="NttDatalLibrary" Namespace="NttDatalLibrary" TagPrefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="<%=Page.ResolveClientUrl("~/css/SegnaturaPermanente.css?v=11") %>" rel="Stylesheet" type="text/css" />
    <script src="<%=Page.ResolveClientUrl("~/Scripts/SegnaturaPermanente.js?v=11") %>" type="text/javascript"></script>
</asp:Content>


<%-- Contenuto principale --%>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderContent" runat="server">
    <div id="segnaturaPermanenteMainContent" class="fullHeight fullWidth" style="background-color:#333">
        <!-- Box Settings -->
        <div style="width:30%;">
            <div style="margin: 40px 20px; ">

                <div id="pdfPageInfo">
                    <h3>Informazioni</h3>
                    <p>
                        Posizionare il cursore del mouse in corrispondenza della sezione su cui apporre la segnatura permanente
                    </p>
                    <input name="segnaturaPermanentePosition" runat="server" id="segnaturaPermanentePosition" type="hidden" value="TOP_L" />
                </div>

            </div>
        </div>

        <!-- Box Preview -->
        <div style="width: 68%;">
            <div id="blankDocumentPreview">
                <!--div id="boxSegnaturaPositionTopLeft" class="boxSegnaturePosition selected"></div-->
                <div id="boxSegnaturaPositionTopLeft" class="boxSegnaturePosition"></div>
                <!--div id="boxSegnaturaPositionTopCenter" class="boxSegnaturePosition" style="left: 35%;"></div>
                <div id="boxSegnaturaPositionTopRight" class="boxSegnaturePosition" style="left: 67%;"></div-->
                <div id="boxSegnaturaPositionTopRight" class="boxSegnaturePosition" style="left: 52%;"></div>
                <%--<div id="boxSegnaturaPositionRight" class="boxSegnaturePosition"></div>--%>
                <div id="boxSegnaturaPositionBottomLeft" class="boxSegnaturePosition"></div>
                <!--div id="boxSegnaturaPositionBottomCenter" class="boxSegnaturePosition" style="left: 35%;"></!--div>
                <div id="boxSegnaturaPositionBottomRight" class="boxSegnaturePosition" style="left: 67%;"></div-->
                <div id="boxSegnaturaPositionBottomRight" class="boxSegnaturePosition" style="left: 52%;"></div>
                <%--<div id="boxSegnaturaPositionLeft" class="boxSegnaturePosition"></div>--%>
            </div>
        </div>
    </div>
</asp:Content>

<%--Bottom Bar --%>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceOldersButtons" runat="server">
    <cc1:CustomButton ID="SignaturePermanentBtnConfirm" runat="server" CssClass="btnEnable" CssClassDisabled="btnDisable" OnMouseOver="btnHover" OnClick="SignaturePermanentBtnConfirm_Click" />
    <cc1:CustomButton ID="SignaturePermanentBtnClose" runat="server" CssClass="btnEnable" CssClassDisabled="btnDisable" OnMouseOver="btnHover" OnClick="SignaturePermanentBtnClose_Click" />
</asp:Content>

