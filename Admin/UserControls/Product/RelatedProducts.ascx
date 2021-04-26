<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RelatedProducts.ascx.cs"
    Inherits="Admin_UserControls_RelatedProducts" %>
<%@ Import Namespace="AdvantShop.Configuration" %>
<%@ Register TagPrefix="adv" TagName="PopupTree" Src="~/Admin/UserControls/PopupTreeView.ascx" %>
<asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="popTree" EventName="TreeNodeSelected" />
        <asp:AsyncPostBackTrigger ControlID="popTree" EventName="Hiding" />
    </Triggers>
    <ContentTemplate>
            <adv:PopupTree runat="server" ID="popTree" OnTreeNodeSelected="popTree_Selected"
                Type="CategoryProduct" HeaderText="<%$ Resources:Resource, Admin_CatalogLinks_ParentCategory %>" />
        <table class="table-p">
            <tr>
                <td class="formheader">
                    <h2>
                        <span style="margin-left: 3px;">
                            <%=SettingsCatalog.GetRelatedProductName(RelatedType)%>
                        </span>
                    </h2>
                </td>
            </tr>
            <tr style="height: 40px;">
                <td>
                    <asp:Label ID="lRelatedProductsMessage" runat="server" CssClass="mProductLabelInfo"
                        EnableViewState="False" Font-Names="Verdana" Font-Size="14px" ForeColor="Red"
                        Visible="False" />
                    <asp:LinkButton ID="lbAddRelatedProduct" runat="server" OnClientClick="document.body.style.overflowX='hidden';_TreePostBack=true;removeunloadhandler();"
                        OnClick="lbAddRelatedProduct_Click" EnableViewState="false"><%=Resources.Resource.Admin_Product_AddRelatedProduct%></asp:LinkButton>
                </td>
            </tr>
            <tr style="height: 40px;">
                <td>
                    <asp:Label ID="Label30" runat="server" Text="<%$ Resources:Resource, Admin_Product_CurrentRelatedProducts %>"  EnableViewState="false"/>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Repeater ID="rRelatedProducts" runat="server" OnItemCommand="rRelatedProducts_ItemCommand">
                        <HeaderTemplate>
                            <ol style="list-style: none; padding: 0; margin: 0;">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <li><%# Container.ItemIndex +1 %>.&nbsp;<a href='<%#"Product.aspx?ProductID=" + Eval("ProductID")%>' class="Link"><%#Eval("Name")%></a>
                                <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%#Eval("ProductID")%>'
                                    OnClientClick="removeunloadhandler();" CommandName="DeleteRelatedProduct">
                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Admin/images/remove.jpg" EnableViewState="false" />
                                </asp:LinkButton>
                            </li>
                        </ItemTemplate>
                        <FooterTemplate>
                            </ol>
                        </FooterTemplate>
                    </asp:Repeater>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
