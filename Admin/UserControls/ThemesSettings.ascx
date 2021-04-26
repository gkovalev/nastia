<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ThemesSettings.ascx.cs"
    Inherits="Admin_UserControls_Settings_DesignSettings" %>
<table cellpadding="2" cellspacing="2" width="100%">
    <tr>
        <td>
            <asp:Label ID="lblLoadNew" runat="server" Text="" />
        </td>
        <td>
            <asp:FileUpload ID="ThemeLoad" runat="server" />
        </td>
        <td>
        </td>
    </tr>
    <tr>
        <td colspan="2" style="padding-bottom: 10px;">
            <asp:Button ID="bthAddTheme" runat="server" Text="<%$ Resources:Resource, Admin_ThemesSettings_Load %>"
                OnClick="bthAddTheme_Click" />
        </td>
        <td style="text-align: right">
            <asp:LinkButton runat="server" Text="<%$ Resources:Resource, Admin_ThemesSettings_InstallAll %>"
                OnClick="InstallAll" />
            |
            <asp:LinkButton runat="server" Text="<%$ Resources:Resource, Admin_ThemesSettings_UpdateInstalled %>"
                OnClick="UpdateInstalled" />
            |
            <asp:LinkButton runat="server" Text="<%$ Resources:Resource, Admin_ThemesSettings_DeleteAll %>" OnClick="DeleteAll" />
        </td>
    </tr>
</table>
<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="themesUP">
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
<asp:UpdatePanel ID="themesUP" runat="server">
    <ContentTemplate>
        <asp:Label ID="lblError" runat="server" Text="" Style="color: blue; padding: 10px;
            display: block; border: solid 1px blue; width: 300px; margin: 0 0 10px 0;" Visible="false" />
        <asp:Repeater ID="DataListDesigns" runat="server" OnItemCommand="dlItems_ItemCommand"
            OnItemDataBound="dlItems_ItemDataBound">
            <HeaderTemplate>
                <table width="100%" border="0" cellspacing="0" cellpadding="3" class="grid-main">
                    <tr class="header">
                        <td style="width: 40px; padding-left: 10px;">
                            <%= Resources.Resource.Admin_ThemesSettings_Active %>
                        </td>
                        <td style="width: 35px;">
                            &nbsp;
                        </td>
                        <td>
                            <b>
                                <asp:Label ID="Label15" runat="server" Text="<%$ Resources:Resource, Admin_ThemesSettings_Name %>"></asp:Label></b>
                        </td>
                        <td align="right" style="width: 50px">
                            &nbsp;
                        </td>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr class="row1" style="height: 40px;">
                    <td style="text-align: center;">
                        <asp:ImageButton ID="btnCurrentTheme" runat="server" CausesValidation="false" ImageUrl="~/Admin/images/check_active.png"
                            CssClass="addbtn showtooltip" ToolTip='<%$ Resources:Resource, Admin_ThemesSettings_Current %>' />
                        <asp:ImageButton ID="btnActivate" runat="server" CausesValidation="false" CommandArgument='<%# Eval("Name") %>'
                            CommandName="ApplyTheme" ImageUrl="~/Admin/images/check_noactive.png" CssClass="addbtn showtooltip"
                            ToolTip='<%$ Resources:Resource, Admin_ThemesSettings_Apply %>' />
                    </td>
                    <td>
                        <%# Eval("PreviewImage") == null || string.IsNullOrEmpty(Eval("PreviewImage").ToString()) ? "" : "<img class='imgtooltip' src='images/adv_photo_ico.gif' abbr='" + Eval("PreviewImage")+ "' alt='' style='padding-left:5px;' />"%>
                        <asp:HiddenField ID="hfName" runat="server" Value='<%# Eval("Name") %>' />
                        <asp:HiddenField ID="hfSource" runat="server" Value='<%# Eval("Source") %>' />
                    </td>
                    <td>
                        <asp:Literal runat="server" Text='<%#Eval("Title")%>'></asp:Literal>
                    </td>
                    <td>
                        <asp:ImageButton runat="server" ID="btnAdd" CausesValidation="false" CommandArgument='<%# Eval("Name") %>'
                            CommandName="Add" ImageUrl="~/Admin/images/download.png" CssClass="addbtn showtooltip"
                            ToolTip='<%$ Resources:Resource, Admin_ThemesSettings_Add %>' />
                        <asp:ImageButton runat="server" ID="btnDelete" CausesValidation="false" CommandArgument='<%# Eval("Name") %>'
                            CommandName="Delete" ImageUrl="~/Admin/images/deletebtn.png" CssClass="deletebtn showtooltip"
                            ToolTip='<%$ Resources:Resource, Admin_MasterPageAdminCatalog_Delete %>' />
                        <ajaxToolkit:ConfirmButtonExtender ID="ConfirmButtonExtender2" runat="server" TargetControlID="btnDelete"
                            ConfirmText="<%$ Resources:Resource, Admin_ThemesSettings_Confirmation %>">
                        </ajaxToolkit:ConfirmButtonExtender>
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr class="row2" style="height: 40px;">
                    <td style="text-align: center;">
                        <asp:ImageButton ID="btnCurrentTheme" runat="server" CausesValidation="false" ImageUrl="~/Admin/images/check_active.png"
                            CssClass="addbtn showtooltip" ToolTip='<%$ Resources:Resource, Admin_ThemesSettings_Current %>' />
                        <asp:ImageButton ID="btnActivate" runat="server" CausesValidation="false" CommandArgument='<%# Eval("Name") %>'
                            CommandName="ApplyTheme" ImageUrl="~/Admin/images/check_noactive.png" CssClass="addbtn showtooltip"
                            ToolTip='<%$ Resources:Resource, Admin_ThemesSettings_Apply %>' />
                    </td>
                    <td>
                        <%# Eval("PreviewImage") == null || string.IsNullOrEmpty(Eval("PreviewImage").ToString()) ? "" : "<img class='imgtooltip' src='images/adv_photo_ico.gif' abbr='" + Eval("PreviewImage").ToString() + "' alt=''' style='padding-left:5px;' />"%>
                        <asp:HiddenField ID="hfName" runat="server" Value='<%# Eval("Name") %>' />
                        <asp:HiddenField ID="hfSource" runat="server" Value='<%# Eval("Source") %>' />
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="ltArtNo" Text='<%#Eval("Title")%>'></asp:Literal>
                    </td>
                    <td>
                        <asp:ImageButton runat="server" ID="btnAdd" CausesValidation="false" CommandArgument='<%#  Eval("Title") %>'
                            CommandName="Add" ImageUrl="~/Admin/images/download.png" CssClass="addbtn showtooltip"
                            ToolTip='<%$ Resources:Resource, Admin_ThemesSettings_Add %>' />
                        <asp:ImageButton runat="server" ID="btnDelete" CausesValidation="false" CommandArgument='<%#  Eval("Title") %>'
                            CommandName="Delete" ImageUrl="~/Admin/images/deletebtn.png" CssClass="deletebtn showtooltip"
                            ToolTip='<%$ Resources:Resource, Admin_MasterPageAdminCatalog_Delete %>' />
                        <ajaxToolkit:ConfirmButtonExtender ID="ConfirmButtonExtender2" runat="server" TargetControlID="btnDelete"
                            ConfirmText="<%$ Resources:Resource, Admin_ThemesSettings_Confirmation %>">
                        </ajaxToolkit:ConfirmButtonExtender>
                    </td>
                </tr>
            </AlternatingItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </ContentTemplate>
</asp:UpdatePanel>
