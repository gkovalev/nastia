<%@ Page Language="C#" MasterPageFile="MasterPage.master" AutoEventWireup="true"
    EnableViewState="false" CodeFile="Brands.aspx.cs" Inherits="Brands" %>

<%@ Import Namespace="AdvantShop.Core.UrlRewriter" %>
<%@ Import Namespace="AdvantShop.FilePath" %>
<%@ MasterType VirtualPath="MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div class="stroke">
        <div class="content-owner">
            <h1>
                <%= Resources.Resource.Client_Brands_Header %></h1>
            <div class="brands-symbol">
                <div class="brands-abc">
                    <div>
                        <asp:Literal ID="lEngLetters" runat="server" EnableViewState="false"></asp:Literal>
                    </div>
                    <div>
                        <asp:Literal ID="lRusLetters" runat="server" EnableViewState="false"></asp:Literal>
                    </div>
                </div>
            </div>
            <div class="brands-country">
                <asp:DropDownList ID="ddlCountry" runat="server" DataTextField="Name" DataValueField="CountryID"
                    onchange="redirectBrandCountry()">
                </asp:DropDownList>
            </div>
            <div>
                <asp:ListView runat="server" ID="lvBrands" ItemPlaceholderID="groupLi" GroupItemCount="4"
                    GroupPlaceholderID="liPlaceholder">
                    <LayoutTemplate>
                        <ul class="brands-list">
                            <li runat="server" id="liPlaceholder" />
                        </ul>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <li <%#Container.DataItemIndex % 4 == 0 || Container.DataItemIndex == 0 ? "class=\"brand-row-first\"" :""  %>>
                            <table class="brands-logo">
                                <tr>
                                    <td>
                                        <a href="<%# UrlService.GetLink(ParamType.Brand, Eval("UrlPath").ToString() ,(int) Eval("BrandID")) %>">
                                            <img src="<%# !String.IsNullOrEmpty(Eval("BrandLogo.PhotoName").ToString()) ? FoldersHelper.GetPath(FolderType.BrandLogo, Eval("BrandLogo.PhotoName") as string , false) : UrlService.GetAbsoluteLink("images/nophoto_xsmall.jpg") %>"
                                                <%# string.Format("alt=\"{0}\" title=\"{0}\"", HttpUtility.HtmlEncode(Eval("Name").ToString())) %> />
                                        </a>
                                    </td>
                                </tr>
                            </table>
                            <div class="brands-name">
                                <table class="t-bn">
                                    <tr>
                                        <td>
                                            <a href="<%# UrlService.GetLink(ParamType.Brand, Eval("UrlPath").ToString() , (int) Eval("BrandID")) %>"
                                                class="link-brend">
                                                <%#Eval("Name") %></a>
                                        </td>
                                        <td class="country-cell">
                                            <span class="country">
                                                <img src="<%# UrlService.GetAbsoluteLink("images/countries/") + (Eval("BrandCountry") != null ? Eval("BrandCountry.ISO2") + ".png" : "_world.png") %>"
                                                    alt='<%# Eval("BrandCountry") != null ? HttpUtility.HtmlEncode(Eval("BrandCountry.Name").ToString()) :  string.Empty %>' />
                                                <%# Eval("BrandCountry.Name")%></span>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="brands-descr">
                                <%#Eval("BriefDescription")%>
                            </div>
                        </li>
                    </ItemTemplate>
                    <GroupTemplate>
                        <li runat="server" id="groupLi"></li>
                    </GroupTemplate>
                    <GroupSeparatorTemplate>
                        <li class="clear"></li>
                    </GroupSeparatorTemplate>
                </asp:ListView>
                <br class="clear" />
                <adv:AdvPaging runat="server" ID="paging" />
            </div>
        </div>
    </div>
</asp:Content>
