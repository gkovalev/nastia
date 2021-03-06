<%@ Page Language="C#" MasterPageFile="MasterPage.master" AutoEventWireup="true"
    EnableViewState="false" CodeFile="News.aspx.cs" Inherits="News" %>

<%@ Import Namespace="AdvantShop.Core.UrlRewriter" %>
<%@ Import Namespace="AdvantShop.FilePath" %>
<%@ Import Namespace="AdvantShop.Helpers" %>
<%@ MasterType VirtualPath="MasterPage.master" %>
<%@ Register TagPrefix="adv" TagName="StaticBlock" Src="~/UserControls/StaticBlock.ascx" %>
<%@ Register TagPrefix="adv" TagName="BreadCrumbs" Src="~/UserControls/BreadCrumbs.ascx" %>
<%@ Register TagPrefix="adv" TagName="NewsSubscription" Src="~/UserControls/NewsSubscription.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div class="stroke">
        <div class="left-thin">
            <h1>
                <asp:Literal ID="header" runat="server"></asp:Literal>
            </h1>
            <adv:BreadCrumbs runat="server" ID="ucBreadCrumbs" />
            <asp:ListView ID="lvNews" runat="server" ItemPlaceholderID="itemPlaceHolder">
                <LayoutTemplate>
                    <ul class="news-list">
                        <li class="news-item" runat="server" id="itemPlaceHolder"></li>
                    </ul>
                </LayoutTemplate>
                <ItemTemplate>
                    <li class="news-item">
                        <div class="news-img">
                            <a href='<%# UrlService.GetLink(ParamType.News, Convert.ToString(Eval("UrlPath")), Convert.ToInt32( Eval("NewsID"))) %>'
                                runat="server" visible='<%# Eval("Picture") != null && !string.IsNullOrEmpty( Eval("Picture").ToString() ) %>'>
                                <img src="<%#FoldersHelper.GetPath(FolderType.News,  Eval("Picture") as string ,false)  %>"
                                    <%# string.Format("alt=\"{0}\" title=\"{0}\"", HttpUtility.HtmlEncode(Eval("Title").ToString())) %> /></a>
                        </div>
                        <div class="news-info">
                            <div class="news-title">
                                <a href="<%# UrlService.GetLink(ParamType.News, Convert.ToString(Eval("UrlPath")), Convert.ToInt32( Eval("NewsID"))) %>">
                                    <%#Eval("Title")%><br />
                                    <%--<%#Eval("TextAnnotation")%>--%></a></div>
                            <div class="news-date">
                                <%# Eval("AddingDate")%></div>
                            <div class="news-descr">
                                <%#Eval("TextAnnotation")%>
                            </div>
                        </div>
                    </li>
                </ItemTemplate>
            </asp:ListView>
            <adv:AdvPaging runat="server" ID="paging" DisplayShowAll="True" />
        </div>
        <div class="right-slim">
            <div class="block-uc">
                <h3 class="title">
                    <%= Resources.Resource.Client_News_Categories %></h3>
                <div class="content">
                    <asp:ListView ID="lvNewsCategories" runat="server">
                        <LayoutTemplate>
                            <ul class="list-news-cat">
                                <li runat="server" id="itemPlaceHolder"></li>
                            </ul>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <li><a href="<%#  UrlService.GetLink(ParamType.NewsCategory, SQLDataHelper.GetString( Eval("UrlPath")),SQLDataHelper.GetInt( Eval("NewsCategoryID") ) ) %>">
                                <%#Eval("Name")%></a></li>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
            </div>
            <adv:NewsSubscription runat="server" />
            <adv:StaticBlock ID="staticBlockTwitter" runat="server" SourceKey="TwitterInNews" />
            <adv:StaticBlock ID="staticBlockVk" runat="server" SourceKey="Vk" />
        </div>
    </div>
    <br class="clear" />
</asp:Content>
