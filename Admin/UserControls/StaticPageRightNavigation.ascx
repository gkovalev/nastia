<%@ Control Language="C#" AutoEventWireup="true" CodeFile="StaticPageRightNavigation.ascx.cs"
    Inherits="Admin_UserControls__StaticPageRightNavigation" %>
<asp:UpdatePanel ID="UpdateCboCategory" runat="server" ChildrenAsTriggers="true"
    UpdateMode="Conditional">
    <ContentTemplate>
        <div>
            <div class="rightPanelHeader">
                <div style="width: 30px; float: left">
                    <img src="~/admin/images/folder.gif" alt="" />
                </div>
                <div style="width: 187px; float: left">
                    <asp:Label ID="lblBigHead" runat="server" Text="<%$ Resources:Resource, Admin_StaticPage_lblSubMain %>"
                        Font-Bold="true" />
                </div>
                <div style="width: 30px; float: left">
                    <asp:HyperLink ID="hlStaticPage" NavigateUrl="../StaticPage.aspx" runat="server"><img style="border:none;" class="showtooltip" src="~/admin/images/gplus.gif" title="<%=Resources.Resource.Admin_UserControl_StaticPageRightNavigation_AddStaticPage%>"
                                            onmouseover="this.src='~/admin/images/bplus.gif';"
                                            onmouseout="this.src='~/admin/images/gplus.gif';" /></asp:HyperLink>
                </div>
            </div>
            <div style="clear: both">
                <div class="admin_product_categoryListBlock">
                    <adv:CommandTreeView ID="tree" runat="server" NodeWrap="True" ShowLines="true" CssClass="AdminTree_MainClass"
                        OnTreeNodePopulate="tree_TreeNodePopulate">
                        <ParentNodeStyle Font-Bold="False" />
                        <SelectedNodeStyle ForeColor="#027dc2" Font-Bold="true" HorizontalPadding="0px" VerticalPadding="0px" />
                        <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" HorizontalPadding="5px"
                            NodeSpacing="0px" VerticalPadding="0px" />
                    </adv:CommandTreeView>
                    <%--<asp:Repeater runat="server" OnItemCommand="rContent_ItemCommand" ID="rContent">
                            <HeaderTemplate>
                                <table style="width: 100%">
                                    <tbody>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td style="width: 14px;">
                                        <img src='~/admin/images/blank.gif' alt="" />
                                    </td>
                                    <td style="width: 189px;">
                                        <a href='<%# "StaticPage.aspx?PageID=" + Eval("StaticPageID")%>'>
                                            <%# Eval("PageName")%></a>
                                    </td>
                                    <td>
                                        <td style="width: 14px;">
                                            <asp:ImageButton runat="server" ID="ibDel" CssClass="showtooltip" ImageUrl="~/admin/images/gcross.gif"
                                                CommandName="DeletePage" CommandArgument='<%# Eval("StaticPageID")%>' Enabled='<%# (!AdvantShop.Demo.IsDemoEnabled()) %>'
                                                ToolTip="<%$Resources:Resource, Admin_UserControl_StaticPageRightNavigation_Deleting%>" />
                                            <ajaxToolkit:ConfirmButtonExtender ID="ConfirmButtonExtenderDel" runat="server" TargetControlID="ibDel"
                                                ConfirmText='<%$Resources:Resource, Admin_UserControl_StaticPageRightNavigation_ConfirmDeleting%>'>
                                            </ajaxToolkit:ConfirmButtonExtender>
                                        </td>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </tbody> </table>
                            </FooterTemplate>
                        </asp:Repeater>--%>
                </div>
                <%--  <% if (Paging.PageCount > 1)
                       { %>
                    <table id="pager" style="width: 100%; background-color: #EFF0F1; text-align: center;">
                        <tr>
                            <td>
                                <asp:LinkButton ID="lbPreviousPage" CssClass="Link" runat="server" Enabled="false"
                                    EnableViewState="false" OnClientClick="removeunloadhandler()" Text="<%$Resources:Resource, Admin_Product_Prev%>"
                                    OnClick="lbPreviousPage_Click"> </asp:LinkButton>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCurrentPage" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCurrentPage_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:LinkButton ID="lbNextPage" CssClass="Link" runat="server" Enabled="false" EnableViewState="false"
                                    OnClientClick="removeunloadhandler()" Text="<%$Resources:Resource, Admin_Product_Next %>"
                                    OnClick="lbNextPage_Click">
                                </asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                    <% }  %>--%>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
