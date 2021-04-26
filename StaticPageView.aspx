<%@ Page Language="C#" MasterPageFile="MasterPage.master" AutoEventWireup="true"
    CodeFile="StaticPageView.aspx.cs" Inherits="StaticPageView" %>

<%@ Import Namespace="AdvantShop.Core.UrlRewriter" %>
<%@ Import Namespace="AdvantShop" %>
<%@ Register TagPrefix="adv" TagName="BreadCrumbs" Src="~/UserControls/BreadCrumbs.ascx" %>
<%@ Register TagPrefix="adv" TagName="StaticBlock" Src="~/UserControls/StaticBlock.ascx" %>
<%@ MasterType VirtualPath="MasterPage.master" %>
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
    <div class="stroke">
        <div class="<%= hasSubPages ? "left-thin" : "content-owner" %>">
            <h1>
                <%= page.PageName %></h1>
            <adv:BreadCrumbs runat="server" ID="ucBreadCrumbs" />
            <div class="new-descr">
                <%= page.PageText %>
            </div>
            <div class="newsSocial">
                <adv:StaticBlock ID="sbShareButtons" runat="server" SourceKey="socialShareNews" />
                <div class="clear">
                </div>
            </div>
        </div>
        <div class="right-slim" runat="server" id="rightBlock">
            <div class="block-static">
                <div class="title">
                    <%= page.PageName %>
                </div>
                <asp:ListView ID="lvSubPages" runat="server">
                    <LayoutTemplate>
                        <div class="content">
                            <ul class="list-news-cat">
                                <li runat="server" id="itemPlaceHolder"></li>
                            </ul>
                        </div>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <li><a href="<%# UrlService.GetLink(ParamType.StaticPage, Eval("UrlPath").ToString(), Eval("id").ToString().TryParseInt()) %>">
                            <%#Eval("PageName")%></a></li>
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
