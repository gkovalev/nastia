<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RecentlyView.ascx.cs"
    Inherits="UserControls_RecentlyView" %>
<%@ Import Namespace="AdvantShop.Core.UrlRewriter" %>
<!--noindex-->
<article class="block-uc-alt expander-ligth" data-plugin="expander">
    <h3 class="title" data-expander-control="#recentlyview-content"><%= Resources.Resource.Client_UserControls_RecentlyViews_RecentlyViewed %></h3>
    <div class="content" id="recentlyview-content">
        <asp:ListView ID="lvRecentlyView" runat="server">
            <ItemTemplate>
                <div class="row">
                    <a class="link-light" href="<%# UrlService.GetLink(ParamType.Product, Eval("UrlPath").ToString(), Convert.ToInt32(Eval("ProductID")) ) %>">
                        <%# Eval("Name") %></a>
                </div>
            </ItemTemplate>
        </asp:ListView>
    </div>
</article>
<!--/noindex-->
