<%@ Control Language="C#" AutoEventWireup="true" CodeFile="News.ascx.cs" Inherits="UserControls_News"
    EnableViewState="false" %>
<%@ Import Namespace="AdvantShop.Core.UrlRewriter" %>
<!--noindex-->
<article class="block-uc" data-plugin="expander">
    <h3 class="title" data-expander-control="#expander-news">
        <%= Resources.Resource.Client_UserControls_News_News %></h3>
    <div class="content" id="expander-news">
        <asp:ListView runat="server" ID="lvNews">
            <ItemTemplate>
                <div class="news-item">
                    <a href="<%# UrlService.GetLink(ParamType.News, Eval("UrlPath").ToString() ,Convert.ToInt32(Eval("NewsID"))) %>"
                        class="link-news-anno">
                        <%#Eval("Title")%></a>
                    <div class="news-date">
                        <%# AdvantShop.Localization.Culture.ConvertShortDate((DateTime)Eval("AddingDate"))%>
                    </div>
                </div>
            </ItemTemplate>
        </asp:ListView>
        <a href="news">
            <%= Resources.Resource.Client_UserControls_News_AllNews %></a>
    </div>
</article>
<!--/noindex-->
