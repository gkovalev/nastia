<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GeneralSettings.ascx.cs"
    Inherits="Admin_UserControls_Settings_GeneralSettings" %>
<%@ Register Src="~/UserControls/LogoImage.ascx" TagName="Logo" TagPrefix="adv" %>
<%@ Register Src="~/UserControls/MasterPage/Favicon.ascx" TagName="Favicon" TagPrefix="adv" %>
<table border="0" cellpadding="2" cellspacing="0">
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Localize ID="lzGeneral" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_HeadGeneral%>"></asp:Localize></span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize_Admin_CommonSettings_LogoImage" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_LogoImage %>"></asp:Localize>
            <br />
            <span style="color: Gray; font-size: 11px;">
                <asp:Literal ID="Literal6" runat="server" Text="<%$ Resources: Resource, Admin_CommonSettings_LogoImageSize %>" /></span>
        </td>
        <td style="vertical-align: top">
            <asp:Panel ID="pnlLogo" runat="server" Width="100%">
                <div style="border-width: 1px; border-color: gray; border-style: solid; width: 308px;
                    height: 100px; overflow: auto;">
                    <adv:Logo ID="Logo" EnableHref="False" CssClassImage="CommonSettingsImage" runat="server" />
                </div>
                <asp:Button ID="DeleteLogo" runat="server" Text="<%$ Resources:Resource, Admin_Delete%>"
                    OnClick="DeleteLogo_Click" />
            </asp:Panel>
            <asp:FileUpload ID="fuLogoImage" runat="server" Height="20px" Width="308px" BackColor="White" />
            <br />
            <br />
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize_Admin_CommonSettings_Favicon" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_Favicon %>"></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top">
            <asp:Panel ID="pnlFavicon" runat="server" Width="100%">
                <div class="dvFaviIco">
                    <adv:Favicon ID="Favicon" runat="server" GetOnlyImage="true" CssClassImage="imgFaviIco" />
                </div>
                <asp:Button ID="DeleteFavicon" runat="server" Text="<%$ Resources:Resource, Admin_Delete%>"
                    OnClick="DeleteFavicon_Click" />
            </asp:Panel>
            <asp:FileUpload ID="fuFaviconImage" runat="server" Height="20px" Width="308px" BackColor="White" /><br />
            <span style="color: Gray;">
                <asp:Literal runat="server" ID="ltFaviconInvalid" Text="<%$ Resources: Resource, Admin_CommonSettings_FaviconFormat %>"></asp:Literal></span>
            <br />
            <br />
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize_Admin_CommonSettings_ShopURL" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_ShopURL %>"></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:TextBox ID="txtShopURL" runat="server" class="textBoxClass"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize4" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_ShopURL_FB %>"></asp:Localize><br />
        </td>
        <td style="vertical-align: top;">
            <asp:Label runat="server" ID="lSocialLinkFb" />
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_ShopURL_VK %>"></asp:Localize><br />
        </td>
        <td style="vertical-align: top;">
            <asp:Label runat="server" ID="lSocialLinkVk" />
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize_Admin_CommonSettings_ShopName" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_ShopName %>"></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top">
            <asp:TextBox ID="txtShopName" runat="server" class="textBoxClass"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize_Admin_CommonSettings_ImgAlt" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_ImgAlt %>"></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:TextBox ID="txtImageAlt" class="textBoxClass" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr id="trChangeAdminPass" runat="server">
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize_Admin_CommonSettings_ChangeAdminPassword" runat="server"
                Text="<%$ Resources:Resource, Admin_CommonSettings_ChangeAdminPassword %>"></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top">
            <a id="lbChangeAdminPassword" href="javascript:void(0);" onclick="$('#tblChangeAdminPassword').show();$('#lbChangeAdminPassword').hide();">
                <%=Resources.Resource.Admin_CommonSettings_Change %></a>
            <asp:CompareValidator ID="cvPasswords" runat="server" ErrorMessage="<%$ Resources:Resource, Admin_CommonSettings_ConfirmFail %>"
                ControlToValidate="txtConfirmPassword" ControlToCompare="txtPassword" Display="Dynamic"></asp:CompareValidator>
            <table id="tblChangeAdminPassword" style="display: none">
                <tbody>
                    <tr>
                        <td>
                            <asp:Localize ID="Localize_Admin_CommonSettings_Password" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_Password %>"></asp:Localize>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtPassword" TextMode="Password" ValidationGroup="password"></asp:TextBox><asp:RequiredFieldValidator
                                ControlToValidate="txtPassword" ID="rfvPassword" runat="server" ErrorMessage="*" ValidationGroup="password"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Localize ID="Localize_Admin_CommonSettings_PasswordConfirm" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_PasswordConfirm %>"></asp:Localize>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtConfirmPassword" TextMode="Password" ValidationGroup="password"></asp:TextBox><asp:RequiredFieldValidator
                                ControlToValidate="txtConfirmPassword" ID="RequiredFieldValidator2" runat="server"
                                ErrorMessage="*" ValidationGroup="password"></asp:RequiredFieldValidator>
                            <br />
                            <br />
                            <asp:Button runat="server" ID="btnChangeAdminPassword" Text="<%$ Resources:Resource, Admin_CommonSettings_Change %>"
                                OnClick="btnChangeAdminPassword_Click"  ValidationGroup="password"/>
                            <input type="button" id="btnCancelChangingAdminPassword" value="<%= Resources.Resource.Admin_Cancel %>"
                                onclick="$('#tblChangeAdminPassword').hide();$('#lbChangeAdminPassword').show();" />
                            <br />
                            <br />
                        </td>
                    </tr>
                </tbody>
            </table>
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize_Admin_CommonSettings_Language" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_Language %>"></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:DropDownList ID="ddlLanguage" runat="server" class="textBoxClass" OnSelectedIndexChanged="ddlLanguage_SelectedIndexChanged">
                <asp:ListItem Text="<%$ Resources:Resource, Global_Language_Russian %>" Value="0"></asp:ListItem>
                <asp:ListItem Text="<%$ Resources:Resource, Global_Language_English %>" Value="1"></asp:ListItem>
            </asp:DropDownList>
            <asp:HiddenField runat="server" ID="hfLanguage" />
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize_Admin_CommonSettings_Format" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_Format %>"></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:TextBox ID="txtFormat" class="textBoxClass" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize14" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_ShortFormat %>"></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:TextBox ID="txtShortFormat" class="textBoxClass" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize10" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_SalerCountry %>"></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:SqlDataSource SelectCommand="SELECT CountryID, CountryName FROM [Customers].[Country]"
                ID="sdsCountry" runat="server" OnInit="sds_Init"></asp:SqlDataSource>
            <asp:DropDownList runat="server" CssClass="textBoxClass" OnSelectedIndexChanged="ddlCountry_SelectedChanged"
                AutoPostBack="true" DataSourceID="sdsCountry" ID="ddlCountry" DataTextField="CountryName"
                DataValueField="CountryID">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize11" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_SalerRegion %>"></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:SqlDataSource SelectCommand="SELECT RegionID, RegionName FROM [Customers].[Region] WHERE [CountryID] = @CountryID"
                ID="sdsRegion" runat="server" OnInit="sds_Init">
                <SelectParameters>
                    <asp:ControlParameter Name="CountryID" Type="Int32" ControlID="ddlCountry" PropertyName="SelectedValue" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:DropDownList runat="server" CssClass="textBoxClass" ID="ddlRegion" DataSourceID="sdsRegion"
                DataValueField="RegionID" DataTextField="RegionName">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize23" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_City %>"></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:TextBox class="textBoxClass" ID="txtCity" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize22" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_Phone %>"></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:TextBox class="textBoxClass" ID="txtPhone" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_EnableUsersOnline %>"></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:CheckBox runat="server" ID="ChbEnableUserOnline" />
        </td>
    </tr>
</table>
<script type="text/javascript">
    Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(
        function () {
            $("#lbChangeAdminPassword").closest("div.tab-content").addClass("selected");
            tabInit();
        }
    );
</script>
