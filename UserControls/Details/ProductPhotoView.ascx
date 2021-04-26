<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProductPhotoView.ascx.cs"
    Inherits="UserControls_ProductPhotoView" %>
<%@ Import Namespace="AdvantShop" %>
<%@ Import Namespace="AdvantShop.Catalog" %>
<%@ Import Namespace="AdvantShop.Configuration" %>
<%@ Import Namespace="AdvantShop.FilePath" %>
<%@ Import Namespace="Resources" %>
<div class="d-photo" runat="server" id="pnlPhoto">
    <div class="d-c-photo">
        <div class="preview-image">
            <table>
                <tr>
                    <td class="preview-cell" style="height: <%= SettingsPictureSize.MiddleProductImageHeight %>px">
                        <% if (SettingsDesign.EnableZoom)
                           {%>
                        <a id="zoom" href="<%= FoldersHelper.GetImageProductPath(ProductImageType.Big, Product.Photo, false) %>"
                            class="zoom cloud-zoom" rel="">
                            <% }%>
                            <img id="preview-img" src="<%= FoldersHelper.GetImageProductPath(ProductImageType.Middle, Product.Photo, false) %>"
                                <%= string.Format("alt=\"{0}\" title=\"{0}\"", HttpUtility.HtmlEncode(PhotoService.GetDescription(Product.Photo.TryParseInt()))) %> />
                            <%= CatalogService.RenderLabels(Product.Recomended, Product.OnSale, Product.BestSeller, Product.New, Product.Discount)%>
                            <% if (SettingsDesign.EnableZoom)
                               {%>
                        </a>
                        <% }%>
                    </td>
                </tr>
            </table>
            <div class="zoom-c">
                <img src="images/icons/zoom.png" alt="" class="zoom-link" id="icon-zoom" />
                <a class="link-zoom" href="javascript:void(0);" id="link-fancybox"><% = Resource.Client_Details_Zoom %></a>
                <div class="fancybox-list">
                    <asp:ListView runat="server" ID="lvFancyBox">
                        <ItemTemplate>
                            <a class="fancybox" rel="fancybox-gallery" href="<%# FoldersHelper.GetImageProductPath(ProductImageType.Big, Eval("PhotoName").ToString(), false) %>"
                                title="<%# HttpUtility.HtmlEncode(Eval("Description").ToString()) %>"></a>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
            </div>
        </div>
        <div class="carousel-small" id="carouselDetails" runat="server">
            <asp:ListView runat="server" ID="lvPhotos" ItemPlaceholderID="liItemPlaceholder">
                <LayoutTemplate>
                    <ul class="jcarousel" id="carousel-preview">
                        <li runat="server" id="liItemPlaceholder" />
                    </ul>
                </LayoutTemplate>
                <ItemTemplate>
                    <li <%# (bool)Eval("Main") ? "class='selected'" : string.Empty %>>
                        <table>
                            <tr>
                                <td class="carousel-border" style="height: <%= SettingsPictureSize.XSmallProductImageHeight %>px">
                                    <a href="<%# FoldersHelper.GetImageProductPath(ProductImageType.Big, Eval("PhotoName").ToString(), false) %>" class="cloud-zoom-gallery" title="<%# HttpUtility.HtmlEncode(Eval("Description")) %>"
                                        rel="smallImage:'<%# FoldersHelper.GetImageProductPath(ProductImageType.Middle, Eval("PhotoName").ToString(), false) %>', useZoom: 'zoom'">
                                        <img src="<%# FoldersHelper.GetImageProductPath(ProductImageType.XSmall, Eval("PhotoName").ToString(), false) %>"
                                            <%# string.Format("alt=\"{0}\" title=\"{0}\"", HttpUtility.HtmlEncode(Eval("Description").ToString())) %> />
                                    </a>
                                </td>
                            </tr>
                        </table>
                    </li>
                </ItemTemplate>
            </asp:ListView>
        </div>
    </div>
        <!--Added By Evgeni to insert 3 Year warranty for Bosch Blue Pt-->
               <div align="center"> 
             <a href="https://instrument-opt.by/news/3yearswarrantybosch" target="_blank"> <asp:Image ID="imgWarranty" Visible="false" runat="server" src="images/tv_3ywarranty.jpg" AlternateText="Ð—Ð°Ñ€ÐµÐ³Ð¸ÑÑ‚Ñ€Ð¸Ñ€ÑƒÐ¹Ñ‚Ðµ ÑÐ²Ð¾Ð¹ Ð¿Ñ€Ð¾Ñ„ÐµÑÑÐ¸Ð¾Ð½Ð°Ð»ÑŒÐ½Ñ‹Ð¹ ÑÐ»ÐµÐºÑ‚Ñ€Ð¾Ð¸Ð½ÑÑ‚Ñ€ÑƒÐ¼ÐµÐ½Ñ‚ Ð½Ð° ÑÑ‚Ð¾Ð¼ ÑÐ°Ð¹Ñ‚Ðµ Ð² Ñ‚ÐµÑ‡ÐµÐ½Ð¸Ðµ Ñ‡ÐµÑ‚Ñ‹Ñ€Ñ‘Ñ… Ð½ÐµÐ´ÐµÐ»ÑŒ Ð¿Ð¾ÑÐ»Ðµ Ð¿Ð¾ÐºÑƒÐ¿ÐºÐ¸ Ð¸ Ð²Ñ‹ Ð¿Ð¾Ð»ÑƒÑ‡Ð¸Ñ‚Ðµ Ñ‚Ñ€Ð¸ Ð³Ð¾Ð´Ð° Ð±ÐµÑÐ¿Ð»Ð°Ñ‚Ð½Ð¾Ð³Ð¾ Ð³Ð°Ñ€Ð°Ð½Ñ‚Ð¸Ð¹Ð½Ð¾Ð³Ð¾ Ð¾Ð±ÑÐ»ÑƒÐ¶Ð¸Ð²Ð°Ð½Ð¸Ñ." /></a>
             <a href="http://www.dremeleurope.com/ru/ru/ocs/category/6147/%D0%BF%D0%BE%D0%B4%D0%B1%D0%B5%D1%80%D0%B8%D1%82%D0%B5-%D1%81%D0%BE%D0%BE%D1%82%D0%B2%D0%B5%D1%82%D1%81%D1%82%D0%B2%D1%83%D1%8E%D1%89%D0%B8%D0%B5-%D0%BD%D0%B0%D1%81%D0%B0%D0%B4%D0%BA%D0%B8" target="_blank"> <asp:Image ID="imgDremel" Visible="false" runat="server" src="images/DremelAccessor.png" AlternateText="Dremel подберите-соответствующие-насадки" /></a>
          </div>
</div>
<div class="d-photo" id="pnlNoPhoto" runat="server">
    <div class="d-c-photo">
        <div class="preview-image">
            <img src="images/nophoto.jpg" alt="" />
            <%= CatalogService.RenderLabels(Product.Recomended, Product.OnSale, Product.BestSeller, Product.New, Product.Discount)%>
        </div>
    </div>
</div>
