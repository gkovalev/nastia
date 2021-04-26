<%@ Page Language="C#" MasterPageFile="MasterPageSocial.master" EnableViewState="false"
    CodeFile="DetailsSocial.aspx.cs" Inherits="DetailsSocial" %>

<%@ Import Namespace="AdvantShop.Catalog" %>
<%@ Import Namespace="AdvantShop.Customers" %>
<%@ Import Namespace="Resources" %>
<%@ MasterType VirtualPath="MasterPageSocial.master" %>
<%@ Register Src="~/UserControls/Details/CustomOptions.ascx" TagName="CustomOptions"
    TagPrefix="adv" %>
<%@ Register Src="~/UserControls/Details/ProductPhotoView.ascx" TagName="ProductPhotoView"
    TagPrefix="adv" %>
<%@ Register Src="~/UserControls/Details/ProductVideoView.ascx" TagName="ProductVideoView"
    TagPrefix="adv" %>
<%@ Register Src="~/UserControls/Details/ProductPropertiesView.ascx" TagName="ProductPropertiesView"
    TagPrefix="adv" %>
<%@ Register Src="~/Social/UserControls/RelatedProductsSocial.ascx" TagName="RelatedProductsSocial"
    TagPrefix="adv" %>
<%@ Register Src="~/UserControls/Details/ProductReviews.ascx" TagName="ProductReviews"
    TagPrefix="adv" %>
