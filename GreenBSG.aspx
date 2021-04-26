<%@ Page Language="C#" MasterPageFile="MasterPage.master" EnableViewState="false"
    CodeFile="GreenBSG.aspx.cs" Title="ПОМОЩЬ В ВЫБОРЕ ИНСТРУМЕНТА (БЫТОВОГО И САДОВОГО) " Inherits="Details" %>

<%@ Import Namespace="AdvantShop.Catalog" %>
<%@ Import Namespace="AdvantShop.Core.UrlRewriter" %>
<%@ Import Namespace="AdvantShop.Customers" %>
<%@ Import Namespace="Resources" %>
<%@ Import Namespace="AdvantShop.Orders" %>
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
<%@ Register Src="~/UserControls/Details/ProductReviews.ascx" TagName="ProductReviews"
    TagPrefix="adv" %>

<%@ Register TagPrefix="adv" TagName="BreadCrumbs" Src="~/UserControls/BreadCrumbs.ascx" %>
<%@ Register TagPrefix="adv" TagName="Rating" Src="~/UserControls/Rating.ascx" %>
<%@ Register TagPrefix="adv" TagName="StaticBlock" Src="~/UserControls/StaticBlock.ascx" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderHeader" runat="Server">
    <script type="text/javascript" src="http://vk.com/js/api/share.js?11" charset="windows-1251"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderBottom" runat="Server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
   <iframe src="BoschBrandSiteGenerator/bsgDIY/fe/index.html" id="bosch-bsg-frontend" width="100%" height="100" scrolling="no"  frameborder="0">
   </iframe>
</asp:Content>
