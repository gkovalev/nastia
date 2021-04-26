<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BestSellers.ascx.cs" Inherits="UserControls_BestSellers" %>
<%@ Import Namespace="AdvantShop.Core.UrlRewriter" %>
<!--noindex-->
<%
    if (this.Visible)
    {
        if (DtBestSellers.Count > 0)
        {
            if (DtBestSellers.Count >= 4)
            {
%>
<img class="block_shadow" src="~/images/block_l_sh.jpg" style="margin-top: -2px"
    alt="" />
<%
            }
%>
<div class="block" style="margin-top: -7px;">
    <div class="block_header">
        <div class="block_name">
            <%=Resources.Resource.Client_UserControls_BestSellers_Bestsellers%></div>
    </div>
    <div class="block_middle">
        <div class="block_content">
            <asp:Repeater ID="repeater" runat="server">
                <HeaderTemplate>
                    <ul style="padding: 0px; margin: 0px; margin-left: 20px;">
                </HeaderTemplate>
                <ItemTemplate>
                    <li><a runat="server" href='<%# UrlService.GetLink(ParamType.Product, Eval("UrlPath").ToString() ,Convert.ToInt32(Eval("ProductID"))) %>'
                        class="LeafLink">
                        <%# Eval("Name")%></a> </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
    <asp:Label ID="lblErr" runat="server" Text="Label" ForeColor="Blue"></asp:Label>
    <div class="block_footer">
    </div>
</div>
<%}
    }%>
<!--/noindex-->