<%@ Register TagPrefix="adv" TagName="BreadCrumbs" Src="~/UserControls/BreadCrumbs.ascx" %>
<%@ Register TagPrefix="adv" TagName="Rating" Src="~/UserControls/Rating.ascx" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderHeader" runat="Server">
    <script type="text/javascript" src="http://vk.com/js/api/share.js?11" charset="windows-1251"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderBottom" runat="Server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div class="stroke">
        <div class="crumbs-thin">
            <adv:BreadCrumbs runat="server" ID="breadCrumbs" />
        </div>
        <div class="block-d">
            <adv:ProductPhotoView runat="server" ID="productPhotoView" />
            <div class="d-info">
                <h1 class="product-name">
                    <%= CurrentProduct.Name %></h1>
                <adv:Rating ID="rating" runat="server" />
                <div class="prop">
                    <div class="prop-str">
                        <span class="param-name">
                            <%= Resource.Client_Details_SKU %>:</span><span class="param-value"><%= CurrentProduct.ArtNo %></span>
                    </div>
                    <div class="prop-str" id="pnlBrand" runat="server">
                        <span class="param-name">
                            <%= Resource.Client_Details_Brand %>:</span><span class="param-value"><%= CurrentProduct.Brand.Name %></span>
                    </div>
                    <div class="prop-str" id="pnlSize" runat="server">
                        <span class="param-name">
                            <%= Resource.Client_Details_Size%>:</span><span class="param-value"><%= CurrentProduct.Size.Replace("|", " x ") %>
                            </span>
                    </div>
                    <div class="prop-str" id="pnlWeight" runat="server">
                        <span class="param-name">
                            <%= Resource.Client_Details_Weight%>:</span><span class="param-value"><%= CurrentProduct.Weight %>
                                <%= Resource.Client_Details_KG %></span>
                    </div>
                    <div class="prop-str">
                        <span class="param-name">
                            <%= Resource.Client_Details_Availability %>:</span> <span class="param-value">
                                <asp:Literal ID="lAvailiableAmount" runat="server" /></span>
                    </div>
                    <div class="prop-str" runat="server" id="divAmount">
  <span class="param-name param-name-txt">
                            <%= Resource.Client_Details_Amount %>:</span> <span class="param-value"><span class="input-wrap">
                                <input data-plugin="spinbox" runat="server" type="text" id="txtAmount" value="1" /></span></span>
                    </div>
                    <adv:CustomOptions ID="productCustomOptions" runat="server" />
                </div>
                <div id="pnlPrice" runat="server">
                    <div class="price-c">
                        <%= CatalogService.RenderPrice(CurrentProduct.Price, CurrentProduct.Discount, true, CustomerSession.CurrentCustomer.CustomerGroup, CustomOptionsService.SerializeToXml(productCustomOptions.CustomOptions, productCustomOptions.SelectedOptions)) %>
                    </div>
                    <div class="btns-d">
                        <adv:Button ID="btnAdd" runat="server" Type="Buy" Size="Big" Text='<%$ Resources:Resource, Client_Details_Add %>'/>
                        <adv:Button ID="btnOrderByRequest" runat="server" Size="Big" Type="Action" Text='<%$ Resources:Resource, Client_Catalog_OrderByRequest %>'
                            OnClick="btnOrderByRequest_Click" DisableValidation="true" Target="_blank" />
                    </div>
                    <br class="clear" />
                </div>
            </div>
            <br class="clear" />
        </div>
        <div class="tabs tabs-hr" data-plugin="tabs">
            <div class="tabs-headers">
                <div data-tabs-header="true" class="tab-header <%= string.IsNullOrEmpty(CurrentProduct.Description) ? "tab-hidden" : ""%>"
                    id="tab-descr">
                    <span class="tab-inside">
                        <%= Resource.Client_Details_Description %></span>
                </div>
                <div data-tabs-header="true" class="tab-header <%= !productPropertiesView.HasProperties ? "tab-hidden" : ""%>"
                    id="tab-property">
                    <span class="tab-inside">
                        <%= Resource.Client_Details_Properties %></span>
                </div>
                <div data-tabs-header="true" class="tab-header <%= !ProductVideoView.hasVideos ? "tab-hidden" : ""%>" id="tab-video">
                    <span class="tab-inside">
                        <%= Resource.Client_Details_Video %></span>
                </div>
            </div>
            <div class="tabs-contents">
                <div data-tabs-content="true" class="tab-content">
                    <% = CurrentProduct.Description %>
                </div>
                <div data-tabs-content="true" class="tab-content">
                    <adv:ProductPropertiesView ID="productPropertiesView" runat="server" />
                </div>
                <div data-tabs-content="true" class="tab-content">
                    <adv:ProductVideoView ID="ProductVideoView" runat="server" />
                </div>
            </div>
        </div>
        <div class="tabs tabs-hr" data-plugin="tabs">
            <div class="tabs-headers">
                <div data-tabs-header="true" class="tab-header <%= !alternativeProducts.HasProducts ? "tab-hidden" : ""%>"
                    id="tab-alt">
                    <span class="tab-inside">
                        <%= AdvantShop.Configuration.SettingsCatalog.AlternativeProductName %></span>
                </div>
                <div data-tabs-header="true" class="tab-header <%= !relatedProducts.HasProducts ? "tab-hidden" : ""%>" id="tab-related">
                    <span class="tab-inside">
                        <%= AdvantShop.Configuration.SettingsCatalog.RelatedProductName %></span>
                </div>
            </div>
            <div class="tabs-contents">
                <div data-tabs-content="true" class="tab-content">
                    <adv:RelatedProductsSocial runat="server" ID="alternativeProducts" RelatedType="Alternative" />
                </div>
                <div data-tabs-content="true" class="tab-content">
                    <adv:RelatedProductsSocial runat="server" ID="relatedProducts" RelatedType="Related" />
                </div>
            </div>
        </div>
        <div class="tabs tabs-hr" data-plugin="tabs">
            <div data-tabs-header="true" class="tabs-headers">
                <div class="tab-header selected" id="tab-review">
                    <span class="tab-inside">
                        <%= Resource.Client_Details_Reviews%>
                        <asp:Literal ID="lReviewsCount" runat="server" /></span>
                </div>
            </div>
            <div class="tabs-contents">
                <div data-tabs-content="true"  class="tab-content selected">
                    <adv:ProductReviews ID="productReviews" runat="server" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
