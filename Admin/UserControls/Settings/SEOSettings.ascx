<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SEOSettings.ascx.cs" Inherits="Admin_UserControls_Settings_SEOSettings" %>
<style type="text/css">
    .style1
    {
        width: 30%;
        height: 28px;
    }
    .style2
    {
        height: 28px;
    }
</style>
<table border="0" cellpadding="2" cellspacing="0">
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <h4 style="display: inline; font-size: 10pt;">
                <asp:Localize ID="Localize5" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_Products%>" /></h4>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr style="background-color: #eff0f1;">
        <td style="width: 30%; height: 26px; text-align: left; vertical-align: top; padding-top: 5px;">
            <asp:Localize ID="Localize10" runat="server" Text="<%$ Resources: Resource, Admin_m_Default_PageTitle %>" />
        </td>
        <td style="vertical-align: top; height: 26px;">
            <asp:TextBox ID="txtProductsHeadTitle" runat="server" Width="384" Text="#STORE_NAME# - #PRODUCT_NAME#" /><br />
        </td>
    </tr>
    <tr>
        <td style="width: 30%; height: 29px; text-align: left; vertical-align: top; padding-top: 3px;">
            <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources: Resource, Admin_m_Default_MetaKeywords %>" />
        </td>
        <td style="vertical-align: top; height: 29px;">
            <asp:TextBox ID="txtProductsMetaKeywords" runat="server" TextMode="MultiLine" Width="384"
                Text="#STORE_NAME# - #PRODUCT_NAME#" /><br />
        </td>
    </tr>
    <tr style="background-color: #eff0f1;">
        <td style="width: 30%; height: 26px; text-align: left; vertical-align: top; padding-top: 3px;">
            <asp:Localize ID="Localize4" runat="server" Text="<%$ Resources: Resource, Admin_m_Default_MetaDescription %>" />
        </td>
        <td style="vertical-align: top; height: 26px;">
            <asp:TextBox ID="txtProductsMetaDescription" runat="server" TextMode="MultiLine"
                Width="384" Text="#STORE_NAME# - #PRODUCT_NAME#" /><br />
            <span style="color: Gray; font-size: 11px;">
                <asp:Literal ID="Literal6" runat="server" Text="<%$ Resources: Resource, Admin_m_Product_UseGlobalVariables %>" /></span>
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <h4 style="display: inline; font-size: 10pt;">
                <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_Categories%>" /></h4>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr style="background-color: #eff0f1;">
        <td style="width: 30%; height: 26px; text-align: left; vertical-align: top; padding-top: 5px;">
            <asp:Localize ID="txtCategoriesTitle" runat="server" Text="<%$ Resources: Resource, Admin_m_Default_PageTitle %>" />
        </td>
        <td style="vertical-align: top; height: 26px;">
            <asp:TextBox ID="txtCategoriesHeadTitle" runat="server" Width="384" Text="#STORE_NAME# - #CATEGORY_NAME#" /><br />
        </td>
    </tr>
    <tr>
        <td style="width: 30%; height: 29px; text-align: left; vertical-align: top; padding-top: 3px;">
            <asp:Localize ID="Localize7" runat="server" Text="<%$ Resources: Resource, Admin_m_Default_MetaKeywords %>" />
        </td>
        <td style="vertical-align: top; height: 29px;">
            <asp:TextBox ID="txtCategoriesMetaKeywords" runat="server" TextMode="MultiLine" Width="384"
                Text="#STORE_NAME# - #CATEGORY_NAME#" /><br />
        </td>
    </tr>
    <tr style="background-color: #eff0f1;">
        <td style="width: 30%; height: 26px; text-align: left; vertical-align: top; padding-top: 3px;">
            <asp:Localize ID="Localize8" runat="server" Text="<%$ Resources: Resource, Admin_m_Default_MetaDescription %>" />
        </td>
        <td style="vertical-align: top; height: 26px;">
            <asp:TextBox ID="txtCategoriesMetaDescription" runat="server" TextMode="MultiLine"
                Width="384" Text="#STORE_NAME# - #CATEGORY_NAME#" /><br />
            <span style="color: Gray; font-size: 11px;">
                <asp:Literal ID="Literal9" runat="server" Text="<%$ Resources: Resource, Admin_m_Category_UseGlobalVariables %>" /></span>
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <h4 style="display: inline; font-size: 10pt;">
                <asp:Localize ID="Localize16" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_HeadNewsSEO%>" /></h4>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr style="background-color: #eff0f1;">
        <td style="width: 30%; height: 26px; text-align: left; vertical-align: top; padding-top: 5px;">
            <asp:Localize ID="Localize_Admin_m_Product_HeadTitle" runat="server" Text="<%$ Resources: Resource, Admin_m_Default_PageTitle %>" />
        </td>
        <td style="vertical-align: top; height: 26px;">
            <asp:TextBox ID="txtNewsHeadTitle" runat="server" Width="384" Text="#STORE_NAME# - #NEWS_NAME#" /><br />
        </td>
    </tr>
    <tr>
        <td style="width: 30%; height: 29px; text-align: left; vertical-align: top; padding-top: 3px;">
            <asp:Localize ID="Localize_Admin_m_Product_MetaKeywords" runat="server" Text="<%$ Resources: Resource, Admin_m_Default_MetaKeywords %>" />
        </td>
        <td style="vertical-align: top; height: 29px;">
            <asp:TextBox ID="txtNewsMetaKeywords" runat="server" TextMode="MultiLine" Width="384"
                Text="#STORE_NAME# - #NEWS_NAME#" /><br />
        </td>
    </tr>
    <tr style="background-color: #eff0f1;">
        <td style="width: 30%; height: 26px; text-align: left; vertical-align: top; padding-top: 3px;">
            <asp:Localize ID="Localize_Admin_m_Product_MetaDescription" runat="server" Text="<%$ Resources: Resource, Admin_m_Default_MetaDescription %>" />
        </td>
        <td style="vertical-align: top; height: 26px;">
            <asp:TextBox ID="txtNewsMetaDescription" runat="server" TextMode="MultiLine" Width="384"
                Text="#STORE_NAME# - #NEWS_NAME#" /><br />
            <span style="color: Gray; font-size: 11px;">
                <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources: Resource, Admin_m_News_UseGlobalVariables %>" /></span>
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <h4 style="display: inline; font-size: 10pt;">
                <asp:Localize ID="Localize6" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_StaticPages%>" /></h4>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr style="background-color: #eff0f1;">
        <td style="width: 30%; height: 26px; text-align: left; vertical-align: top; padding-top: 5px;">
            <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources: Resource, Admin_m_Default_PageTitle %>" />
        </td>
        <td style="vertical-align: top; height: 26px;">
            <asp:TextBox ID="txtStaticPageHeadTitle" runat="server" Width="384" Text="#STORE_NAME# - #PAGENAME#" /><br />
        </td>
    </tr>
    <tr>
        <td style="width: 30%; height: 29px; text-align: left; vertical-align: top; padding-top: 3px;">
            <asp:Localize ID="Localize20" runat="server" Text="<%$ Resources: Resource, Admin_m_Default_MetaKeywords %>" />
        </td>
        <td style="vertical-align: top; height: 29px;">
            <asp:TextBox ID="txtStaticPageMetaKeywords" runat="server" TextMode="MultiLine" Width="384"
                Text="#STORE_NAME# - #PAGENAME#" /><br />
        </td>
    </tr>
    <tr style="background-color: #eff0f1;">
        <td style="width: 30%; height: 26px; text-align: left; vertical-align: top; padding-top: 3px;">
            <asp:Localize ID="Localize11" runat="server" Text="<%$ Resources: Resource, Admin_m_Default_MetaDescription %>" />
        </td>
        <td style="vertical-align: top; height: 26px;">
            <asp:TextBox ID="txtStaticPageMetaDescription" runat="server" TextMode="MultiLine"
                Width="384" Text="#STORE_NAME# - #PAGENAME#" /><br />
            <span style="color: Gray; font-size: 11px;">
                <asp:Literal ID="Literal12" runat="server" Text="<%$ Resources: Resource, Admin_StaticPage_UseGlobalVariables %>" /></span>
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <h4 style="display: inline; font-size: 10pt;">
                <asp:Localize ID="Admin_CommonSettings_HeadAnotherPages" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_HeadAnotherPages%>" /></h4>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr style="background-color: #eff0f1;">
        <td style="width: 30%; height: 26px; text-align: left; vertical-align: top; padding-top: 5px;">
            <asp:Localize ID="Localize_Admin_CommonSettings_Title" runat="server" Text="<%$ Resources: Resource, Admin_m_Default_PageTitle %>" />
        </td>
        <td style="vertical-align: top; height: 26px;">
            <asp:TextBox ID="txtTitle" runat="server" Width="384" Text="#STORE_NAME#" /><br />
        </td>
    </tr>
    <tr>
        <td style="width: 30%; height: 29px; text-align: left; vertical-align: top; padding-top: 3px;">
            <asp:Localize ID="Localize18" runat="server" Text="<%$ Resources: Resource, Admin_m_Default_MetaKeywords %>" />
        </td>
        <td style="vertical-align: top; height: 29px;">
            <asp:TextBox ID="txtMetaKeys" runat="server" TextMode="MultiLine" Width="384" Text="#STORE_NAME#" /><br />
        </td>
    </tr>
    <tr style="background-color: #eff0f1;">
        <td style="text-align: left; vertical-align: top; padding-top: 3px;" class="style1">
            <asp:Localize ID="Localize19" runat="server" Text="<%$ Resources: Resource, Admin_m_Default_MetaDescription %>" />
        </td>
        <td style="vertical-align: top;" class="style2">
            <asp:TextBox ID="txtMetaDescription" runat="server" TextMode="MultiLine" Width="384"
                Text="#STORE_NAME#" /><br />
            <span style="color: Gray; font-size: 11px;">
                <asp:Literal ID="Literal18" runat="server" Text="<%$ Resources: Resource, Admin_UseGlobalVariables %>" /></span>
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <h4 style="display: inline; font-size: 10pt;">
                <asp:Localize ID="Localize15" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_GoogleAnalytics%>" /></h4>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr style="background-color: #eff0f1;">
        <td style="width: 30%; height: 26px; text-align: left; vertical-align: top; padding-top: 5px;">
            <asp:Localize ID="Localize17" runat="server" Text="<%$ Resources: Resource, Admin_CommonSettings_GoogleAnalyticsEnabled %>" />
        </td>
        <td style="vertical-align: top; height: 26px;">
            <asp:CheckBox ID="chbGoogleAnalytics" runat="server" />
        </td>
    </tr>
    <tr style="background-color: #eff0f1;">
        <td style="width: 30%; height: 26px; text-align: left; vertical-align: top; padding-top: 5px;">
            <asp:Localize ID="Localize13" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_GoogleAnalyticsNumer%>" />
            <br />
        </td>
        <td style="vertical-align: top; height: 26px;">
            UA-<asp:TextBox ID="txtGoogleAnalytics" runat="server" class="textBoxClass" />
        </td>
    </tr>
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <h4 style="display: inline; font-size: 10pt;">
                <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_OtherSEO%>" /></h4>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr style="background-color: #eff0f1;">
        <td style="width: 30%; height: 26px; text-align: left; vertical-align: top; padding-top: 5px;">
            <asp:Localize ID="Localize12" runat="server" Text="<%$ Resources:Resource,Admin_SettingsSEO_CustomMetaString %>" />
            <br />
        </td>
        <td style="vertical-align: top; height: 26px;">
            <asp:TextBox ID="txtCustomMetaString" runat="server" class="textBoxClass" />
        </td>
    </tr>
</table>
