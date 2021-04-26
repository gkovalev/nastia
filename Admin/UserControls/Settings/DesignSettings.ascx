<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DesignSettings.ascx.cs"
    Inherits="Admin_UserControls_Settings_DesignSettings" %>
<table border="0" cellpadding="2" cellspacing="0">
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Localize ID="Localize_Admin_CommonSettings_HeadDesign" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_HeadDesign %>"></asp:Localize></span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr runat="server" visible="False">
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize_Admin_CommonSettings_Themes" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_Themes %>"></asp:Localize>
        </td>
        <td style="vertical-align: top;">
            <asp:DropDownList ID="ddlThemes" runat="server" Width="300px">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize11" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_MainPageMode %>"></asp:Localize>
        </td>
        <td style="vertical-align: top;">
            <asp:DropDownList ID="ddlMainPageMode" runat="server" Width="300px">
                <asp:ListItem Text="<%$ Resources:Resource, Admin_CommonSettings_MainPageModeDefault %>"
                    Value="0" />
<%--                <asp:ListItem Text="<%$ Resources:Resource, Admin_CommonSettings_MainPageModeExtended %>"
                    Value="1" />--%>
                <asp:ListItem Text="<%$ Resources:Resource, Admin_CommonSettings_MainPageModeTwoColumns %>"
                    Value="1" />
                <asp:ListItem Text="<%$ Resources:Resource, Admin_CommonSettings_MainPageModeThreeColumns %>"
                    Value="2" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize10" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_SearchBlockLocation %>"></asp:Localize>
        </td>
        <td style="vertical-align: top;">
            <asp:DropDownList ID="ddlSearchBlockLocation" runat="server" Width="300px">
                <asp:ListItem Text="<%$ Resources:Resource, Admin_CommonSettings_SearchNone %>" Value="0" />
                <asp:ListItem Text="<%$ Resources:Resource, Admin_CommonSettings_SearchTopMenu %>"
                    Value="1" />
                <asp:ListItem Text="<%$ Resources:Resource, Admin_CommonSettings_SearchCatalogMenu %>"
                    Value="2" />
            </asp:DropDownList>
        </td>
    </tr>
        <tr>
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Localize ID="Localize13" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_Carousel%>"></asp:Localize>:</span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize12" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_ShowCarousel %>"></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            &nbsp;<asp:CheckBox ID="CheckBoxCarousel" runat="server" />
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize6" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_CarouselAnimation %>"></asp:Localize>
        </td>
        <td style="vertical-align: top;">
            <asp:DropDownList ID="ddlCarouselAnimation" runat="server" Width="300px">
                <asp:ListItem Text="fade" Value="fade" />
                <asp:ListItem Text="slide" Value="slide" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize7" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_CarouselAnimationSpeed %>"></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:TextBox ID="txtCarouselAnimationSpeed" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize8" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_CarouselAnimationDelay %>"></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:TextBox ID="txtCarouselAnimationDelay" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Localize ID="Localize_Admin_CommonSettings_ZoomView" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_ZoomView %>"></asp:Localize></span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_EnableZoom %>"></asp:Localize>
        </td>
        <td style="vertical-align: top;">
            &nbsp;<asp:CheckBox ID="chkEnableZoom" runat="server" />
        </td>
    </tr>
    <tr>
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Localize ID="Localize_Admin_CommonSettings_View" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_MainPageProcuts%>"></asp:Localize></span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_DisplayMainPageProcuts%>"></asp:Localize>
        </td>
        <td style="vertical-align: top;">
            &nbsp;<asp:CheckBox ID="chkMainPageProducts" runat="server" />
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_MainPageProductsLinesCount%>"></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:TextBox runat="server" ID="txtCountLineOnMainPage" />
        </td>
    </tr>
    <tr>
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Localize ID="Localize5" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_View %>"></asp:Localize></span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize_Admin_CommonSettings_SeeProduct" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_SeeProduct %>"></asp:Localize>
        </td>
        <td style="vertical-align: top;">
            &nbsp;<asp:CheckBox ID="CheckBoxSeeProduct" runat="server" />
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize_Admin_CommonSettings_News" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_News %>"></asp:Localize>
        </td>
        <td style="vertical-align: top;">
            &nbsp;<asp:CheckBox ID="CheckBoxNews" runat="server" />
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize_Admin_CommonSettings_NewsSubscription" runat="server"
                Text="<%$ Resources:Resource, Admin_CommonSettings_NewsSubscription %>"></asp:Localize>
        </td>
        <td style="vertical-align: top;">
            &nbsp;<asp:CheckBox ID="CheckBoxNewsSubscription" runat="server" />
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize_Admin_CommonSettings_StatusComment" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_StatusComment %>"></asp:Localize>
        </td>
        <td style="vertical-align: top;">
            &nbsp;<asp:CheckBox ID="CheckBoxStatusComment" runat="server" />
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize_Admin_CommonSettings_Voting" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_Voting %>"></asp:Localize>
        </td>
        <td style="vertical-align: top;">
            &nbsp;<asp:CheckBox ID="CheckBoxVoting" runat="server" />
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize_Admin_CommonSettings_Currency" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_Currency %>"></asp:Localize>
        </td>
        <td style="vertical-align: top;">
            &nbsp;<asp:CheckBox ID="CheckBoxCurrency" runat="server" />
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize_Admin_CommonSettings_ShowFilter" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_ShowFilter %>"></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            &nbsp;<asp:CheckBox ID="CheckBoxShowFilter" runat="server" /><span id="spanAlert"
                runat="server" visible="False" style="color: red;"><%= Resources.Resource.Admin_CommonSettings_Warning %></span>
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_ShowGiftCrtificateBlock %>"></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            &nbsp;<asp:CheckBox ID="CheckBoxGiftCertificate" runat="server" />
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize4" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_ShowWishList %>"></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            &nbsp;<asp:CheckBox ID="CheckBoxWishList" runat="server" />
        </td>
    </tr>
    <tr>
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize14" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_EnableSocialShareButtons %>"></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            &nbsp;<asp:CheckBox ID="chkEnableSocialShareButtons" runat="server" />
        </td>
    </tr>
</table>
