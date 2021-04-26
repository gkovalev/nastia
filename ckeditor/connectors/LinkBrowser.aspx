<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LinkBrowser.aspx.cs" Inherits="LinkBrowserPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%=Resources.Resource.Admin_CKEditor_LinkBrowser%></title>
    <script type="text/javascript" src="../../js/jq/jquery-1.7.1.min.js"></script>
    <style type="text/css">
        body
        {
            margin: 0px;
        }
        form
        {
            width: 500px;
            background-color: #E3E3C7;
        }
        h1
        {
            padding: 15px;
            margin: 0px;
            padding-bottom: 0px;
            font-family: Arial;
            font-size: 14pt;
            color: #737357;
        }
        .tab-panel .ajax__tab_body
        {
            background-color: #E3E3C7;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager runat="server">
    </asp:ScriptManager>
    <div>
        <h1>
            <%=Resources.Resource.Admin_CKEditor_LinkBrowser%></h1>
        <table width="100%" cellpadding="10" cellspacing="0" border="1" style="background-color: #F1F1E3;">
            <tr>
                <td valign="top">
                    <%=Resources.Resource.Admin_CKEditor_Folders%>:<br />
                    <asp:DropDownList ID="DirectoryList" runat="server" Style="width: 160px;" OnSelectedIndexChanged="ChangeDirectory"
                        AutoPostBack="true" />
                    <ajaxToolkit:ConfirmButtonExtender ID="ConfirmButtonExtender2" runat="server" TargetControlID="DeleteDirectoryButton"
                        ConfirmText="<%$Resources:Resource, Admin_CKEditor_ConfirmDeleteFolder%>">
                    </ajaxToolkit:ConfirmButtonExtender>
                    <asp:Button ID="DeleteDirectoryButton" runat="server" Text="<%$ Resources:Resource, Admin_CKEditor_Delete %>"
                        OnClick="DeleteFolder" />
                    <asp:HiddenField ID="NewDirectoryName" runat="server" />
                    <asp:Button ID="NewDirectoryButton" runat="server" Text="<%$Resources:Resource, Admin_CKEditor_NewFolder%>"
                        OnClick="CreateFolder" />
                    <br />
                    <br />
                    <asp:Panel ID="SearchBox" runat="server" DefaultButton="SearchButton">
                        <%=Resources.Resource.Admin_CKEditor_Search%>:<br />
                        <asp:TextBox ID="SearchTerms" runat="server" />
                        <%= Resources.Resource.Admin_CKEditor_Go%>
                        <asp:Button ID="SearchButton" runat="server" Text="<%$Resources:Resource, Admin_CKEditor_Go%>"
                            OnClick="Search" UseSubmitBehavior="false" />
                        <br />
                    </asp:Panel>
                    <asp:ListBox ID="ImageList" runat="server" Style="width: 300px; height: 220px;" OnSelectedIndexChanged="SelectImage"
                        AutoPostBack="true" />
                    <br />
                    <asp:HiddenField ID="NewImageName" runat="server" />
                    <asp:Button ID="RenameImageButton" runat="server" Text="<%$Resources:Resource, Admin_CKEditor_Rename%>"
                        OnClick="RenameImage" />
                    <asp:Button ID="DeleteImageButton" runat="server" Text="<%$Resources:Resource, Admin_CKEditor_Delete%>"
                        OnClick="DeleteImage"  />
                        <ajaxToolkit:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="DeleteImageButton"
                        ConfirmText="<%$Resources:Resource, Admin_CKEditor_ConfirmDeleteImage%>">
                    </ajaxToolkit:ConfirmButtonExtender>
                    <br />
                    <br />
                    <%=Resources.Resource.Admin_CKEditor_UploadFile%>: (10 MB max)<br />
                    <asp:FileUpload ID="UploadedImageFile" runat="server" />
                    <asp:Button ID="UploadButton" runat="server" Text="<%$Resources:Resource, Admin_CKEditor_Upload%>" OnClick="Upload" /><br />
                    <br />
                </td>
            </tr>
        </table>
        <center>
            <asp:Button ID="OkButton" runat="server" Text="<%$Resources:Resource, Admin_CKEditor_Ok%>" OnClick="Clear" />
            <asp:Button ID="CancelButton" runat="server" Text="<%$Resources:Resource, Admin_CKEditor_Cancel%>" OnClientClick="window.top.close(); window.top.opener.focus();" OnClick="Clear" />
            <br /><br />
        </center>
    </div>
    </form>
</body>
</html>
