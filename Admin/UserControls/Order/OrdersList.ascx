<%@ Control Language="C#" AutoEventWireup="true" CodeFile="OrdersList.ascx.cs" Inherits="UserControls_OrdersSearch" %>
<%@ Import Namespace="AdvantShop.Catalog" %>
<asp:UpdatePanel runat="server" ChildrenAsTriggers="true">
    <ContentTemplate>
        <table>
            <tr>
                <th colspan="2" align="left">
                    <asp:Localize ID="Localize1" runat="server" Text="<%$ Resources: Resource, Admin_OrderSearch_OrdersWithStatuses %>"></asp:Localize>
                </th>
            </tr>
            <tr>
                <td style="width: 240px;">
                    <asp:SqlDataSource ID="sdsStatuses" runat="server" SelectCommand="SELECT OrderStatusID, StatusName FROM [Order].OrderStatus"
                        OnInit="sdsStatuses_Init"></asp:SqlDataSource>
                    <div style="width: 250px;">
                        <asp:Repeater ID="rptStatuses" DataSourceID="sdsStatuses" runat="server">
                            <ItemTemplate>
                                <div style="width: 110px; float: left;">
                                    <asp:CheckBox ID="CheckBox1" Value='<%# Eval("OrderStatusId") %>' runat="server"
                                        Text='<%# Eval("StatusName") %>' Checked="true" />
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </td>
            </tr>
        </table>
        <table>
            <tr style="text-align: left;">
                <th style="text-align: right">
                    <asp:Label ID="Label1" Width="130" runat="server" Text='<%$ Resources: Resource, Admin_OrderSearch_DateFrom %>'></asp:Label>
                </th>
                <td>
                    <asp:TextBox ID="txtDateFrom" runat="server" Font-Size="10px" Width="75"></asp:TextBox>
                </td>
                <td style="margin-right: 5px;">
                    <asp:Image ID="popupDateFrom" runat="server" ImageUrl="~/Admin/images/Calendar_scheduleHS.png" />
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDateFrom"
                        PopupButtonID="popupDateFrom" CssClass="popupCalendar">
                    </ajaxToolkit:CalendarExtender>
                </td>
            </tr>
            <tr style="text-align: left">
                <th style="text-align: right;">
                    <asp:Label ID="Label2" Width="130" runat="server" Text='<%$ Resources: Resource, Admin_OrderSearch_DateTo %>'></asp:Label>
                </th>
                <td>
                    <asp:TextBox ID="txtDateTo" runat="server" Font-Size="10px" Width="75"></asp:TextBox>
                </td>
                <td style="margin-right: 5px;">
                    <asp:Image ID="popupDateTo" runat="server" ImageUrl="~/Admin/images/Calendar_scheduleHS.png" />
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtDateTo"
                        PopupButtonID="popupDateTo" CssClass="popupCalendar">
                    </ajaxToolkit:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td colspan="3" align="right">
                    <adv:OrangeRoundedButton runat="server" OnClick="btnFilterClick" Text="<%$ Resources: Resource, Admin_ViewOrder_Filter %>"
                        ID="btnFilter" CausesValidation="false" />
                </td>
            </tr>
        </table>
        <hr />
        <asp:SqlDataSource ID="sdsStatusesFiltered" runat="server" OnInit="sds_Init" SelectCommand="SELECT OrderStatusID, StatusName FROM [Order].OrderStatus"
            FilterExpression="OrderStatusID in {0}">
            <FilterParameters>
                <asp:Parameter Name="Statuses" Type="String" />
            </FilterParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="sdsOrders" DataSourceMode="DataSet" runat="server" OnInit="sds_Init"
            SelectCommand="SELECT OrderID, (SELECT LastName + ' ' +  FirstName FROM [Customers].[Customer] WHERE [Customer].[ID] = [Order].[CustomerID]) as CustomerName, OrderDate, OrderStatusID, Sum, TaxCost, ShippingCost, OrderDiscount, CurrencyCode, CurrencyValue FROM [Order].[Order] WHERE OrderDate > @DateFrom and OrderDate < @DateTo">
            <SelectParameters>
                <asp:Parameter Name="DateFrom" DefaultValue="01/01/1801" />
                <asp:Parameter Name="DateTo" Type="DateTime" DefaultValue="01/01/3000" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:Repeater ID="rptOrderBlocks" DataSourceID="sdsStatusesFiltered" runat="server"
            OnItemDataBound="rptOrderBlocks_ItemBound" OnDataBinding="rptOrderBlocks_DataBinding">
            <ItemTemplate>
                <asp:Panel ID="pnStatusBlock" Style="margin-left: 5px;" runat="server">
                    <asp:HiddenField ID="hfStatusID" Value='<%# Eval("OrderStatusId") %>' runat="server" />
                    <br />
                    <asp:Label Text='<%# Eval("StatusName") + ":" %>' ID="StatusID" runat="server" Font-Bold="true"></asp:Label>
                    <br />
                    <div style="margin-left: 10px;">
                        <asp:Repeater ID="rptOrders" runat="server">
                            <ItemTemplate>
                                <div>
                                    <asp:HiddenField ID="hfOrderID" Value='<%# Eval("OrderID") %>' runat="server" />
                                    <asp:SqlDataSource ID="sdsItems" runat="server" OnInit="sds_Init" SelectCommand="SELECT Name, Price, CurrencyCode, CurrencyValue, Amount FROM [Order].[OrderedCart] left join [Order].[OrderCurrency] on [OrderCurrency].[OrderID] = [OrderedCart].[OrderID] WHERE [OrderCurrency].[OrderID] = @OrderID">
                                        <SelectParameters>
                                            <asp:ControlParameter Name="OrderID" ControlID="hfOrderID" PropertyName="Value" Type="Int32" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                    <asp:HyperLink ID="orderLink" CausesValidation="false" Font-Bold='<%# Convert.ToInt32(Eval("OrderID")) == SelectedOrder %>'
                                        CssClass="blueLink orderLink" NavigateUrl='<%# "~/Admin/EditOrder.aspx?OrderID=" + Eval("OrderID") %>'
                                        Text='<%# Eval("OrderID") + " - " + Eval("CustomerName") %>' runat="server"></asp:HyperLink>
                                    <br />
                                    <%--<asp:Panel CssClass="orderPanel" runat="server" Style="display: none;">
                                        <table width="600px">
                                            <tr>
                                                <td colspan="2">
                                                    <b>
                                                        <%=Resources.Resource.Admin_ViewOrder_Date%></b>
                                                    <asp:Label ID="lOrderDate" Text='<%# AdvantShop.Localization.Culture.ConvertDate((DateTime)Eval("OrderDate")) %>'
                                                        runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr valign="top">
                                                <td style="width: 300px;">
                                                    <asp:Label runat="server" Text="<%$ Resources:Resource, Admin_ViewOrder_OrderItem %>"
                                                        Font-Bold="true"></asp:Label>
                                                    <br />
                                                    <asp:Repeater runat="server" DataSourceID="sdsItems">
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# String.Format("{0} * {1} = {2}", Eval("Name"), Eval("Amount"), CatalogService.GetStringPrice((decimal)Eval("Price"), 1, (string)Eval("CurrencyCode"),Convert.ToDecimal(Eval("CurrencyValue"))) ) %>'></asp:Label>
                                                            <br />
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </td>
                                                <td style="width: 300px;">
                                                    <b>
                                                        <%=Resources.Resource.Admin_ViewOrder_ItemCost2%></b><asp:Label ID="Label5" runat="server"
                                                            Text='<%# CatalogService.GetStringPrice(((((decimal)Eval("Sum") - (decimal)Eval("ShippingCost")) * 100) / (100 - Convert.ToDecimal(Eval("OrderDiscount")))), Convert.ToDecimal(Eval("CurrencyValue")), Eval("CurrencyCode").ToString()) %>'></asp:Label><br />
                                                    <b>
                                                        <%=Resources.Resource.Admin_ViewOrder_ItemDiscount%></b><asp:Label ID="Label6" runat="server"
                                                            Text='<%# "-" + CatalogService.GetStringPrice((((((decimal)Eval("Sum") - (decimal)Eval("ShippingCost")) * 100) / (100 - Convert.ToDecimal(Eval("OrderDiscount")))) * (Convert.ToDecimal(Eval("OrderDiscount")) / 100)), Convert.ToDecimal(Eval("CurrencyValue")), Eval("CurrencyCode").ToString()) %>'></asp:Label><br />
                                                    <b>
                                                        <%=Resources.Resource.Admin_ViewOrder_ShippingPrice%></b><asp:Label ID="Label3" runat="server"
                                                            Text='<%# "+" + CatalogService.GetStringPrice((decimal)Eval("ShippingCost"), Convert.ToDecimal(Eval("CurrencyValue")),Eval("CurrencyCode").ToString()) %>'></asp:Label>
                                                    <br />
                                                    <b>
                                                        <%=Resources.Resource.Admin_ViewOrder_Taxes%></b><asp:Label ID="Label7" runat="server"
                                                            Text='<%# "+" + CatalogService.GetStringPrice((decimal)Eval("TaxCost"), Convert.ToDecimal(Eval("CurrencyValue")), Eval("CurrencyCode").ToString()) %>'></asp:Label>
                                                    <br />
                                                    <b>
                                                        <%=Resources.Resource.Admin_ViewOrder_TotalPrice%></b><asp:Label ID="Label4" runat="server"
                                                            Text='<%# CatalogService.GetStringPrice((decimal)Eval("Sum"), Convert.ToDecimal(Eval("CurrencyValue")),Eval("CurrencyCode").ToString()) %>'></asp:Label><br />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>--%>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </asp:Panel>
            </ItemTemplate>
        </asp:Repeater>
        <table width="100%" runat="server" id="tblPager">
            <tr align="center">
                <td align="left">
                    <asp:LinkButton ID="lbPagePrev" runat="server" CausesValidation="false" OnClick="lbPagePrev_Click"
                        Text="<%$ Resources: Resource, Admin_OrderList_Prev %>"></asp:LinkButton>
                </td>
                <td>
                    <asp:DropDownList ID="ddlPage" runat="server" OnSelectedIndexChanged="ddlPage_Selected"
                        AutoPostBack="true">
                    </asp:DropDownList>
                </td>
                <td style="text-align:right">
                    <asp:LinkButton ID="lbPageNext" runat="server" CausesValidation="false" OnClick="lbPageNext_Click"
                        Text="<%$ Resources: Resource, Admin_OrderList_Next %>"></asp:LinkButton>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
<%--<script type="text/javascript">
    Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
        $(".orderLink").tooltip({
            delay: 10,
            showURL: false,
            bodyHandler: function () {
                return $(this).parent().find(".orderPanel").html();
            }
        });
    });
</script>
--%>