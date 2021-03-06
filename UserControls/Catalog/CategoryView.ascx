<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CategoryView.ascx.cs"
    Inherits="UserControls_Catalog_CategoryView" %>
<%@ Import Namespace="AdvantShop.Core.UrlRewriter" %>
<asp:ListView runat="server" ID="lvCategory" GroupItemCount="4">
    <LayoutTemplate>
        <table class="categories">
            <asp:PlaceHolder ID="groupPlaceholder" runat="server" />
        </table>
    </LayoutTemplate>
    <GroupTemplate>
        <tr>
            <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
        </tr>
    </GroupTemplate>
    <GroupSeparatorTemplate>
        <tr class="cat-row-split">
            <td class="cat-split">
                &nbsp;
            </td>
            <td class="cat-split">
                &nbsp;
            </td>
            <td class="cat-split">
                &nbsp;
            </td>
            <td class="cat-split">
                &nbsp;
            </td>
            <td class="cat-split">
                &nbsp;
            </td>
            <td class="cat-split">
                &nbsp;
            </td>
            <td class="cat-split">
                &nbsp;
            </td>
        </tr>
    </GroupSeparatorTemplate>
    <ItemTemplate>
        <td>
            <%# RenderCategoryImage(Eval("MiniPicture.PhotoName").ToString(), Convert.ToInt32(Eval("CategoryID")), Eval("UrlPath").ToString(), Eval("Name").ToString())%>
            <div class="cat-name">
                <a href="<%# UrlService.GetLink(ParamType.Category, Eval("UrlPath").ToString(), Convert.ToInt32(Eval("CategoryId")))%>">
                    <%#Eval("Name") %></a>
                <%if (DisplayProductsCount)
                  { %>
                <span class="cat-count">
                    <%# Eval("ProductsCount")%></span>
                <% } %></div>
        </td>
    </ItemTemplate>
    <ItemSeparatorTemplate>
        <td class="cat-split">
            &nbsp;
        </td>
    </ItemSeparatorTemplate>
    <EmptyItemTemplate>
        <td class="cat-empty">
            &nbsp;
        </td>
    </EmptyItemTemplate>
</asp:ListView>
