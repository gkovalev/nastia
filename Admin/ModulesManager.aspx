<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ModulesManager.aspx.cs" Inherits="Admin_ModulesManager"
    MasterPageFile="MasterPageAdmin.master" %>

<%@ Import Namespace="Resources" %>
<asp:Content ID="contentMain" runat="server" ContentPlaceHolderID="cphMain">
    <table cellpadding="0" cellspacing="0" width="100%">
        <tbody>
            <tr>
                <td style="width: 72px;">
                    <img src="images/orders_ico.gif" alt="" />
                </td>
                <td>
                    <asp:Label ID="lblHead" CssClass="AdminHead" runat="server" Text="<%$ Resources:Resource, Admin_ModuleManager_Header %>"></asp:Label><br />
                    <asp:Label ID="lblSubHead" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_ModuleManager_SubHeader %>"></asp:Label>
                </td>
                <td style="vertical-align: bottom; padding-right: 10px">
                    <div style="float: right; padding-right: 10px">
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <div id="inprogress">
                <div id="curtain" class="opacitybackground">
                    &nbsp;</div>
                <div class="loader">
                    <table width="100%" style="font-weight: bold; text-align: center;">
                        <tbody>
                            <tr>
                                <td align="center">
                                    <img src="images/ajax-loader.gif" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" style="color: #0D76B8;">
                                    <asp:Localize ID="Localize_Admin_Properties_PleaseWait" runat="server" Text="<%$ Resources:Resource, Admin_Properties_PleaseWait %>"></asp:Localize>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:ListView ID="lvModules" runat="server" ItemPlaceholderID="itemPlaceHolder" OnItemCommand="lvModules_ItemCommand">
        <LayoutTemplate>
            <table width="100%" border="0" cellspacing="0" cellpadding="3" class="grid-main">
                <tr class="header leftHeader">
                    <th style="width: 35px;">
                        &nbsp;
                    </th>
                    <th>
                        <asp:Literal ID="Literal1" runat="server" Text='<%$ Resources :Resource, Admin_ModulesManager_ModuleName %>'></asp:Literal>
                    </th>
                    <th>
                        <asp:Literal ID="Literal2" runat="server" Text='<%$ Resources :Resource, Admin_ModulesManager_Version %>'></asp:Literal>
                    </th>
                    <th>
                        <asp:Literal ID="Literal3" runat="server" Text='<%$ Resources :Resource, Admin_ModulesManager_ModuleCost %>'></asp:Literal>
                    </th>
                    <th>
                        &nbsp;
                    </th>
                </tr>
                <tr id="itemPlaceHolder" runat="server">
                </tr>
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr class="row1" style="height: 35px;">
                <td style="width: 35px;">
                    &nbsp;
                </td>
                <td>
                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                    <asp:HiddenField ID="hfLastVersion" runat="server" Value='<%# Eval("Version") %>' />
                    <asp:HiddenField ID="hfId" runat="server" Value='<%# Eval("Id") %>' />
                </td>
                <td>
                    <asp:Label ID="lblLastVersion" runat="server" Visible='<%# !string.IsNullOrEmpty(Convert.ToString(Eval("Version")))%>'><%# Eval("Version") %></asp:Label>
                    <asp:Label ID="lblCurrentVersion" runat="server" Visible='<%# !string.IsNullOrEmpty(Convert.ToString(Eval("CurrentVersion")))%>'>(<%= Resource.Admin_ModulesManager_Installed %>: <%# Eval("CurrentVersion") %>)</asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblCost" runat="server"><%# Convert.ToDecimal(Eval("Price")) != 0 ? String.Format("{0:##,##0.##}", Eval("Price")) + " " + Eval("Currency") : Resources.Resource.Admin_Modules_FreeCost%></asp:Label><br />
                </td>
                <td>
                    <asp:ImageButton runat="server" ID="btnDelete" CausesValidation="false" CommandArgument='<%# Eval("StringId") %>'
                        CommandName="Uninstall" ImageUrl="~/Admin/images/delete.png" CssClass="deletebtn showtooltip"
                        ToolTip='<%$ Resources:Resource, Admin_ModulesManager_Delete %>' Visible='<%# Convert.ToBoolean(Eval("IsInstall")) %>' />
                     <asp:Button runat="server" ID="btnInstall" CausesValidation="false" CommandArgument='<%# Eval("StringId") %>'
                        CommandName="Install" ImageUrl="~/Admin/images/download_module.png" CssClass="deletebtn showtooltip" Text="Install"
                        ToolTip='<%$ Resources:Resource, Admin_ModulesManager_Install %>' Visible='<%# !Convert.ToBoolean(Eval("IsInstall")) && Convert.ToBoolean(Eval("IsLocalVersion"))%>' />
                    <%--<asp:Button ID="btnBuyModule" runat="server" Text="Buy" Visible='<%# !Convert.ToBoolean(Eval("Active")) && !Convert.ToBoolean(Eval("IsLocalVersion"))%>' />--%>
                    <asp:ImageButton ID="btnInstallFLastVersion" runat="server" AlternateText="Install From server"
                        ImageUrl="~/Admin/images/download_module.png" CommandArgument='<%# Eval("StringId") %>'
                        CommandName="InstallLastVersion" ToolTip='<%$ Resources : Resource , Admin_Modules_Update%>' />
                    <%--<asp:ImageButton ID="ImageButton1" runat="server" AlternateText="Install From server"
                        ImageUrl="~/Admin/images/download_module.png" CommandArgument="StringId" CommandName="InstallLastVersion"
                        Visible='<%# !string.Equals(Convert.ToString(Eval("Version")),Convert.ToString(Eval("CurrentVersion"))) && Convert.ToBoolean(Eval("Active"))%>' />--%>
                    <ajaxToolkit:ConfirmButtonExtender ID="ConfirmButtonExtender2" runat="server" TargetControlID="btnDelete"
                        ConfirmText="<%$ Resources:Resource, Admin_ThemesSettings_Confirmation %>">
                    </ajaxToolkit:ConfirmButtonExtender>
                </td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr class="row2" style="height: 35px;">
                <td style="width: 35px;">
                    &nbsp;
                </td>
                <td>
                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                    <asp:HiddenField ID="hfLastVersion" runat="server" Value='<%# Eval("Version") %>' />
                    <asp:HiddenField ID="hfId" runat="server" Value='<%# Eval("Id") %>' />
                </td>
                <td>
                    <asp:Label ID="lblLastVersion" runat="server" Visible='<%# !string.IsNullOrEmpty(Convert.ToString(Eval("Version")))%>'><%# Eval("Version") %></asp:Label>
                    <asp:Label ID="lblCurrentVersion" runat="server" Visible='<%# !string.IsNullOrEmpty(Convert.ToString(Eval("CurrentVersion")))%>'>(<%= Resource.Admin_ModulesManager_Installed %>: <%# Eval("CurrentVersion") %>)</asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblCost" runat="server"><%# Convert.ToDecimal(Eval("Price")) != 0 ? String.Format("{0:##,##0.##}", Eval("Price")) + " " + Eval("Currency") : Resources.Resource.Admin_Modules_FreeCost%></asp:Label><br />
                </td>
                <td>
                    <asp:ImageButton runat="server" ID="btnDelete" CausesValidation="false" CommandArgument='<%# Eval("StringId") %>'
                        CommandName="Uninstall" ImageUrl="~/Admin/images/delete.png" CssClass="deletebtn showtooltip"
                        ToolTip='<%$ Resources:Resource, Admin_ModulesManager_Delete %>' Visible='<%# Convert.ToBoolean(Eval("IsInstall")) %>' />
                    <asp:Button runat="server" ID="btnInstall" CausesValidation="false" CommandArgument='<%# Eval("StringId") %>'
                        CommandName="Install" ImageUrl="~/Admin/images/download_module.png" CssClass="deletebtn showtooltip" Text="Install"
                        ToolTip='<%$ Resources:Resource, Admin_ModulesManager_Install %>' Visible='<%# !Convert.ToBoolean(Eval("IsInstall")) && Convert.ToBoolean(Eval("IsLocalVersion"))%>' />
                    <%--<asp:Button ID="btnBuyModule" runat="server" Text="Buy" Visible='<%# !Convert.ToBoolean(Eval("Active")) && !Convert.ToBoolean(Eval("IsLocalVersion"))%>' />--%>
                    <asp:ImageButton ID="btnInstallFLastVersion" runat="server" AlternateText="Install From server"
                        ImageUrl="~/Admin/images/download_module.png" CommandArgument='<%# Eval("StringId") %>'
                        CommandName="InstallLastVersion" ToolTip='<%$ Resources : Resource , Admin_Modules_Update%>' />
                    <%--<asp:ImageButton ID="ImageButton1" runat="server" AlternateText="Install From server"
                        ImageUrl="~/Admin/images/download_module.png" CommandArgument="StringId" CommandName="InstallLastVersion"
                        Visible='<%# !string.Equals(Convert.ToString(Eval("Version")),Convert.ToString(Eval("CurrentVersion"))) && Convert.ToBoolean(Eval("Active"))%>' />--%>
                    <ajaxToolkit:ConfirmButtonExtender ID="ConfirmButtonExtender2" runat="server" TargetControlID="btnDelete"
                        ConfirmText="<%$ Resources:Resource, Admin_ThemesSettings_Confirmation %>">
                    </ajaxToolkit:ConfirmButtonExtender>
                </td>
            </tr>
        </AlternatingItemTemplate>
    </asp:ListView>
</asp:Content>
