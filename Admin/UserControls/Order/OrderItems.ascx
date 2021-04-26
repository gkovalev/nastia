<%@ Control Language="C#" AutoEventWireup="true" CodeFile="OrderItems.ascx.cs" Inherits="Admin_UserControls_Order_OrderItems" %>
<%@ Register Src="~/Admin/UserControls/PopupTreeView.ascx" TagName="PopupTree" TagPrefix="adv" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="AdvantShop.Catalog" %>
<%@ Import Namespace="AdvantShop.Orders" %>
<adv:PopupTree runat="server" ID="pTreeProduct" HeaderText="<%$ Resources:Resource, Admin_CatalogLinks_ParentCategory %>"
    Type="CategoryProduct" ExceptId="0" OnTreeNodeSelected="pTreeProduct_NodeSelected"
    OnHiding="pTreeProduct_Hiding" />
<asp:Label runat="server" ID="lblError" Visible="false"></asp:Label>
<table style="width: 100%">
    <tr>
        <td style="width: 300px">
            <asp:SqlDataSource runat="server" ID="sdsCurrs" OnInit="sds_Init" SelectCommand="SELECT * FROM [Catalog].[Currency]">
            </asp:SqlDataSource>
            <span>
                <% = Resources.Resource.Admin_ViewOrder_ChoosingCurrency%>: </span>
            <asp:DropDownList ID="ddlCurrs" runat="server" DataSourceID="sdsCurrs" DataTextField="Name"
                DataValueField="CurrencyIso3" AutoPostBack="true" CausesValidation="false" OnSelectedIndexChanged="ddlCurrs_SelectedChanged" Visible="false">
            </asp:DropDownList>
            <asp:Label runat="server" ID="lcurrency" />
            <asp:HiddenField runat="server" ID="hfOldCurrencyValue" />
        </td>
        <td>
            <asp:Label runat="server" ID="lDiscount" Text="<%$ Resources: Resource, Admin_EditOrder_Discount%>"></asp:Label>:
            <asp:TextBox runat="server" ID="txtDiscount"></asp:TextBox>
            %
        </td>
        <td>
            <div style="float: right;">
                <adv:OrangeRoundedButton CausesValidation="false" ID="btnAddProduct" runat="server"
                    OnClick="btnAddProduct_Click" Text="<%$ Resources: Resource, Admin_OrderSearch_AddProduct%>" />
            </div>
        </td>
    </tr>
    <tr class="formheaderfooter">
        <td colspan="2">
        </td>
    </tr>
