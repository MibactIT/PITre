<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VisibilitySignatureProcess.aspx.cs"
    Inherits="NttDataWA.Popup.VisibilitySignatureProcess" MasterPageFile="~/MasterPages/Popup.Master" %>

<%@ Register Assembly="NttDatalLibrary" Namespace="NttDatalLibrary" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ajaxpopup2.ascx" TagPrefix="uc" TagName="ajaxpopup2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/Calendar.ascx" TagPrefix="uc6" TagName="Calendar" %>
<asp:Content ID="ContentPlaceHolderHeader" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/chosen.jquery.min.js" type="text/javascript"></script>
    <link href="../Css/chosen.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .tbl_rounded_custom th, .tbl_rounded_custom td.th
        {
            background: url(../Images/Common/table_header_bg.png) repeat-x top left;
            color: #fff;
            font-weight: bold;
            border: 1px solid #d4d4d4;
            padding: 5px;
            vertical-align: top;
            text-align: left;
        }
        
        .tbl_rounded_custom tr.header td, .tbl_rounded_custom tr.header2 td
        {
            background-color: #e1e9f0;
        }
    </style>
    <script type="text/javascript">
        
        function closeAddressBookPopup() {
            $('#btnAddressBookPostback').click();
        }

        function acePopulated(sender, e) {
            var behavior = $find('AutoCompleteExIngressoBIS');
            var target = behavior.get_completionList();
            if (behavior._currentPrefix != null) {
                var prefix = behavior._currentPrefix.toLowerCase();
                var i;
                for (i = 0; i < target.childNodes.length; i++) {
                    var sValue = target.childNodes[i].innerHTML.toLowerCase();
                    if (sValue.indexOf(prefix) != -1) {
                        var fstr = target.childNodes[i].innerHTML.substring(0, sValue.indexOf(prefix));

                        var pstr = target.childNodes[i].innerHTML.substring(fstr.length, fstr.length + prefix.length);

                        var estr = target.childNodes[i].innerHTML.substring(fstr.length + prefix.length, target.childNodes[i].innerHTML.length);

                        target.childNodes[i].innerHTML = fstr + '<span class="selectedWord">' + pstr + '</span>' + estr;
                        try {
                            target.childNodes[i].attributes["_value"].value = fstr + pstr + estr;
                        }
                        catch (ex) {
                            target.childNodes[i].attributes["_value"] = fstr + pstr + estr;
                        }
                    }
                }
            }
        }

        function aceSelected(sender, e) {
            var value = e.get_value();
            if (!value) {

                if (e._item.parentElement && e._item.parentElement.tagName == "LI") {

                    try {
                        value = e._item.parentElement.attributes["_value"].value;
                    }
                    catch (ex1) {
                        value = e._item.parentElement.attributes["_value"];
                    }
                    if (value == undefined || value == null)
                        value = e._item.parentElement.attributes["_value"];
                }
                else if (e._item.parentElement && e._item.parentElement.parentElement.tagName == "LI") {
                    try {
                        value = e._item.parentElement.attributes["_value"].value;
                    }
                    catch (ex1) {
                        value = e._item.parentElement.attributes["_value"];
                    }
                    if (value == undefined || value == null)
                        value = e._item.parentElement.attributes["_value"];
                }
                else if (e._item.parentNode && e._item.parentNode.tagName == "LI") {
                    value = e._item.parentNode._value;
                }
                else if (e._item.parentNode && e._item.parentNode.parentNode.tagName == "LI") {
                    value = e._item.parentNode.parentNode._value;
                }
                else value = "";

            }

            var searchText = $get('<%=TxtDescriptionCorr.ClientID %>').value;
            searchText = searchText.replace('null', '');
            var testo = value;
            var indiceFineCodice = testo.lastIndexOf(')');
            document.getElementById("<%=this.TxtDescriptionCorr.ClientID%>").focus();
            document.getElementById("<%=this.TxtDescriptionCorr.ClientID%>").value = "";
            var indiceDescrizione = testo.lastIndexOf('(');
            var descrizione = testo.substr(0, indiceDescrizione - 1);
            var codice = testo.substring(indiceDescrizione + 1, indiceFineCodice);
            document.getElementById("<%=this.TxtCodeCorr.ClientID%>").value = codice;
            document.getElementById("<%=TxtDescriptionCorr.ClientID%>").value = descrizione;

            document.getElementById("<%=btnRecipient.ClientID%>").click();
            //__doPostBack('UpPnlRecipient', '');
        }

    </script>
