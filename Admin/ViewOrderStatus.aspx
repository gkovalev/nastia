<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/MasterPageAdmin.master" AutoEventWireup="true"
    CodeFile="ViewOrderStatus.aspx.cs" Inherits="Admin_ViewOrderStatus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div style="text-align: center;" >
        <asp:Label ID="lblCustomer" runat="server" CssClass="AdminHead" Text="<%$ Resources:Resource, Admin_ViewOrderStatus_Header %>"></asp:Label>
        <br/>
        <asp:Label ID="lblCustomerName" CssClass="AdminSubHead" runat="server" Text="<%$ Resources:Resource, Admin_ViewOrderStatus_SubHeader %>"></asp:Label>
        <br/>
    </div>
    <br />
    <div style="text-align: center;" >
        <asp:Label ID="lblSplit3" runat="server" Text="<%$ Resources:Resource, Admin_Slash%>"></asp:Label>
        <asp:LinkButton ID="LinkButton1" runat="server" CssClass="Link" Text="<%$ Resources:Resource, Admin_HeadCmdInsert %>"></asp:LinkButton>
        <asp:Label ID="lblSplit2" runat="server" Text="<%$ Resources:Resource, Admin_Slash%>"></asp:Label>
        <asp:LinkButton ID="LinkButton2" runat="server" CssClass="Link" Text="<%$ Resources:Resource, Admin_HeadCmdHide %>" Visible="False"></asp:LinkButton>
        <asp:Label ID="lblSplit1" runat="server" Text="<%$ Resources:Resource, Admin_Slash%>" Visible="False"></asp:Label>
    </div>
    <br />
    <div style="text-align: center;" >
        <asp:GridView ID="GridView2" runat="server" DataKeyNames="OrderID" DataSourceID="SqlDataSource1"
                      AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" CellPadding="3"
                      CssClass="GridViewStyle" Width="98%">
            <Columns>
                <asp:TemplateField SortExpression = "OrderID" >
                    <ItemStyle HorizontalAlign="Left" Width="75px" />
                    <ControlStyle CssClass="Link" />
                    <HeaderTemplate>
                        <%=Resources.Resource.Admin_ViewOrderStatus_OrderNum%></HeaderTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1" CommandName = "Select" PostBackUrl = '<%# "ViewOrder.aspx?OrderID=" + Eval("OrderID") %>'  runat="server"><%# Eval("OrderID") %></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                
                
                <asp:BoundField DataField="StatusName" HeaderStyle-ForeColor="Black" HeaderText="<%$ Resources:Resource, Admin_ViewOrderStatus_Status %>"
                                SortExpression="OrderStatusID" />
                
                <asp:TemplateField>
                    <ItemStyle HorizontalAlign="Left"  />
                    <ControlStyle CssClass="Link" />
                    <HeaderTemplate>
                        <%=Resources.Resource.Admin_ViewOrderStatus_Added%></HeaderTemplate>
                    <ItemTemplate>
                        <%=Resources.Resource.Admin_ViewOrderStatus_Date%>
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField>
                    <ItemStyle HorizontalAlign="Left" />
                    <ControlStyle CssClass="Link" />
                    <HeaderTemplate>
                        <%=Resources.Resource.Admin_ViewOrderStatus_Byer%></HeaderTemplate>
                    <ItemTemplate>
                        <a href = "">
                            <%=Resources.Resource.Admin_ViewOrderStatus_SiteAdmin%></a>
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField>
                    <ItemStyle HorizontalAlign="Left"  />
                    <ControlStyle CssClass="Link" />
                    <HeaderTemplate>
                        <%=Resources.Resource.Admin_ViewOrderStatus_Payment%></HeaderTemplate>
                    <ItemTemplate>
                        <%=Resources.Resource.Admin_ViewOrderStatus_Cash%> 
                    </ItemTemplate>
                </asp:TemplateField>                
                <asp:TemplateField>
                    <ItemStyle HorizontalAlign="Left"  />
                    <ControlStyle CssClass="Link" />
                    <HeaderTemplate>
                        <%=Resources.Resource.Admin_ViewOrderStatus_Shipping%></HeaderTemplate>
                    <ItemTemplate>
                        <%=Resources.Resource.Admin_ViewOrderStatus_Export%>
                    </ItemTemplate>
                </asp:TemplateField>                
                <asp:TemplateField>
                    <ItemStyle HorizontalAlign="Left" />
                    <ControlStyle CssClass="Link" />
                    <HeaderTemplate>
                        <%=Resources.Resource.Admin_ViewOrderStatus_Price%></HeaderTemplate>
                    <ItemTemplate>
                        <%=Resources.Resource.Admin_ViewOrderStatus_PriceVal%>
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
            <RowStyle CssClass="GridView_RowStyle" />
            <FooterStyle CssClass="GridView_FooterStyle" Font-Bold="True" />
            <EditRowStyle CssClass="GridView_EditRowStyle" />
            <SelectedRowStyle CssClass="GridView_SelectedRowStyle" />
            <HeaderStyle CssClass="GridView_HeaderStyle" />
            <AlternatingRowStyle CssClass="GridView_AlternatingRowStyle" />
            <PagerStyle CssClass="GridView_PagerStyle" HorizontalAlign="Center" ForeColor="Black" />
        </asp:GridView>
    </div>
    <div style="text-align: center;" >
        <asp:Label ID="Message" runat="server" Font-Bold="True" ForeColor="Red" Visible="False"></asp:Label><br/>
    </div>
    <br />
    <div style="text-align: center;" >
        <table width="98%">
            <tr align="left">
                <td>&nbsp;</td>
            </tr>
        </table>
    </div>
    <br />
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        SelectCommand="SELECT [Order].[Order].OrderID, [Order].[Order].ShippingMethod, [Order].[Order].PaymentMethod, [Order].OrderStatus.StatusName, Customers.Customer.FirstName, [Order].OrderStatus.OrderStatusID, Customers.Customer.LastName FROM [Order].[Order] INNER JOIN [Order].OrderStatus ON [Order].[Order].OrderStatusID = [Order].OrderStatus.OrderStatusID INNER JOIN Customers.Customer ON [Order].[Order].CustomerID = Customers.Customer.ID ORDER BY [Order].OrderStatus.OrderStatusID" OnInit="SqlDataSource1_Init">
    </asp:SqlDataSource>
    <br />
</asp:Content>