</table>
<div style="text-align: center;">
    <asp:Repeater ID="DataListOrderItems" runat="server" OnItemCommand="dlItems_ItemCommand"
        OnItemDataBound="dlItems_ItemDataBound">
        <HeaderTemplate>
            <table width="100%" border="0" cellspacing="0" cellpadding="3" class="grid-main">
                <tr class="header">
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <b>
                            <asp:Label ID="Label15" runat="server" Text="<%$ Resources:Resource, Admin_ViewOrder_ArtNo %>"></asp:Label></b>
                    </td>
                    <td>
                        <b>
                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Resource, Admin_ViewOrder_ItemName %>"></asp:Label></b>
                    </td>
                    <td>
                        <b>
                            <asp:Label ID="Label14" runat="server" Text="<%$ Resources:Resource, Admin_ViewOrder_CustomOptions %>"></asp:Label></b>
                    </td>
                    <td style="width: 150px; text-align: center;">
                        <b>
                            <asp:Label ID="Label13" runat="server" Text="<%$ Resources:Resource, Admin_ViewOrder_Price %>"></asp:Label>
                        </b>
                    </td>
                    <td style="width: 100px; text-align: center;">
                        <b>
                            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Resource, Admin_ViewOrder_ItemAmount %>"></asp:Label>
                        </b>
                    </td>
                    <% if (AdvantShop.Configuration.SettingsOrderConfirmation.AmountLimitation)
                       {%>
                    <td class="OrderTableHead" style="width: 100px;">
                        <asp:Localize ID="Localize_Client_ShoppingCart_Available" runat="server" Text="<%$ Resources:Resource, Client_ShoppingCart_AvailableHeader %>"></asp:Localize>
                    </td>
                    <%
                       }%>
                    <td style="width: 150px; text-align: center;">
                        <b>
                            <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Resource, Admin_ViewOrder_ItemCost %>"></asp:Label>
                        </b>
                    </td>
                    <td style="width: 30px">
                    </td>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr class="row1" style="height: 35px;">
                <td>
                    <%# (EnumItemType)Eval("ItemType") == EnumItemType.Product ? RenderPicture((int)Eval("EntityID")) :string.Format("<img src=\"{0}\" alt=\"\"/>", AdvantShop.Core.UrlRewriter.UrlService.GetAbsoluteLink("images/giftcertificate/certifacate_small.jpg"))%>
                    <asp:HiddenField ID="hfItemType" runat="server" Value='<%# (EnumItemType) Eval("ItemType") %>' />
                </td>
                <td>
                    <asp:Literal runat="server" ID="ltArtNo" Text='<%#Eval("ArtNo")%>'></asp:Literal>
                </td>
                <td>
                    <%# (EnumItemType)Eval("ItemType") == EnumItemType.Product ? Eval("Name") : "<a href=\"javascript:open_window('m_Certificate.aspx?ID=" + HttpUtility.UrlEncode(HttpUtility.UrlEncode(Eval("EntityID").ToString())) + "',750,600);\" class='editbtn showtooltip' title=" + Resources.Resource.Admin_MasterPageAdminCatalog_Edit + ">" + Eval("Name") + "</a>"%>
                </td>
                <td>
                    <%#RenderSelectedOptions((IList<EvaluatedCustomOptions>)Eval("SelectedOptions"))%>
                </td>
                <td style="text-align: center;">
                     <!--Added By Evgeni to change item price-->
                     <nobr> <asp:TextBox ID="txtSinglePrice" runat="server" Text='<%# CatalogService.GetStringPrice(Convert.ToDecimal(Eval("Price"))) %>' ></asp:TextBox>
                   <asp:ImageButton CausesValidation="false" ID="ImageButton1" ImageUrl="~/Admin/images/refresh.png"
                        runat="server" CommandArgument='<%# Eval("Id") %>' CommandName="SaveSinglePrice" />
                        </nobr>
                </td>
                <td style="text-align: center;">
                    <asp:TextBox ID="txtQuantity" runat="server" Text='<%# Eval("Amount") %>' Width="25"
                        Visible='<%# (EnumItemType)Eval("ItemType") == EnumItemType.Product %>'></asp:TextBox>
                    <asp:Label ID="lblQuantity" runat="server" Text='<%# Eval("Amount") %>' Visible='<%# (EnumItemType)Eval("ItemType") == EnumItemType.Certificate %>'></asp:Label>
                    <asp:ImageButton CausesValidation="false" ID="btnQuantUp" ImageUrl="~/Admin/images/refresh.png"
                        runat="server" CommandArgument='<%# Eval("Id") %>' CommandName="SaveQuantity"
                        Visible='<%# (EnumItemType)Eval("ItemType") == EnumItemType.Product %>' />
                </td>
                <% if (AdvantShop.Configuration.SettingsOrderConfirmation.AmountLimitation)
                   {%>
                <td class="OrderTable_td" style="width: 100px;">
                    <asp:Label ID="lbMaxCount" runat="server" Text="Label" ForeColor="Red" CssClass="lbMaxCount"
                        Font-Bold="true" Visible='<%# (EnumItemType)Eval("ItemType") == EnumItemType.Product %>'></asp:Label>
                </td>
                <%
                   }%>
                <td style="text-align: center;">
                    <%# CatalogService.GetStringPrice(Convert.ToDecimal(Eval("Price")), Convert.ToInt32(Eval("Amount")), CurrencyCode, CurrencyValue)%>
                </td>
                <td>
                    <asp:ImageButton runat="server" ID="btnDelete" CausesValidation="false" CommandArgument='<%#  Eval("Id") %>'
                        CommandName="Delete" ImageUrl="~/Admin/images/deletebtn.png" />
                </td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr class="row2" style="height: 35px;">
                <td>
                     <%# (EnumItemType)Eval("ItemType") == EnumItemType.Product ? RenderPicture((int)Eval("EntityID")) :string.Format("<img src=\"{0}\" alt=\"\"/>", AdvantShop.Core.UrlRewriter.UrlService.GetAbsoluteLink("images/giftcertificate/certifacate_small.jpg"))%>
                    <asp:HiddenField ID="hfItemType" runat="server" Value='<%# (EnumItemType)Eval("ItemType") %>' />
                </td>
                <td>
                    <asp:Literal runat="server" ID="ltArtNo" Text='<%#Eval("ArtNo")%>'></asp:Literal>
                </td>
                <td>
                    <%# (EnumItemType)Eval("ItemType") == EnumItemType.Product ? Eval("Name") : "<a href=\"javascript:open_window('m_Certificate.aspx?ID=" + HttpUtility.UrlEncode(HttpUtility.UrlEncode(Eval("EntityID").ToString())) + "',750,600);\" class='editbtn showtooltip' title=" + Resources.Resource.Admin_MasterPageAdminCatalog_Edit + ">" + Eval("Name") + "</a>"%>
                </td>
                <td>
                    <%#RenderSelectedOptions((IList<EvaluatedCustomOptions>)Eval("SelectedOptions"))%>
                </td>
                <td style="text-align: center;">
                                    <!--Added By Evgeni to change item price-->
                     <nobr> <asp:TextBox ID="txtSinglePrice" runat="server" Text='<%# CatalogService.GetStringPrice(Convert.ToDecimal(Eval("Price"))) %>' ></asp:TextBox>
                   <asp:ImageButton CausesValidation="false" ID="ImageButton1" ImageUrl="~/Admin/images/refresh.png"
                        runat="server" CommandArgument='<%# Eval("Id") %>' CommandName="SaveSinglePrice" />
                        </nobr>
                         </td>
                <td style="text-align: center;">
                    <asp:TextBox ID="txtQuantity" runat="server" Text='<%# Eval("Amount") %>' Width="25"
                        Visible='<%# (EnumItemType)Eval("ItemType") == EnumItemType.Product %>'></asp:TextBox>
                    <asp:Label ID="lblQuantity" runat="server" Text='<%# Eval("Amount") %>' Visible='<%# (EnumItemType)Eval("ItemType") == EnumItemType.Certificate %>'></asp:Label>
                    <asp:ImageButton CausesValidation="false" ID="btnQuantUp" ImageUrl="~/Admin/images/refresh.png"
                        runat="server" CommandArgument='<%# Eval("Id") %>' CommandName="SaveQuantity"
                        Visible='<%# (EnumItemType)Eval("ItemType") == EnumItemType.Product %>' />
                </td>
                <% if (AdvantShop.Configuration.SettingsOrderConfirmation.AmountLimitation)
                   {%>
                <td class="OrderTable_td" style="width: 100px;">
                    <asp:Label ID="lbMaxCount" runat="server" Text="Label" ForeColor="Red" CssClass="lbMaxCount"
                        Font-Bold="true" Visible='<%# (EnumItemType)Eval("ItemType") == EnumItemType.Product %>'></asp:Label>
                </td>
                <%
                   }%>
                <td style="text-align: center;">
                    <%# CatalogService.GetStringPrice((decimal)Eval("Price"), Convert.ToInt32(Eval("Amount")), CurrencyCode, CurrencyValue)%>
                </td>
                <td>
                    <asp:ImageButton CausesValidation="false" runat="server" ID="btnDelete" CommandArgument='<%# Eval("Id") %>'
                        CommandName="Delete" ImageUrl="~/Admin/images/deletebtn.png" />
                </td>
            </tr>
        </AlternatingItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
</div>
