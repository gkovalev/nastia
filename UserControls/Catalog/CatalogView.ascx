<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CatalogView.ascx.cs" Inherits="UserControls_CatalogView" %>
<%@ Import Namespace="AdvantShop.Core.UrlRewriter" %>
<article class="block-uc" data-plugin="expander" id="catalogview">
    <h3 class="title" data-expander-control="#catalogView-content"><%= HeadCategory.Name %></h3>
    <div class="content" id="catalogView-content">
        <asp:ListView runat="server" ID="lvChilds" ItemPlaceholderID="item">
            <LayoutTemplate>
                <ul class="treeview">
                    <div runat="server" id="item">
                    </div>
                </ul>
            </LayoutTemplate>
            <ItemTemplate>
                <li>
                    <%# Convert.ToInt32(Eval("CategoryID")) == CategoryID 
                    ? String.Format("<a href=\"{0}\" class=\"selected\">{1}{2}</a>", UrlService.GetLink(ParamType.Category, Eval("UrlPath").ToString(), Convert.ToInt32(Eval("CategoryID"))), Eval("Name"), DisplayProductsCount ? " (" + Eval("ProductsCount") + ")": "")
                        : String.Format("<a href=\"{0}\">{1}{2}</a>", UrlService.GetLink(ParamType.Category, Eval("UrlPath").ToString(), Convert.ToInt32(Eval("CategoryID"))), Eval("Name"), DisplayProductsCount ? " (" + Eval("ProductsCount") + ")": "")%>
                </li>
            </ItemTemplate>
        </asp:ListView>
    </div>
</article>