</asp:Content>
<asp:Content ID="ContentPlaceHolderContent" ContentPlaceHolderID="ContentPlaceHolderContent"
    ClientIDMode="static" runat="server">
<div id="container" style="padding: 20px">
    <asp:UpdatePanel ID="UpdPnlContainer" UpdateMode="Conditional" runat="server" ClientIDMode="Static">
        <ContentTemplate>
                <asp:UpdatePanel ID="UpdPnlCorr" UpdateMode="Conditional" runat="server" ClientIDMode="Static">
                    <ContentTemplate>
                        <div class="row" style="width: 100%">
                            <div class="colHalf" style="width: 35%">
                                <strong>
                                    <asp:Literal ID="LitVisibilitySignatureProcessCorr" runat="server" /></strong>
                                <cc1:CustomTextArea ID="TxtCodeCorr" runat="server" CssClass="txt_addressBookLeft"
                                    autocomplete="off" AutoPostBack="true"
                                    Width="40%" Style="margin-left: 10px" OnTextChanged="TxtCode_OnTextChanged" CssClassReadOnly="txt_addressBookLeft_disabled"
                                    AutoCompleteType="Disabled" onchange="disallowOp('ContentPlaceHolderContent');">
                                </cc1:CustomTextArea>
                            </div>
                            <div class="colHalf" style="width: 55%;">
                                <asp:HiddenField ID="idCorr" runat="server" />
                                <cc1:CustomTextArea ID="TxtDescriptionCorr" runat="server" CssClass="txt_addressBookRight"
                                    CssClassReadOnly="txt_addressBookRight_disabled" autocomplete="off" AutoCompleteType="Disabled"></cc1:CustomTextArea>
                                <uc1:AutoCompleteExtender runat="server" ID="RapidCorr" TargetControlID="TxtDescriptionCorr"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="single_item"
                                    CompletionListHighlightedItemCssClass="single_item_hover" ServiceMethod="GetListaCorrispondentiVeloce_trasmD"
                                    MinimumPrefixLength="3" CompletionInterval="1000" EnableCaching="true" CompletionSetCount="20"
                                    DelimiterCharacters=";" ServicePath="~/AjaxProxy.asmx" ShowOnlyCurrentWordInCompletionListItem="true"
                                    UseContextKey="true" OnClientItemSelected="aceSelected" BehaviorID="AutoCompleteExIngressoBIS"
                                    OnClientPopulated="acePopulated">
                                </uc1:AutoCompleteExtender>
                                <asp:Button ID="btnRecipient" runat="server" Text="vai" Style="display: none;" />
                            </div>
                            <div class="col-right-no-margin2">
                                <cc1:CustomImageButton runat="server" ID="BtnAddressBook" ImageUrl="../Images/Icons/address_book.png"
                                    OnMouseOutImage="../Images/Icons/address_book.png" OnMouseOverImage="../Images/Icons/address_book_hover.png"
                                    CssClass="clickableLeft" ImageUrlDisabled="../Images/Icons/address_book_disabled.png"
                                    OnClick="BtnAddressBook_Click" />
                                <cc1:CustomImageButton ID="BtnAddRole" runat="server" CssClass="clickableLeft" Visible="true"
                                    ImageUrl="../Images/Icons/add_version.png" OnMouseOutImage="../Images/Icons/add_version.png"
                                    OnClick="BtnAddRole_Click" OnMouseOverImage="../Images/Icons/add_version_hover.png" Enabled="false"
                                    ImageUrlDisabled="../Images/Icons/add_version_disabled.png" ToolTip='<%$ localizeByText:VisibilitySignaturedAddVisibilityTooltip%>' />
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel runat="server" ID="UpnlGrid" ClientIDMode="Static" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="row" style="padding-top: 15px">
                            <asp:GridView ID="GridViewResult" runat="server" AllowSorting="false" AllowPaging="true"
                                    AutoGenerateColumns="false" CssClass="tbl_rounded_custom round_onlyextreme" PageSize="15"
                                    HorizontalAlign="Center"  ShowHeader="true" ShowHeaderWhenEmpty="true"
                                    Width="100%" OnPreRender="GridViewResult_PreRender" OnRowCreated="GridViewResult_RowDataBound"
                                     OnPageIndexChanging="GridViewResult_PageIndexChanging"
                                    >
                                    <RowStyle CssClass="NormalRow" />
                                    <AlternatingRowStyle CssClass="AltRow" />
                                    <PagerStyle CssClass="recordNavigator2" />
                                <Columns>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="systemIdCorr" Text='<%# Bind("systemId") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText='<%$ localizeByText:VisibilitySignatureProcessRole%>'
                                        HeaderStyle-Width="85%">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblDescriptionRole" Text='<%#this.GetDescriptionCorr((NttDataWA.DocsPaWR.Corrispondente) Container.DataItem) %>'  />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="10px" ItemStyle-Width="10px" ItemStyle-CssClass="grdList_del"
                                        ItemStyle-HorizontalAlign="Center" HeaderText='<%$ localizeByText:VisibilitySignatureProcessRemove%>'>
                                        <ItemTemplate>
                                            <cc1:CustomImageButton ID="ImgDeleteVisibility" runat="server" ImageAlign="Middle"
                                                ImageUrl="../Images/Icons/delete.png" OnMouseOverImage="../Images/Icons/delete_hover.png" ToolTip='<%$ localizeByText:VisibilitySignaturedDeleteVisibilityTooltip%>'
                                                OnMouseOutImage="../Images/Icons/delete.png" CssClass="clickableLeft" OnClick="ImgDeleteVisibility_Click"  OnClientClick="disallowOp('ContentPlaceHolderContent');" />
                                        </ItemTemplate>
                                        <ItemStyle />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:PlaceHolder ID="plcNavigator" runat="server" />
                            <asp:HiddenField ID="grid_pageindex" runat="server" ClientIDMode="Static" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
        </ContentTemplate>
    </asp:UpdatePanel>
                </div>
