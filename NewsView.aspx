<%@ Page Language="C#" MasterPageFile="MasterPage.master" AutoEventWireup="true"
    EnableViewState="false" CodeFile="NewsView.aspx.cs" Inherits="NewsView" Title="" %>

<%@ MasterType VirtualPath="MasterPage.master" %>
<%@ Import Namespace="AdvantShop.Core.UrlRewriter" %>
<%@ Import Namespace="AdvantShop.Helpers" %>
<%@ Register TagPrefix="adv" TagName="StaticBlock" Src="~/UserControls/StaticBlock.ascx" %>
<%@ Register TagPrefix="adv" TagName="BreadCrumbs" Src="~/UserControls/BreadCrumbs.ascx" %>
<asp:Content runat="server" ID="contentHead" ContentPlaceHolderID="ContentPlaceHolderHeader">
    <script type="text/javascript" src="http://vk.com/js/api/share.js?11" charset="windows-1251"></script>
    <script type="text/javascript">
        (function () {
            var po = document.createElement('script'); po.type = 'text/javascript'; po.async = true;
            po.src = 'https://apis.google.com/js/plusone.js';
            var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(po, s);
        })();
    </script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div id="MainNewsDiv" class="stroke">
        <div class="left-thin">
            <h1>
                <%=StrTitleText%></h1>
            <adv:BreadCrumbs runat="server" ID="ucBreadCrumbs" />
            <br />
            <div class="new-descr">
                <%=StrMainText%>
            </div>
            <div class="newsSocial">
                <adv:StaticBlock ID="sbShareButtons" runat="server" SourceKey="socialShareNews" />
                <div class="clear">
                </div>
            </div>
            <div class="news-return">
                <a href="<%= UrlService.GetAbsoluteLink("news") %>">
                    <%= Resources.Resource.Client_News_BackToNews%></a>
            </div>
        </div>
        <div class="right-slim">
            <div class="block-static">
                <asp:ListView ID="lvNewsCategories" runat="server">
                    <LayoutTemplate>
                        <div class="title">
                            <asp:Literal runat="server" Text="<%$ Resources:Resource,Client_News_Categories   %>"></asp:Literal>
                        </div>
                        <div class="content">
                            <ul class="list-news-cat">
                                <li runat="server" id="itemPlaceHolder"></li>
                            </ul>
                        </div>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <li><a href="<%# UrlService.GetLink(ParamType.NewsCategory, SQLDataHelper.GetString( Eval("UrlPath")),SQLDataHelper.GetInt( Eval("NewsCategoryID") ) )  %>">
                            <%#Eval("Name")%></a></li>
                    </ItemTemplate>
                </asp:ListView>
            </div>
            <div class="block-static">
                <adv:StaticBlock ID="staticBlockTwitter" runat="server" SourceKey="TwitterInNews" />
            </div>
            <div>
                <adv:StaticBlock ID="staticBlockVk" runat="server" SourceKey="Vk" />
            </div>
        </div>
        <br class="clear" />
    </div>
</asp:Content>
