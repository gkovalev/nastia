<%@ Page Language="C#" MasterPageFile="MasterPage.master" EnableViewState="false"
    CodeFile="Details.aspx.cs" Inherits="Details"  %>

<%@ Import Namespace="AdvantShop.Catalog" %>
<%@ Import Namespace="AdvantShop.Configuration" %>
<%@ Import Namespace="AdvantShop.Core.UrlRewriter" %>
<%@ Import Namespace="AdvantShop.Customers" %>
<%@ Import Namespace="Resources" %>
<%@ MasterType VirtualPath="MasterPage.master" %>
<%@ Register Src="~/UserControls/Details/CustomOptions.ascx" TagName="CustomOptions"
    TagPrefix="adv" %>
<%@ Register Src="~/UserControls/Details/ProductPhotoView.ascx" TagName="ProductPhotoView"
    TagPrefix="adv" %>
<%@ Register Src="~/UserControls/Details/ProductVideoView.ascx" TagName="ProductVideoView"
    TagPrefix="adv" %>
<%@ Register Src="~/UserControls/Details/ProductPropertiesView.ascx" TagName="ProductPropertiesView"
    TagPrefix="adv" %>
      <%@ Register Src="~/UserControls/Details/ProductPropertiesSetView.ascx" TagName="ProductPropertiesSetView"
    TagPrefix="adv" %>
 <%@ Register Src="~/UserControls/Details/ProductPropertiesAddedValueView.ascx" TagName="ProductPropertiesAddedValueView"
    TagPrefix="adv" %>
<%@ Register Src="~/UserControls/Details/RelatedProducts.ascx" TagName="RelatedProducts"
    TagPrefix="adv" %>
<%@ Register Src="~/UserControls/Details/RelatedBoschProducts.ascx" TagName="RelatedBoschProducts"
    TagPrefix="adv" %>
<%@ Register Src="~/UserControls/Details/ProductReviews.ascx" TagName="ProductReviews"
    TagPrefix="adv" %>
    <%@ Register TagPrefix="adv" TagName="BuyInOneClick" Src="~/UserControls/BuyInOneClick.ascx" %>