</asp:Content>
<asp:Content ID="ContentButtons" ContentPlaceHolderID="ContentPlaceOldersButtons"
    runat="server">
    <asp:UpdatePanel ID="UpPnlButtons" runat="server" UpdateMode="Conditional" ClientIDMode="Static">
        <ContentTemplate>
            <%--            <cc1:CustomButton ID="BtnConfirm" runat="server" CssClass="btnEnable" CssClassDisabled="btnDisable"
                OnMouseOver="btnHover" ClientIDMode="Static" OnClick="BtnConfirm_Click" />--%>
            <cc1:CustomButton ID="BtnClose" runat="server" CssClass="btnEnable" CssClassDisabled="btnDisable"
                OnMouseOver="btnHover" ClientIDMode="Static" OnClick="BtnClose_Click" />
            <%--<asp:Button ID="btnObjectPostback" runat="server" CssClass="hidden" ClientIDMode="Static"
                OnClick="btnObjectPostback_Click" />
            <asp:Button ID="btnAddressBookPostback" runat="server" CssClass="hidden" ClientIDMode="Static"
                OnClick="btnAddressBookPostback_Click" OnClientClick="disallowOp('ContentPlaceHolderContent')" />--%>
            <asp:Button ID="btnAddressBookPostback" runat="server" CssClass="hidden" ClientIDMode="Static"
                OnClick="btnAddressBookPostback_Click" OnClientClick="disallowOp('ContentPlaceHolderContent')" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
