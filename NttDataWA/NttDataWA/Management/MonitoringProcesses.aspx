<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MonitoringProcesses.aspx.cs"
    Inherits="NttDataWA.Management.MonitoringProcesses" MasterPageFile="~/MasterPages/Base.Master" %>

<%@ Register Assembly="NttDatalLibrary" Namespace="NttDatalLibrary" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ajaxpopup2.ascx" TagPrefix="uc" TagName="ajaxpopup2" %>
<%@ Register Src="~/UserControls/Calendar.ascx" TagPrefix="uc6" TagName="Calendar" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/chosen.jquery.min.js" type="text/javascript"></script>
    <link href="../Css/chosen.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .containerTreeView
        {
            clear: both;
            margin-bottom: 10px;
            min-height: 18px;
            margin: 0 0 10px 0;
            text-align: left;
            vertical-align: top;
            max-height: 150px;
            max-width: 100%;
            overflow: auto;
        }
        .TreeSignatureProcess
        {
            padding: 0;
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
        
        
        #content
        {
            float: left;
            width: 99%;
            height: 99%;
            margin: 5px;
            overflow: auto;
        }
        
        .contentResult
        {
            margin-top: 10px;
        }
        
        #contentFilter
        {
            width: 98.5%;
        }
        
        .col2 p
        {
            margin: 0px;
            padding: 0px;
        }
        
        .row3
        {
            clear: both;
            min-height: 25px;
            margin: 0 0 10px 0;
            text-align: left;
            vertical-align: top;
        }
        
        .col-marginSx2
        {
            float: left;
            margin: 0px 5px 0px 5px;
            text-align: left;
        }
        
        .col-marginSx2 p
        {
            margin: 0px;
            padding: 0px;
            font-weight: normal;
            margin-top: 4px;
        }
        .filterAddressbook
        {
            margin: 0px;
            margin-left: 5px;
            background-color: #e4f1f6;
            border: 0px;
            padding-left: 5px;
            padding-right: 5px;
            padding-bottom: 5px;
            padding-top: 5px;
            width: 100%;
            margin-top: 5px;
            border-radius: 5px;
            -ms-border-radius: 5px; /* ie */
            -moz-border-radius: 5px; /* firefox */
            -webkit-border-radius: 5px; /* safari, chrome */
            -o-border-radius: 5px; /* opera */
        }
        
        .gridViewResult
        {
            min-width: 100%;
            overflow: auto;
        }
        .gridViewResult th
        {
            text-align: center;
            white-space: nowrap;
        }
        .gridViewResult td
        {
            text-align: center;
            padding: 5px;
        }
        #gridViewResult tr.selectedrow
        {
            background: #f3edc6;
            color: #333333;
        }
        
        .tbl_rounded tr.Borderrow
        {
            border-top: 2px solid #FFFFFF;
        }
        
        .tbl_rounded td
        {
            background: #fff;
            min-height: 1em;
            border: 1px solid #A8A8A8;
            border-top: 0;
            text-align: center;
        }
        
        .margin-left
        {
            padding-left: 5px;
        }
        .tbl_rounded tr.Borderrow:hover td
        {
            background-color: #b2d7f8;
        }
        .col-marginSx3
        {
            float: left;
            width: 130px;
            margin: 0px 3px 0px 3px;
        }
        .col-marginSx3 p
        {
            margin: 0px;
            padding: 0px;
        }
        .col-marginSx4
        {
            float: left;
            width: 100px;
            margin: 0px 3px 0px 3px;
        }
        .col-marginSx3 p
        {
            margin: 0px;
            padding: 0px;
        }
        .col5
        {
            float: left;
            margin: 0px 30px 0px 0px;
            text-align: left;
        }
    </style>
</asp:Content>
<asp:Content ID="ContentPlaceHolderContent" ContentPlaceHolderID="ContentPlaceHolderContent"
    runat="server">
    <div id="containerTop">
        <div id="containerDocumentTop">
            <div id="containerStandardTop">
                <div id="containerStandardTopSx">
                </div>
                <div id="containerStandardTopCx">
                    <p>
                        <asp:Literal ID="ManagementMonitoringProcesses" runat="server"></asp:Literal></p>
                </div>
                <div id="containerStandardTopDx">
                </div>
            </div>
            <div id="containerStandardBottom">
                <div id="containerProjectCxBottom">
                </div>
            </div>
        </div>
        <div id="containerStandard" runat="server" clientidmode="Static" style="overflow: hidden">
            <div id="content">
                <asp:Panel ID="pnlNoProcesses" runat="server" Visible="false">
                    <div style=" text-align:left; font-size:small; font-weight:bold; padding-top:10px; padding-bottom:10px; padding-left:5px">
                        <asp:Label ID="lblNoProcesses" runat="server"></asp:Label>
                    </div>
                </asp:Panel>
                <asp:UpdatePanel ID="UpFilters" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div id="contentFilter">
                            <fieldset class="filterAddressbook">
                                <asp:Panel ID="pnlFilter" runat="server">
                                    <div class="row">
                                        <%-- ************** PROCESSO ******************** --%>
                                        <div class="containerTreeView">
                                            <asp:UpdatePanel ID="UpPnlProcesses" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:Panel ID="PnlProcesses" runat="server">
                                                        <asp:TreeView ID="TreeProcessSignature" runat="server" ExpandLevel="10" ShowLines="true"
                                                            NodeStyle-CssClass="TreeSignatureProcess_node" SelectedNodeStyle-CssClass="TreeSignatureProcess_selected"
                                                            OnSelectedNodeChanged="TreeSignatureProcess_SelectedNodeChanged" OnTreeNodeCollapsed="TreeSignatureProcess_Collapsed"
                                                            OnTreeNodeExpanded="TreeSignatureProcess_Collapsed" CssClass="TreeSignatureProcess" />
                                                    </asp:Panel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <%-- ************** ID OGGETTO ******************** --%>
                                        <div class="row3">
                                            <asp:UpdatePanel runat="server" ID="UpPnlIdDoc" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <div class="col6">
                                                        <div class="col-marginSx3">
                                                            <p>
                                                                <span class="weight">
                                                                    <asp:Literal runat="server" ID="LtlIdDoc"></asp:Literal>
                                                                </span>
                                                            </p>
                                                        </div>
                                                        <div class="col2">
                                                            <asp:DropDownList ID="ddl_idDoc" runat="server" Width="140px" AutoPostBack="true"
                                                                CssClass="chzn-select-deselect" OnSelectedIndexChanged="ddl_idDoc_SelectedIndexChanged">
                                                                <asp:ListItem Value="0"></asp:ListItem>
                                                                <asp:ListItem Value="1"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col2">
                                                            <asp:Literal runat="server" ID="LtlDaIdDoc"></asp:Literal>
                                                        </div>
                                                        <div class="col4">
                                                            <cc1:CustomTextArea ID="txt_initIdDoc" runat="server" Width="80px" CssClass="txt_input_full onlynumbers"
                                                                CssClassReadOnly="txt_input_full_disabled"></cc1:CustomTextArea>
                                                        </div>
                                                        <div class="col2">
                                                            <asp:Literal runat="server" ID="LtlAIdDoc"></asp:Literal>
                                                        </div>
                                                        <div class="col2">
                                                            <cc1:CustomTextArea ID="txt_fineIdDoc" runat="server" Width="80px" Visible="true"
                                                                CssClass="txt_input_full onlynumbers" CssClassReadOnly="txt_input_full_disabled"></cc1:CustomTextArea>
                                                        </div>
                                                    </div>
                                                    <div class="col2">
                                                        <div class="col-marginSx4">
                                                            <p>
                                                                <span class="weight">
                                                                    <asp:Literal runat="server" ID="LtlObject"></asp:Literal>
                                                                </span>
                                                            </p>
                                                        </div>
                                                        <div class="col4">
                                                            <cc1:CustomTextArea ID="txt_object" runat="server" Width="300px" CssClass="txt_addressBookLeft"
                                                                CssClassReadOnly="txt_addressBookLeft_disabled"></cc1:CustomTextArea>
                                                        </div>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="row3">
                                            <%-- DATA DI AVVIO --%>
                                            <div class="col5">
                                                <div class="col-marginSx3">
                                                    <p>
                                                        <span class="weight">
                                                            <asp:Literal runat="server" ID="LtlStartDate"></asp:Literal>
                                                        </span>
                                                    </p>
                                                </div>
                                                <div class="col2">
                                                    <asp:DropDownList ID="ddl_StartDate" runat="server" AutoPostBack="true" Width="140px"
                                                        OnSelectedIndexChanged="ddl_StartDate_SelectedIndexChanged" CssClass="chzn-select-deselect">
                                                        <asp:ListItem Value="0"></asp:ListItem>
                                                        <asp:ListItem Value="1"></asp:ListItem>
                                                        <asp:ListItem Value="2"></asp:ListItem>
                                                        <asp:ListItem Value="3"></asp:ListItem>
                                                        <asp:ListItem Value="4"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col2">
                                                    <asp:Literal runat="server" ID="LtlDaStartDate"></asp:Literal>
                                                </div>
                                                <div class="col4">
                                                    <cc1:CustomTextArea ID="txt_initStartDate" runat="server" Width="80px" CssClass="txt_textdata datepicker"
                                                        CssClassReadOnly="txt_textdata_disabled"></cc1:CustomTextArea>
                                                </div>
                                                <div class="col2">
                                                    <asp:Literal runat="server" ID="LtlAStartDate"></asp:Literal>
                                                </div>
                                                <div class="col4">
                                                    <cc1:CustomTextArea ID="txt_finedataStartDate" runat="server" Width="80px" CssClass="txt_textdata datepicker"
                                                        CssClassReadOnly="txt_textdata_disabled"></cc1:CustomTextArea>
                                                </div>
                                            </div>
                                            <div class="col2">
                                                <div class="col-marginSx4">
                                                    <p>
                                                        <span class="weight">
                                                            <asp:Literal runat="server" ID="LtlNotesStartup"></asp:Literal>
                                                        </span>
                                                    </p>
                                                </div>
                                                <div class="col4">
                                                    <cc1:CustomTextArea ID="txtNotesSturtup" runat="server" Width="300px" CssClass="txt_addressBookLeft"
                                                        CssClassReadOnly="txt_addressBookLeft_disabled"></cc1:CustomTextArea>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row3">
                                            <%-- DATA CONCLUSIONE --%>
                                            <div class="col6">
                                                <div class="col-marginSx3">
                                                    <p>
                                                        <span class="weight">
                                                            <asp:Literal runat="server" ID="LtlCompletitionDate"></asp:Literal>
                                                        </span>
                                                    </p>
                                                </div>
                                                <div class="col2">
                                                    <asp:DropDownList ID="ddl_CompletitionDate" runat="server" AutoPostBack="true" Width="140px"
                                                        OnSelectedIndexChanged="ddl_CompletitionDate_SelectedIndexChanged" CssClass="chzn-select-deselect">
                                                        <asp:ListItem Value="0"></asp:ListItem>
                                                        <asp:ListItem Value="1"></asp:ListItem>
                                                        <asp:ListItem Value="2"></asp:ListItem>
                                                        <asp:ListItem Value="3"></asp:ListItem>
                                                        <asp:ListItem Value="4"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col2">
                                                    <asp:Literal runat="server" ID="LtlDaCompletitionDate"></asp:Literal>
                                                </div>
                                                <div class="col4">
                                                    <cc1:CustomTextArea ID="txt_initCompletitionDate" runat="server" Width="80px" CssClass="txt_textdata datepicker"
                                                        CssClassReadOnly="txt_textdata_disabled"></cc1:CustomTextArea>
                                                </div>
                                                <div class="col2">
                                                    <asp:Literal runat="server" ID="LtlACompletitionDate"></asp:Literal>
                                                </div>
                                                <div class="col4">
                                                    <cc1:CustomTextArea ID="txt_finedataCompletitionDate" runat="server" Width="80px"
                                                        CssClass="txt_textdata datepicker" CssClassReadOnly="txt_textdata_disabled"></cc1:CustomTextArea>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row3">
                                            <%-- DATA INTERRUZIONE --%>
                                            <div class="col5">
                                                <div class="col-marginSx3">
                                                    <p>
                                                        <span class="weight">
                                                            <asp:Literal runat="server" ID="LtlInterruptionDate"></asp:Literal>
                                                        </span>
                                                    </p>
                                                </div>
                                                <div class="col2">
                                                    <asp:DropDownList ID="ddl_InterruptionDate" runat="server" AutoPostBack="true" Width="140px"
                                                        OnSelectedIndexChanged="ddl_InterruptionDate_SelectedIndexChanged" CssClass="chzn-select-deselect">
                                                        <asp:ListItem Value="0"></asp:ListItem>
                                                        <asp:ListItem Value="1"></asp:ListItem>
                                                        <asp:ListItem Value="2"></asp:ListItem>
                                                        <asp:ListItem Value="3"></asp:ListItem>
                                                        <asp:ListItem Value="4"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col2">
                                                    <asp:Literal runat="server" ID="LtlDaInterruptionDate"></asp:Literal>
                                                </div>
                                                <div class="col4">
                                                    <cc1:CustomTextArea ID="txt_initInterruptionDate" runat="server" Width="80px" CssClass="txt_textdata datepicker"
                                                        CssClassReadOnly="txt_textdata_disabled"></cc1:CustomTextArea>
                                                </div>
                                                <div class="col2">
                                                    <asp:Literal runat="server" ID="LtlAInterruptionDate"></asp:Literal>
                                                </div>
                                                <div class="col4">
                                                    <cc1:CustomTextArea ID="txt_finedataInterruptionDate" runat="server" Width="80px"
                                                        CssClass="txt_textdata datepicker" CssClassReadOnly="txt_textdata_disabled"></cc1:CustomTextArea>
                                                </div>
                                            </div>
                                            <div class="col2">
                                                <div class="col2">
                                                    <p>
                                                        <span class="weight">
                                                            <asp:Literal runat="server" ID="LtlNotesInterruption"></asp:Literal>
                                                        </span>
                                                    </p>
                                                </div>
                                                <div class="col4">
                                                    <cc1:CustomTextArea ID="txtNotesInterruption" runat="server" Width="240px" CssClass="txt_addressBookLeft"
                                                        CssClassReadOnly="txt_addressBookLeft_disabled"></cc1:CustomTextArea>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row3">
                                            <%-- STATO --%>
                                            <div class="col5">
                                                <div class="col-marginSx3">
                                                    <p>
                                                        <span class="weight">
                                                            <asp:Literal runat="server" ID="LtlState"></asp:Literal>
                                                        </span>
                                                    </p>
                                                </div>
                                                <div class="col">
                                                    <asp:CheckBoxList ID="cbxState" runat="server" CssClass="testo_grigio" RepeatDirection="Horizontal">
                                                        <asp:ListItem Value="IN_EXEC" Selected="True" runat="server" id="opIN_EXEC"></asp:ListItem>
                                                        <asp:ListItem Value="STOPPED" Selected="True" runat="server" id="opSTOPPED"></asp:ListItem>
                                                        <asp:ListItem Value="CLOSED" Selected="True" runat="server" id="opCLOSED"></asp:ListItem>
                                                    </asp:CheckBoxList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row3">
                                            <%-- ARRESTATO --%>
                                            <div class="col5">
                                                <div class="col-marginSx3">
                                                    <p>
                                                        <span class="weight">
                                                            <asp:Literal runat="server" ID="Literal1"></asp:Literal>
                                                        </span>
                                                    </p>
                                                </div>
                                                <div class="col">
                                                    <asp:CheckBox Value="TRUNCATED"  Checked="False" runat="server" id="opTRUNCATED" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </fieldset>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="contentResult">
                    <div class="row">
                        <%--Label numero di elementi --%>
                        <asp:UpdatePanel runat="server" ID="UpnlNumeroInstanceStarted" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col">
                                        <div class="p-padding-left">
                                            <p>
                                                <asp:Label runat="server" ID="monitoringProcessesResultCount"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div class="row">
                            <asp:UpdatePanel runat="server" ID="UpPnlGridView" UpdateMode="Conditional" ClientIDMode="Static">
                                <ContentTemplate>
                                    <asp:GridView ID="gridViewResult" runat="server" Width="99%" AllowPaging="false" PageSize="10"
                                        AutoGenerateColumns="false" CssClass="gridViewResult tbl_rounded round_onlyextreme"
                                        HorizontalAlign="Center" ShowHeader="true" ShowHeaderWhenEmpty="true" OnRowDataBound="gridViewResult_RowDataBound"
                                        OnPreRender="gridViewResult_PreRender"
                                        OnRowCommand="gridViewResult_RowCommand">
                                        <RowStyle CssClass="NormalRow" />
                                        <AlternatingRowStyle CssClass="AltRow" />
                                        <PagerStyle CssClass="recordNavigator2" />
                                        <Columns>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="systemIdIstanzaProcesso" Text='<%# Bind("idIstanzaProcesso") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText='<%$ localizeByText:MonitoringProcessesTipo%>' HeaderStyle-Width="3%"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <cc1:CustomImageButton runat="server" ID="BtnDocument" Width="20px" CssClass="clickableRight"
                                                        CommandName="viewLinkObject" ToolTip='<%$ localizeByText:MonitoringProcessesGoToDocument%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible = "false">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="idDocumento" Text='<%# Bind("docNumber") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText='<%$ localizeByText:MonitoringProcessesIdObject%>'>
                                                <ItemTemplate>
                                                    <span class="noLink"><b>
                                                        <asp:LinkButton runat="server" ID="idDocNumProto" Text='<%#this.GetIdDocNumProto((NttDataWA.DocsPaWR.IstanzaProcessoDiFirma) Container.DataItem) %>' CommandName="viewLinkObject"
                                                            CssClass="clickableRight" ToolTip='<%$ localizeByText:MonitoringProcessesGoToDocument%>'></asp:LinkButton>
                                                    </b></span>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText='<%$ localizeByText:MonitoringProcessesObject%>'>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="oggetto" Text='<%#this.GetObject((NttDataWA.DocsPaWR.IstanzaProcessoDiFirma) Container.DataItem) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText='<%$ localizeByText:StatisticsSignatureProcessStartDate%>'
                                                HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <%--
                                                <%# NttDataWA.Utils.dateformat.dateLength(Eval("dataAttivazione").ToString())%>--%>
                                                    <asp:Label runat="server" ID="DtaAvvio" Text='<%# Bind("dataAttivazione") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText='<%$ localizeByText:StatisticsSignatureProcessNotesStartup%>'>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="NotesStartup" Text='<%# Bind("NoteDiAvvio") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText='<%$ localizeByText:StatisticsSignatureProcessCompletitionDate%>'
                                                HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="CompletitionDate" Text='<%#this.GetCompletitionDate((NttDataWA.DocsPaWR.IstanzaProcessoDiFirma) Container.DataItem) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText='<%$ localizeByText:StatisticsSignatureProcessInterruptionDate%>'
                                                HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="InterruptionDate" Text='<%#this.GetInterruptionDate((NttDataWA.DocsPaWR.IstanzaProcessoDiFirma) Container.DataItem) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText='<%$ localizeByText:StatisticsSignatureProcessNotesInterruption%>'>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="NotesInterruption" Text='<%# Bind("MotivoRespingimento") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText='<%$ localizeByText:StatisticsSignatureProcessState%>'
                                                HeaderStyle-Width="16%" ItemStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="State" Text='<%#this.GetState((NttDataWA.DocsPaWR.IstanzaProcessoDiFirma) Container.DataItem) %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:PlaceHolder ID="plcNavigator" runat="server" />
                                    <asp:UpdatePanel ID="upPnlGridIndexes" runat="server" ClientIDMode="Static" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:HiddenField ID="grid_pageindex" runat="server" ClientIDMode="Static" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="ContentButtons" ContentPlaceHolderID="ContentPlaceOldersButtons"
    runat="server">
    <asp:UpdatePanel ID="UpPnlButtons" runat="server" ClientIDMode="Static" UpdateMode="Conditional">
        <ContentTemplate>
            <cc1:CustomButton ID="MonitoringProcessesSearch" runat="server" CssClass="btnEnable"
                CssClassDisabled="btnDisable" OnMouseOver="btnHover" OnClick="MonitoringProcessesSearch_Click" />
            <cc1:CustomButton ID="MonitoringProcessesClearFilter" runat="server" CssClass="btnEnable"
                CssClassDisabled="btnDisable" OnMouseOver="btnHover" OnClick="MonitoringProcessesClearFilter_Click" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