<%@ Register TagPrefix="adv" TagName="BreadCrumbs" Src="~/UserControls/BreadCrumbs.ascx" %>
<%@ Register TagPrefix="adv" TagName="Rating" Src="~/UserControls/Rating.ascx" %>
<%@ Register TagPrefix="adv" TagName="StaticBlock" Src="~/UserControls/StaticBlock.ascx" %>
<%@ Register TagPrefix="adv" TagName="CompareControl" Src="~/UserControls/Catalog/CompareControl.ascx" %>
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
                <a id="hrefAdmin" runat="server" visible="false" target="_blank" class="details-admin">
                    <% = Resource.Client_Details_Link_ShowInClient %></a>
                <adv:Rating ID="rating" runat="server" />
                <div class="prop">
                    <div class="prop-str">
                        <span class="param-name">
                            <%= Resource.Client_Details_SKU %>:</span><span class="param-value"><%= CurrentProduct.ArtNo %></span>
                    </div>
                     <!--Added by Evgeni to add EAN-->
                     <div class="prop-str">
                        <span class="param-name">
                            EAN:</span><span class="param-value"><%= CurrentProduct.EAN %></span>
                    </div>
                     <div class="prop-str">
                        <span class="param-name">
                           <%= Resource.Client_Details_ManufactureSKU %>:</span><span class="param-value"><%= CurrentProduct.ManufactureArtNo %></span>
                    </div>
                    
                    <div class="prop-str" id="pnlBrand" runat="server">
                        <span class="param-name">
                            <%= Resource.Client_Details_Brand %>:</span><span class="param-value"> <a href="<%= UrlService.GetLink(ParamType.Brand,CurrentProduct.Brand.UrlPath ,CurrentProduct.BrandId) %>">
                                <%= CurrentProduct.Brand.Name %></a></span>
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
                                                               <%=RenderSpinBox()%></span></span>

                    </div>
                    <div class="prop-str" runat="server" id="divUnit">
                        <span class="param-name">
                            <%= Resource.Client_Details_Unit %>:</span> <span class="param-value">
                                <%= CurrentProduct.Offers[0].Unit %></span>
                    </div>
                    <adv:CustomOptions ID="productCustomOptions" runat="server" />
                </div>
                <div id="pnlPrice" runat="server">
                    <div class="price-c">
                    <!--Added By EVgeni-->
                    <asp:Label ID="lblSinglePriceTextForOptUser" runat="server" Text=""></asp:Label>
                        <%= CatalogService.RenderPrice(CurrentProduct.Price, CurrentProduct.Discount, true, CustomerSession.CurrentCustomer.CustomerGroup, CustomOptionsService.SerializeToXml(productCustomOptions.CustomOptions, productCustomOptions.SelectedOptions)) %>
                        <asp:Label ID="lblOptPrice" runat="server" Text=""></asp:Label>
                     
                    </div>
                    
                    <div class="btns-d">
                        <adv:Button ID="btnAdd" runat="server" Type="Buy" Size="Big" Text='<%$ Resources:Resource, Client_Details_Add %>'
                            ValidationGroup="cOptions" />
                        <adv:Button ID="btnAddCredit" runat="server" Type="Buy" Size="Big" CssSpan="btn-credit" Text='<%$ Resources:Resource, Client_Details_AddCredit %>'
                            ValidationGroup="cOptions" />
                        <adv:Button ID="btnOrderByRequest" runat="server" Size="Big" Type="Action" Text='<%$ Resources:Resource, Client_Catalog_OrderByRequest %>'
                            OnClick="btnOrderByRequest_Click" DisableValidation="true" />
                            <adv:BuyInOneClick ID="BuyInOneClick" runat="server" />
                    </div>
                    <br class="clear" />
                </div>
                <div class="features">
                    <adv:CompareControl ID="CompareControl" runat="server" />
                    <div id="pnlWishlist" runat="server">
                        <asp:LinkButton ID="lbtnWishlist" Class="wishlist-link" runat="server" OnClick="AddToWishList_Click">
                            <img src="images/icons/star.png" class="wishlist-link" alt="" /><%= Resource.Client_Details_AddToWishList %></asp:LinkButton>
                        <span id="alreadyWishList" class="wishlist-link" runat="server" visible="False">
                           <img src="images/icons/star.png" class="wishlist-link" alt=""/><%= Resource.Client_Details_AlreadyInWishList %></span>
                    </div>
                </div>
            </div>
            <div class="sb_details">
                <adv:StaticBlock ID="sbBannerDetails" runat="server" SourceKey="bannerDetails" />
            </div>
            <div class="d-social">
                <div class="d-social-block">
                    <adv:StaticBlock Visible="False" ID="sbShareButtons" runat="server" SourceKey="socialShareDetails" />
                </div>
                <div id="tabs-link" data-tabs-links="true">
                </div>
                 <Adv:StaticBlock ID="staticBlockBp" runat="server" SourceKey="BestPriceOnDetails" />
            </div>
            <div class="d-qr">
                <asp:Literal ID="ltrlRightColumnModules" runat="server"></asp:Literal>
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

                  <div data-tabs-header="true" class="tab-header <%= !productPropertiesAddedValueView.HasProperties ? "tab-hidden" : ""%>"
                    id="tab-propertyAddedValue">
                    <span class="tab-inside"> 
                        <%= Resource.Client_Details_AddedValue_Properties%></span>
                </div>

                <div data-tabs-header="true" class="tab-header <%= !productPropertiesSetView.HasProperties ? "tab-hidden" : ""%>"
                    id="tab-propertySet">
                    <span class="tab-inside"> 
                        <%= Resource.Client_Details_Set_Properties%></span>
                </div>

                <div data-tabs-header="true" class="tab-header <%= !ProductVideoView.hasVideos ? "tab-hidden" : ""%>"
                    id="tab-video">
                    <span class="tab-inside">
                        <%= Resource.Client_Details_Video %></span>
                </div>
                <div class="tabs-border">
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
                    <adv:ProductPropertiesAddedValueView ID="productPropertiesAddedValueView" runat="server" />
                </div>
                 <div data-tabs-content="true" class="tab-content">
                    <adv:ProductPropertiesSetView ID="productPropertiesSetView" runat="server" />
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
                <div class="tabs-border">
                </div>
            </div>
            <div class="tabs-contents">
                <div class="tab-content" data-tabs-content="true">
                    <adv:RelatedProducts runat="server" ID="alternativeProducts" RelatedType="Alternative" />
                </div>
            </div>
        </div>
        <div class="tabs tabs-hr" data-plugin="tabs">
            <div class="tabs-headers">
                <div data-tabs-header="true" class="tab-header <%= !relatedProducts.HasProducts ? "tab-hidden" : ""%>"
                    id="tab-related">
                    <span class="tab-inside">
                        <%= AdvantShop.Configuration.SettingsCatalog.RelatedProductName %></span>
                </div>
                <div class="tabs-border">
                </div>
            </div>
            <div class="tabs-contents">
                <div class="tab-content" data-tabs-content="true">
                    <adv:RelatedProducts runat="server" ID="relatedProducts" RelatedType="Related" />
                </div>
            </div>
        </div>

        <!--Added by Evgeni-->
         <div class="tabs tabs-hr" data-plugin="tabs">
            <div class="tabs-headers">
                <div data-tabs-header="true" class="tab-header <%= !relatedBoschProducts.HasProducts ? "tab-hidden" : ""%>"
                    id="tab-Boschrelated">
                    <span class="tab-inside">
                        Bosch Рекомендует</span>
                </div>
                <div class="tabs-border">
                </div>
            </div>
            <div class="tabs-contents">
                <div class="tab-content" data-tabs-content="true">
                    <adv:RelatedBoschProducts runat="server" ID="relatedBoschProducts" RelatedType="BoschRelated" />
                </div>
            </div>
        </div>

        <!--End changes-->

        <% if (SettingsCatalog.AllowReviews)
           { %>
        <div class="tabs tabs-hr" data-plugin="tabs">
            <div class="tabs-headers">
                <div data-tabs-header="true" class="tab-header selected" id="tab-review">
                    <span class="tab-inside">
                        <%= Resource.Client_Details_Reviews %>
                        <asp:Literal ID="lReviewsCount" runat="server" /></span>
                </div>
                <div class="tabs-border">
                </div>
            </div>
            <div class="tabs-contents">
                <div data-tabs-content="true" class="tab-content selected">
                    <adv:ProductReviews ID="productReviews" runat="server" />
                </div>
            </div>
        </div>
        <% } %>
    <div align="center">
   <Adv:StaticBlock ID="stFooterOnDetails" runat="server" SourceKey="FooterOnDetails" /> 
    </div>
    </div>

</asp:Content>
