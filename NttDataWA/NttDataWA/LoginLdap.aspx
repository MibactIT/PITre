<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginLdap.aspx.cs" Inherits="NttDataWA.LoginLdap" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="uc1" %>
<%@ Register Assembly="NttDatalLibrary" Namespace="NttDatalLibrary" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" id="Html" runat="server">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
    <meta http-equiv="cache-control" content="max-age=0" />
    <meta http-equiv="cache-control" content="no-cache" />
    <meta http-equiv="expires" content="0" />
    <meta http-equiv="expires" content="Tue, 01 Jan 1980 1:00:00 GMT" />
    <meta http-equiv="pragma" content="no-cache" />
    <title>Login</title>
    <script language="javaScript" type="text/javascript" src="Scripts/Functions.js"></script>
    <script src="Scripts/jquery-1.8.1.min.js" type="text/javascript"></script>
    <script language="javaScript" type="text/javascript">
        if (!parent.frames.fra_sessionend) {            
            location.href = "./login.htm";
        }

        function sessionend(userid) {
            var frs = parent.frames.fra_sessionend;
            if (frs.document.getElementById('user_id') != null)
                frs.document.getElementById('user_id').value = userid;
        }
    </script>
    <link runat="server" type="text/css" rel="stylesheet" id="CssLayout" />

    <script src="Scripts/chosen.jquery.min.js" type="text/javascript"></script>
    <link href="Css/chosen.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/Functions.js" type="text/javascript"></script>    
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="3600"
            EnablePartialRendering="true">
            <Scripts>
                <asp:ScriptReference Path="~/Scripts/webkit.js" />
            </Scripts>
        </asp:ScriptManager>
        <script language="javascript" type="text/javascript">
            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(beginRequest);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest);
        </script>
        <!-- PopUp Wait-->
        <uc1:ModalPopupExtender ID="mdlPopupWait" runat="server" TargetControlID="Wait" PopupControlID="Wait" BackgroundCssClass="modalBackground" BehaviorID="mdlWait" />
        <div id="Wait" runat="server" class="wait">
            <asp:UpdatePanel ID="pnlUP" runat="server">
                <ContentTemplate>
                    <div class="modalPopup">
                        <asp:Label ID="Loading" runat="server" Visible="false"></asp:Label>
                        <br />
                        <img id="imgLoading" src="Images/common/loading.gif" alt="" />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
