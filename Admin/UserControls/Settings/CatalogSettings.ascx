<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CatalogSettings.ascx.cs"
    Inherits="Admin_UserControls_Settings_CatalogSettings" %>
<table border="0" cellpadding="2" cellspacing="0">
    <tr class="rowPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Localize ID="Localize_Admin_CommonSettings_HeadCatalog" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_HeadCatalog %>"></asp:Localize></span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Localize ID="Localize_Admin_CommonSettings_ProductPerPage" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_ProductPerPage %>"></asp:Localize>
            <br />
        </td>
        <td>
            &nbsp;<asp:TextBox ID="txtProdPerPage" runat="server" class="shortTextBoxClass"></asp:TextBox>
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Localize ID="Localize_Admin_CommonSettings_DefaultCurrency" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_DefaultCurrency %>"></asp:Localize>
            <br />
        </td>
        <td>
            &nbsp;<asp:DropDownList ID="ddlDefaultCurrency" runat="server" class="shortTextBoxClass">
            </asp:DropDownList>
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Localize ID="Localize9" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_ShowProductsCount %>"></asp:Localize>
        </td>
        <td>
            <asp:CheckBox ID="cbShowProductsCount" runat="server" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Localize ID="Localize2" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_EnableProductRating %>"></asp:Localize>
        </td>
        <td>
            <asp:CheckBox ID="cbEnableProductRating" runat="server" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Localize ID="Localize4" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_EnableCompareProducts %>"></asp:Localize>
        </td>
        <td>
            <asp:CheckBox ID="cbEnableCompareProducts" runat="server" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Localize ID="Localize_Admin_CommonSettings_AllowChangeCatalogView" runat="server"
                    Text="<%$ Resources:Resource, Admin_CommonSettings_AllowChangeCatalogView %>"></asp:Localize></span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Localize ID="Localize_Admin_CommonSettings_EnableCatalogViewChange" runat="server"
                Text="<%$ Resources:Resource, Admin_CommonSettings_EnableCatalogViewChange %>"></asp:Localize>
        </td>
        <td>
            <asp:CheckBox ID="cbEnableCatalogViewChange" runat="server" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Localize ID="Localize_Admin_CommonSettings_EnableSearchViewChange" runat="server"
                Text="<%$ Resources:Resource, Admin_CommonSettings_EnableSearchViewChange %>"></asp:Localize>
        </td>
        <td>
            <asp:CheckBox ID="cbEnableSearchViewChange" runat="server" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Localize ID="Localize_Admin_CommonSettings_Marketing" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_Marketing %>"></asp:Localize></span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_BlockOne %>"></asp:Localize>
        </td>
        <td>
            <asp:TextBox ID="txtBlockOne" runat="server" Width="200px" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Localize ID="Localize3" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_BlockTwo %>"></asp:Localize>
        </td>
        <td>
            <asp:TextBox ID="txtBlockTwo" runat="server" Width="200px" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Localize ID="Localize8" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_Search %>"></asp:Localize></span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 250px; height: 34px; text-align: left; vertical-align: top;">
            <asp:Localize ID="Localize7" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_SearchIndex %>"></asp:Localize>
            <br />
        </td>
        <td style="vertical-align: top;">
            <asp:LinkButton runat="server" ID="btnDoindex" Text="<%$ Resources:Resource, Admin_CommonSettings_Generate %>"
                OnClick="btnDoindex_Click" />
            <asp:Label runat="server" ID="lbDone" Text="<%$ Resources:Resource, Admin_CommonSettings_RunningInBackGroung%>"
                Visible="False"></asp:Label>
        </td>
    </tr>
    <tr class="rowsPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Localize ID="Localize_Admin_CommonSettings_DefaultView" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_DefaultView %>"></asp:Localize></span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Localize ID="Localize_Admin_CommonSettings_DefaultCatalogView" runat="server"
                Text="<%$ Resources:Resource, Admin_CommonSettings_DefaultCatalogView %>"></asp:Localize>
        </td>
        <td>
            <asp:DropDownList ID="ddlCatalogView" runat="server">
                <asp:ListItem Text="<%$ Resources:Resource, Client_Catalog_Tiles %>" Value="0"></asp:ListItem>
                <asp:ListItem Text="<%$ Resources:Resource, Client_Catalog_List %>" Value="1"></asp:ListItem>
                <asp:ListItem Text="<%$ Resources:Resource, Client_Catalog_Table %>" Value="2"></asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Localize ID="Localize_Admin_CommonSettings_DefaultSearchView" runat="server"
                Text="<%$ Resources:Resource, Admin_CommonSettings_DefaultSearchView %>"></asp:Localize>
        </td>
        <td>
            <asp:DropDownList ID="ddlSearchView" runat="server">
                <asp:ListItem Text="<%$ Resources:Resource, Client_Catalog_Tiles %>" Value="0"></asp:ListItem>
                <asp:ListItem Text="<%$ Resources:Resource, Client_Catalog_List %>" Value="1"></asp:ListItem>
                <asp:ListItem Text="<%$ Resources:Resource, Client_Catalog_Table %>" Value="2"></asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr class="rowsPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Localize ID="Localize_Admin_CommonSettings_ProductPhotos" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_ProductPhotos %>"></asp:Localize></span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Localize ID="Localize17" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_CompressBigImage %>"></asp:Localize>
        </td>
        <td>
            <asp:CheckBox runat="server" ID="chkCompressBigImage" />
        </td>
    </tr>
    
    <tr class="rowsPost">
        <td colspan="2" style="height: 34px;">
            <span class="spanSettCategory">
                <asp:Localize ID="Localize5" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_Reviews %>"></asp:Localize></span>
            <hr color="#C2C2C4" size="1px" />
        </td>
    </tr>
    <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Localize ID="Localize10" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_AllowReviews %>"></asp:Localize>
        </td>
        <td>
            <asp:CheckBox runat="server" ID="chkAllowReviews" />
        </td>
    </tr>

    <tr class="rowsPost">
        <td style="width: 250px; text-align: left;">
            <asp:Localize ID="Localize6" runat="server" Text="<%$ Resources:Resource, Admin_CommonSettings_ModerateReviews %>"></asp:Localize>
        </td>
        <td>
            <asp:CheckBox runat="server" ID="ckbModerateReviews" />
        </td>
    </tr>
</table>
<asp:SqlDataSource ID="SqlDataSource2" runat="server" SelectCommand="SELECT [Name], [Code], [CurrencyIso3] FROM [Catalog].[Currency]"
    OnInit="SqlDataSource2_Init"></asp:SqlDataSource>
